using System.Collections.Generic;
using Godot;

public class Limiter : Decorator
{
    private int counter = int.MaxValue;

    public override int Tick(Node actor, Blackboard bb)
    {
        if (Children.Count <= 0 || !Params.ContainsKey("count")) return FAILURE;
        int p_counter = Params["counter"].AsInt32();
        if (counter > p_counter) counter = p_counter;

        if (counter <= 0) return FAILURE;

        var result = Children[0].Tick(actor, bb);
        if (result != RUNNING) counter--;
        return result;
    }

    public override void LoadDebuggingValues(Blackboard bb)
    {
        bb.SetLocal($"debug.{Label}:counter", counter);
    }
}