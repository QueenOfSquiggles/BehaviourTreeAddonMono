using System.Collections.Generic;
using Godot;

public class BlackboardHas : Leaf
{
    protected override void RegisterParams()
    {
        Params["key"] = "bb_key";
    }

    public override int Tick(Node actor, Blackboard blackboard)
    {
        if (!Params.ContainsKey("key")) return FAILURE;
        return blackboard.HasLocal(Params["key"].AsString()) ? SUCCESS : FAILURE;
    }
}