using System.Collections.Generic;
using Godot;

public class ActionWaitForSeconds : Leaf
{
    public float counter = -1f;
    public override int Tick(Node actor, Blackboard blackboard)
    {
        if (!Params.ContainsKey("seconds")) return FAILURE;

        counter -= blackboard.GetLocal("delta").AsSingle();

        if (counter <= 0f)
        {
            counter = Params["seconds"].AsSingle();
            return SUCCESS;
        }
        return RUNNING;
    }

    public override void LoadDebuggingValues(Blackboard bb)
    {
        bb.SetLocal($"debug.{Label}:param.seconds", Params["seconds"]);
        bb.SetLocal($"debug.{Label}:counter", counter);
    }
}