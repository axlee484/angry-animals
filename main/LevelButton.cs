using Godot;
using System;

public partial class LevelButton : Control
{
    private float HOVER_SCALE = 1.2f;
    private float DEFAULT_SCALE = 1.0f;
    [Export] private int LEVEL = 1;

    private Label levelNode;
    private Label bestScoreNode;
    private PackedScene levelScene;
    private GameManager gameManager;
    private ScoreManager scoreManager;

    public override void _Ready()
    {
        scoreManager = GetNode<ScoreManager>("/root/ScoreManager");
        
        levelNode = GetNode<Label>("TextureButton/MC/VB/LevelName");
        bestScoreNode = GetNode<Label>("TextureButton/MC/VB/BestScore");
        levelNode.Text = LEVEL.ToString();
        bestScoreNode.Text = scoreManager.levelBestScore[LEVEL].ToString();
        levelScene = GD.Load<PackedScene>("res://level/levels/level_"+LEVEL+".tscn");
        gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public void OnPressed()
    {
        GetTree().ChangeSceneToPacked(levelScene);
        scoreManager.Level = LEVEL;
    }
    public void OnMouseEntered()
    {
        Scale = new Vector2(HOVER_SCALE, HOVER_SCALE);
    }

    public void OnMouseExited()
    {
        Scale = new Vector2(DEFAULT_SCALE, DEFAULT_SCALE);
    }
}
