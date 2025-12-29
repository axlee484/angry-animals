using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Animal : RigidBody2D
{
	private SignalManager signalManager;
	private GameManager gameManager;
	private ScoreManager scoreManager;
	private bool isAlive;
	private bool isGrabbed;
	private bool isReleased;
	private bool isSoundReady;
	private bool isReleaseRequested;
	private Vector2 initialMousePosition;
	private Vector2 initialAnimalPosition;
	private AudioStreamPlayer2D stretchSound;
	private AudioStreamPlayer2D releaseSound;
	private AudioStreamPlayer2D blockCollideSound;
	private int collisionCount;
	private float stoppingLimit;
	[Export] private Vector2 STRETCH_LIMIT = new(-80,80);
	[Export] private float LAUNCH_IMPULSE = 20.0f;

	public override void _Ready()
	{
		isAlive = true;
		isGrabbed = false;
		isReleased = false;
		isSoundReady = false;
		signalManager = GetNode<SignalManager>("/root/SignalManager");
		gameManager = GetNode<GameManager>("/root/GameManager");
		scoreManager = GetNode<ScoreManager>("/root/ScoreManager");
		stretchSound = GetNode<AudioStreamPlayer2D>("StretchSound");
		releaseSound = GetNode<AudioStreamPlayer2D>("LaunchSound");
		blockCollideSound = GetNode<AudioStreamPlayer2D>("BlockCollideSound");

		collisionCount = 0;
		stoppingLimit = 0.2f;
	}

	

	private void Stretch()
	{
		var mousePos = GetGlobalMousePosition();
		var stretch = mousePos - initialMousePosition;
		stretch.X = Math.Clamp(stretch.X, STRETCH_LIMIT.X, 0);
		stretch.Y = Math.Clamp(stretch.Y, 0, STRETCH_LIMIT.Y);

		var newPosition = initialAnimalPosition + stretch;

		if(newPosition == GlobalPosition) {
			isSoundReady = true;
			return;
		}

		if(!stretchSound.Playing && isSoundReady) {
			stretchSound.Play();
			isSoundReady = false;
		}
		GlobalPosition = newPosition;

	}

	private void Release()
	{
		var stretchAmount = GlobalPosition - initialAnimalPosition;
		if(stretchAmount == Vector2.Zero)
		{
			isGrabbed = false;
			isReleaseRequested = false;
			return;
		}
		isGrabbed = false;
		isReleased = true;
		Freeze = false;
		ApplyCentralImpulse(-1*LAUNCH_IMPULSE*stretchAmount);
		isReleaseRequested = false;
		releaseSound.Play();
		scoreManager.Attempts ++;
		signalManager.EmitSignal(SignalManager.SignalName.AttempComplete);
	}


	private bool IsRolling()
	{
		if(Math.Abs(AngularVelocity) <= stoppingLimit && Math.Abs(LinearVelocity.Y)<= stoppingLimit ) return false;
		return true;
	}

	private void HandleBlockCollision(Node2D body, bool newCollision)
	{
		if(newCollision) blockCollideSound.Play();
		if(IsRolling()) return;
		if(body is Block block) block.Die();
		Die();
		// GD.Print("block collision") ;
	}
	private void PlayCollision()
	{        
		var collidingBodies = GetCollidingBodies();
		var newCollision = false;
		var lastCollisionCount = collisionCount;
		collisionCount = GetContactCount();
		if(collisionCount > lastCollisionCount) {
			newCollision = true;
			collisionCount = GetContactCount();
		}
		
		foreach (var body in collidingBodies)
		{
			if(body.IsInGroup(gameManager.GROUP_BLOCK)) HandleBlockCollision(body, newCollision);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(isReleased)
		{
			PlayCollision();
		}
		if(isGrabbed && !isReleased) Stretch();
		if(isReleaseRequested) Release();
	}

	private void DefferedDie()
	{
	  if(!isAlive) return;
		isAlive = false;
		signalManager.EmitSignal(SignalManager.SignalName.AnimalDied);
		QueueFree();  
	}
	public void Die()
	{
		CallDeferred(nameof(DefferedDie));
	}

	public void OnScreenExited()
	{
		Die();
	}



	private void Grab()
	{
		isGrabbed = true;
		initialMousePosition = GetGlobalMousePosition();
		initialAnimalPosition = GlobalPosition;
		
	}
	public void OnInputEvent(Viewport _viewPort, InputEvent inputEvent, double _shapeIdx)
	{
		if (inputEvent.IsActionPressed("drag") && !isReleased) Grab(); 

	}

	public override void _Process(double delta)
	{
		EmitDebugData();
		if(Input.IsActionJustReleased("drag") && isGrabbed) isReleaseRequested = true;
	}









	private void EmitDebugData()
	{
		var debugData = @$"
        GPos: {GlobalPosition.X:f2}, {GlobalPosition.Y:f2}
        AngularVelocity: {AngularVelocity:f2}
        LinearVelocity: {LinearVelocity.X:f2}, {LinearVelocity.Y:f2}
		";
		signalManager.EmitSignal(SignalManager.SignalName.UpdateDebugLabel, debugData);
	}

}
