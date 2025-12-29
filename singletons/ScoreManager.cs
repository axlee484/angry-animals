using Godot;
using System;
using System.Collections.Generic;

public partial class ScoreManager : Node
{
    [Export ]public int DEFAULT_BEST_SCORE = 1000;

    public int Score { get; set; }
    public int Blocks { get; set; }
    public int Attempts { get; set; }
    public int Level { get; set; }

    public Dictionary<int,  int> levelBestScore = [];
    public override void _Ready()
    {
        levelBestScore.Add(1, DEFAULT_BEST_SCORE);
        levelBestScore.Add(2, DEFAULT_BEST_SCORE);
        levelBestScore.Add(3, DEFAULT_BEST_SCORE);
    }

    public void Reset()
    {
        Score = 0;
        Blocks = 0;
        Attempts = 0;
    }

    private void GetLevelsScore()
    {
        foreach(var level in levelBestScore)
        {
            GD.Print($"Level {level.Key} best score: {level.Value}");
        }
    }

    public void GetScore()
    {
        GD.Print(@$"
        Blocks: {Blocks}
        Attempts: {Attempts}
        Level: {Level}
        ");
        GetLevelsScore();
    }

}
