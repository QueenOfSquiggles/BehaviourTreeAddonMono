using System;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class BehaviourTreeNode : Node
{
    public enum BTProcessMode { PROCESS, PHYSICS }
    public enum BTStatus { SUCCESS, FAILURE, RUNNING, ERROR }

    [Export] private Node _Actor;
    [Export] private BehaviourTree _Behaviour;
    [Export] private BTDebugPanel _DebuggingPanel;
    [Export] private BTProcessMode _BTProcessMode = BTProcessMode.PROCESS;
    [Export] private Dictionary<string, Variant> _InitialBlackboard = new();
    /// <summary>
    /// A debugging value for checking the current running status of the tree.
    /// </summary>
    [Export] private BTStatus _CurrentBTStatus = BTStatus.SUCCESS;

    private Blackboard _Blackboard = new();
    public override void _Ready()
    {
        foreach (var entry in _InitialBlackboard)
            _Blackboard.SetLocal(entry.Key, entry.Value);

        SetProcess(_BTProcessMode == BTProcessMode.PROCESS);
        SetPhysicsProcess(_BTProcessMode == BTProcessMode.PHYSICS);

        _Behaviour.RebuildTree();
    }

    public override void _Process(double delta) => DoTick((float)delta);
    public override void _PhysicsProcess(double delta) => DoTick((float)delta);

    private void DoTick(float delta)
    {
        _Blackboard.SetLocal("delta", delta);
#if DEBUG
        var result = _Behaviour.TreeRoot.Tick(_Actor, _Blackboard);
        _CurrentBTStatus = result switch
        {
            0 => BTStatus.SUCCESS,
            1 => BTStatus.FAILURE,
            2 => BTStatus.RUNNING,
            _ => BTStatus.ERROR
        };
        _DebuggingPanel?.UpdateDisplay(_Blackboard);
#else
        _ = _Behaviour.TreeRoot.Tick(_Actor, _Blackboard);

#endif
    }


}