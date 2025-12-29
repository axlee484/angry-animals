using Godot;
using System;

public partial class Level : Node2D
{
	private SignalManager signalManager;
	private PackedScene animalScene;
	private Marker2D spawnPositionNode;
	private GameUi gameUi;
	private GameManager gameManager;
	private ScoreManager scoreManager;
	private bool isGameOver;


	private void UpdateUi()
	{
		var levelNode = gameUi.GetNode<Label>("MC/VBScore/VBLevel/Level");
		var attemptsNode = gameUi.GetNode<Label>("MC/VBScore/VBLevel/Attempts");
		var bestScoreNode = gameUi.GetNode<Label>("MC/VBScore/VBBest/BestScore");

		levelNode.Text = "Level "+ scoreManager.Level.ToString();
		attemptsNode.Text = scoreManager.Attempts.ToString();
		bestScoreNode.Text = scoreManager.levelBestScore[scoreManager.Level].ToString();
	}

	private void ShowGameOver()
	{
		var gameOverNode = gameUi.GetNode<VBoxContainer>("MC/VBGameOver");
		gameOverNode.Visible = true;
	}

	private void SetInitialScore()
	{
		scoreManager.Reset();
		var blocks = GetTree().GetNodesInGroup(gameManager.GROUP_BLOCK);
		scoreManager.Blocks = blocks.Count;
		UpdateUi();
	}

	public override void _Ready()
	{
		signalManager = GetNode<SignalManager>("/root/SignalManager");
		signalManager.AnimalDied += SpawnAnimal;
		signalManager.BlockDied += OnBlockDied;
		signalManager.AttempComplete += UpdateUi;
		gameManager = GetNode<GameManager>("/root/GameManager");
		scoreManager = GetNode<ScoreManager>("/root/ScoreManager");
		animalScene = GD.Load<PackedScene>("res://animal/animal.tscn"); 
		spawnPositionNode = GetNode<Marker2D>("SpawnPosition");
		gameUi = GetNode<GameUi>("GameUI");

		isGameOver = false;
		SetInitialScore();
		SpawnAnimal();
	}

	public override void _Process(double delta)
	{
		if(!isGameOver && scoreManager.Blocks <= 0)
		{
			ShowGameOver();
			isGameOver = true;
			return;
		}
		if(isGameOver)
		{
			if(Input.IsActionJustPressed("space"))
			{
				gameManager.LoadMain();
			}
			return;
		}

		if(Input.IsKeyPressed(Key.Escape))
		{
			gameManager.LoadMain();
		}
	}

	private void SpawnAnimal()
	{
		if(scoreManager.Blocks<=0) return;

		var animalNode = animalScene.Instantiate<Animal>();
		animalNode.Position = spawnPositionNode.Position;   
		AddChild(animalNode);
	}

	private void OnBlockDied()
	{
		scoreManager.Blocks --;
		if(scoreManager.Blocks <= 0) {
			scoreManager.Blocks = 0;
			var bestScore = scoreManager.levelBestScore[scoreManager.Level];
			if(scoreManager.Score < bestScore) scoreManager.levelBestScore[scoreManager.Level] = scoreManager.Attempts;
			UpdateUi();
		}
		scoreManager.GetScore();
	}

	public override void _ExitTree()
	{
		signalManager.BlockDied -= OnBlockDied;
		signalManager.AnimalDied -= SpawnAnimal;
		
	}
}
