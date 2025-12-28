using Godot;
using System;

public partial class Block : StaticBody2D
{
    private AudioStreamPlayer2D vanishSound;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
       vanishSound = GetNode<AudioStreamPlayer2D>("VanishSound");
       animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void Die()
    {
        vanishSound.Play();
        animationPlayer.Play("vanish");
        QueueFree();
    }
}
