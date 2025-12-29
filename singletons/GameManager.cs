using Godot;
using System;

public partial class GameManager : Node
{
    public string GROUP_BLOCK = "block";
    public string GROUP_ANIMAL = "animal";


    private PackedScene mainScene;
    public override void _Ready()
    {
        mainScene = GD.Load<PackedScene>("res://main/main.tscn");
    }

    public void LoadMain()
    {
        GetTree().ChangeSceneToPacked(mainScene);
    }
}
