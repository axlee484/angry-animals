using Godot;
using System;

public partial class Level : Node2D
{
    private SignalManager signalManager;
    private PackedScene animalScene;
    private Marker2D spawnPositionNode;

    public override void _Ready()
    {
        signalManager = GetNode<SignalManager>("/root/SignalManager");
        signalManager.AnimalDied += SpawnAnimal;


        animalScene = GD.Load<PackedScene>("res://animal/animal.tscn"); 
        spawnPositionNode = GetNode<Marker2D>("SpawnPosition");
        SpawnAnimal();
    }

    private void SpawnAnimal()
    {
        var animalNode = animalScene.Instantiate<Animal>();
        animalNode.Position = spawnPositionNode.Position;
        AddChild(animalNode);
    }
}
