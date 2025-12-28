using Godot;
using System;

public partial class LevelButton : Control
{
    private float HOVER_SCALE = 1.2f;
    private float DEFAULT_SCALE = 1.0f;
    [Export] private int LEVEL = 1;

    private Label levelNode;

    public override void _Ready()
    {
        levelNode = GetNode<Label>("TextureButton/MC/VB/LevelName");
        levelNode.Text = LEVEL.ToString();
    }

    public void OnPressed()
    {
        
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
