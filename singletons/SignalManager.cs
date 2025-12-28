using Godot;
using System;

public partial class SignalManager : Node
{
    [Signal] public delegate void UpdateDebugLabelEventHandler(string text);
    [Signal] public delegate void AnimalDiedEventHandler();
}
