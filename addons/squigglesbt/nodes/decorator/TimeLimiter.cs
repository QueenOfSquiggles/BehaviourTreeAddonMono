using System.Collections.Generic;
using Godot;

public class TimeLimiter : Decorator
{
    private float counter = int.MaxValue;

    public override int Tick(Node actor, Blackboard bb)
    {
        if (Children.Count <= 0 || !Params.ContainsKey("seconds")) return FAILURE;
        var p_counter = Params["seconds"].AsSingle();
        if (counter > p_counter) counter = p_counter;

        if (counter <= 0) return FAILURE;
        counter -= bb.GetLocal("delta").AsSingle();
        var result = Children[0].Tick(actor, bb);
        if (result != RUNNING) counter = -1;
        return result;
    }

    public override void LoadDebuggingValues(Blackboard bb)
    {
        bb.SetLocal($"debug.{Label}:counter", counter);
    }
}