using Godot;
using System;

public partial class Block : StaticBody2D
{
    private AudioStreamPlayer2D vanishSound;
    private AnimationPlayer animationPlayer;
    private SignalManager signalManager;
    public override void _Ready()
    {
       vanishSound = GetNode<AudioStreamPlayer2D>("VanishSound");
       animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
       signalManager = GetNode<SignalManager>("/root/SignalManager");
    }
    public void Die()
    {
        vanishSound.Play();
        animationPlayer.Play("vanish");
        signalManager.EmitSignal(SignalManager.SignalName.BlockDied);
        QueueFree();
    }
}
