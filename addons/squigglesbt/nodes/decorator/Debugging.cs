using System.Collections.Generic;
using Godot;

public class Debugging : Decorator
{
    public override int Tick(Node actor, Blackboard blackboard)
    {
        if (Children.Count <= 0) return FAILURE;
        var child = Children[0];
        var value = child.Tick(actor, blackboard);
        child.LoadDebuggingValues(blackboard);
        blackboard.SetLocal($"debug.{child.Label}:status", value switch
        {
            0 => "Success",
            1 => "Failure",
            2 => "Running",
            _ => "Error?"
        });
        return value;
    }
}