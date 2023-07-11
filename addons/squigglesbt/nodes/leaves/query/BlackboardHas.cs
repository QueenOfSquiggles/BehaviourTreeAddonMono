using System.Collections.Generic;
using Godot;

public class BlackboardHas : Leaf
{
    public override int Tick(Node actor, Blackboard blackboard)
    {
        if (!Params.ContainsKey("key")) return FAILURE;
        return blackboard.HasLocal(Params["key"].AsString()) ? SUCCESS : FAILURE;
    }
}