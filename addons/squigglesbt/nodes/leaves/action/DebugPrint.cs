using System.Collections.Generic;
using Godot;

public class DebugPrint : Leaf
{
    public override int Tick(Node actor, Blackboard blackboard)
    {
        if (!Params.ContainsKey("message")) return FAILURE;
        GD.Print(Params["message"].AsString());
        return SUCCESS;
    }
}