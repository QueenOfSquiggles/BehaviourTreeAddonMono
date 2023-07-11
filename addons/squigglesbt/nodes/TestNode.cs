using System.Collections.Generic;
using Godot;

public class TestNode : BTNode
{
    public TestNode() : base("TestNode", -1) { }

    public override int Tick(Node actor, Blackboard blackboard)
    {
        foreach (var c in Children) c.Tick(actor, blackboard);
        return SUCCESS;
    }

}