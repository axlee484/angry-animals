using Godot;
using System;

public partial class Water : Area2D
{
    private AudioStreamPlayer2D splashSound;
    private GameManager gameManager;
    public override void _Ready()
    {
        splashSound = GetNode<AudioStreamPlayer2D>("SplashSound");
        gameManager = GetNode<GameManager>("/root/GameManager");
    }
    public void OnBodyEntered(Node2D body)
    {
        if(body.IsInGroup(gameManager.GROUP_ANIMAL))
        {
            if(body is Animal animal) animal.Die();
            splashSound.Play(); 
        }
    }
}
