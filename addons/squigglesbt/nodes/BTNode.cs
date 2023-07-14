using System.Collections.Generic;
using Godot;

public abstract class BTNode
{
    public string Label = "";
    public int MaxChildren { get; protected set; }
    public List<BTNode> Children { get; protected set; } = new();
    public Dictionary<string, Variant> Params = new();

    public BTNode() : this("BTNode", -1) { }
    public BTNode(string label, int max_children)
    {
        Label = label;
        MaxChildren = max_children;
        RegisterParams();
    }

    public const int SUCCESS = 0;
    public const int FAILURE = 1;
    public const int RUNNING = 2;
    public const int ERROR = 3;

    protected abstract void RegisterParams();

    public abstract int Tick(Node actor, Blackboard blackboard);

    public virtual void LoadDebuggingValues(Blackboard bb) { }

    public Dictionary<string, Variant> GetKnownParams() => Params;

    protected Variant GetParam(string key, Variant fallback)
    {
        if (Params.ContainsKey(key)) return Params[key];
        return fallback;

    }


}