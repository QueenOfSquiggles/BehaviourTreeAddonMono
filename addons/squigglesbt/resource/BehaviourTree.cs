using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BehaviourTree : Resource
{
    [Export] public string Name;

    [Export] public Dictionary JSONData;
    public Root TreeRoot = new();

    public void RebuildTree()
    {
        TreeRoot = (Root)LoadNodesRecursive(TreeRoot, JSONData);
        TreeRoot.Label = Name;
    }

    private BTNode LoadNodesRecursive(BTNode target, Dictionary data)
    {
        if (!data.ContainsKey("children")) return target;

        var children = data["children"].AsGodotArray<Dictionary>();
        foreach (var dict in children)
        {
            if (target.MaxChildren >= 0 && target.Children.Count >= target.MaxChildren)
            {
                GD.PushWarning($"Child overflow for {target.Label}: skipping -> {dict}");
            }
            var n_node = CreateNodeFrom(dict);
            if (n_node is null) continue;

            LoadNodesRecursive(n_node, dict);
            target.Children.Add(n_node);
        }
        return target;
    }
    private static BTNode CreateNodeFrom(Dictionary dict)
    {

        if (!dict.ContainsKey("type")) { GD.Print($"Invalid node dictionary: {dict}"); return null; }

        string type = dict["type"].AsString();
        string label = dict.GetValueOrDefault("label", type).AsString();
        var cs_type = Type.GetType(type);
        if (cs_type is null) { GD.Print($"Failed to load behaviour tree node of type: {type}"); return null; }
        var node = Activator.CreateInstance(cs_type) as BTNode;
        if (node is not null)
        {
            node.Label = label;
            foreach (var entry in dict)
            {
                var key = entry.Key.AsString();
                if (key == "type" || key == "label" || key == "children") continue;
                node.Params.Add(key, entry.Value);
            }
        }
        return node;
    }
    public void PrintTree() => PrintTreeRecursive(TreeRoot, 0);

    private void PrintTreeRecursive(BTNode node, int level)
    {
        GD.Print($"{StrPat(" | ", level)}{node.Label} ({node.GetType().FullName})");
        foreach (var c in node.Children)
            PrintTreeRecursive(c, level + 1);
    }

    private string StrPat(string pattern, int count)
    {
        if (count <= 0) return "";
        var result = pattern;
        for (int i = 1; i < count; i++) result += pattern;
        return result;
    }
}
