using Godot;
using System;

public partial class GameUi : CanvasLayer
{
    private ScoreManager scoreManager;
    private SignalManager signalManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreManager = GetNode<ScoreManager>("/root/ScoreManager");
        signalManager = GetNode<SignalManager>("/root/SignalManager");
    }

    
}
