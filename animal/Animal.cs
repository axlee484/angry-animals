using Godot;
using System;
using System.Diagnostics;

public partial class Animal : RigidBody2D
{
    private SignalManager signalManager;
    private bool isAlive;
    private bool isGrabbed;
    private bool isReleased;
    private bool isSoundReady;
    private bool isReleaseRequested;
    private Vector2 initialMousePosition;
    private Vector2 initialAnimalPosition;
    private AudioStreamPlayer2D stretchSoundNode;
    private AudioStreamPlayer2D releaseSoundNode;
    [Export] private Vector2 STRETCH_LIMIT = new(-80,80);
    [Export] private float LAUNCH_IMPULSE = 20.0f;

    public override void _Ready()
    {
        isAlive = true;
        isGrabbed = false;
        isReleased = false;
        isSoundReady = false;
        signalManager = GetNode<SignalManager>("/root/SignalManager");
        stretchSoundNode = GetNode<AudioStreamPlayer2D>("StretchSound");
        releaseSoundNode = GetNode<AudioStreamPlayer2D>("LaunchSound");
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

        if(!stretchSoundNode.Playing && isSoundReady) {
            stretchSoundNode.Play();
            isSoundReady = false;
        }
        GlobalPosition = newPosition;

    }

    private void Release()
    {
        isGrabbed = false;
        isReleased = true;
        Freeze = false;
        var stretchAmount = GlobalPosition - initialAnimalPosition;
        ApplyCentralImpulse(-1*LAUNCH_IMPULSE*stretchAmount);
        isReleaseRequested = false;
        releaseSoundNode.Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(isGrabbed && !isReleased) Stretch();
        if(isReleaseRequested) Release();
    }

    private void Die()
    {
        if(!isAlive) return;
        
        isAlive = false;
        signalManager.EmitSignal(SignalManager.SignalName.AnimalDied);
        QueueFree();
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
