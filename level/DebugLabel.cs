using Godot;
using System;

public partial class DebugLabel : Label
{
    public override void _Ready()
    {
        var signalManager = GetNode<SignalManager>("/root/SignalManager");
        signalManager.UpdateDebugLabel += text => Text = text + "\n";
    }
    

}
