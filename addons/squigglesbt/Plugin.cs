#if TOOLS
using System;
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{

    private const string TYPE_BEHAVIOUR_TREE_PATH = "res://addons/squigglesbt/resource/BehaviourTree.cs";
    private const string TYPE_BEHAVIOUR_TREE_NAME = "BehaviourTree";

    private const string TYPE_BT_NODE_PATH = "res://addons/squigglesbt/gdnodes/BehaviourTreeNode.cs";
    private const string TYPE_BT_NODE_NAME = "BehaviourTreeNode";

    private const string TYPE_BT_DEBUGGER_PATH = "res://addons/squigglesbt/gdnodes/BTDebugPanel.cs";
    private const string TYPE_BT_DEBUGGER_NAME = "BehaviourTreeDebugPanel";
    private ImportBT Importer;

    public override void _EnterTree()
    {

        AddType(TYPE_BEHAVIOUR_TREE_NAME, "Resource", TYPE_BEHAVIOUR_TREE_PATH);
        AddType(TYPE_BT_NODE_NAME, "Node", TYPE_BT_NODE_PATH);
        AddType(TYPE_BT_DEBUGGER_NAME, "Node", TYPE_BT_DEBUGGER_PATH);

        Importer = new ImportBT();
        AddImportPlugin(Importer);
    }

    private void AddType(string name, string parent, string path) =>
        AddCustomType(name, parent, GD.Load<Script>(path), null);

    public override void _ExitTree()
    {
        RemoveImportPlugin(Importer);
        Importer = null;
        RemoveCustomType(TYPE_BEHAVIOUR_TREE_NAME);
        RemoveCustomType(TYPE_BT_NODE_NAME);
        RemoveCustomType(TYPE_BT_DEBUGGER_NAME);
    }

    private Variant GetSettingOrCreate(string prop, Variant default_value)
    {
        if (ProjectSettings.HasSetting(prop)) return ProjectSettings.GetSetting(prop, default_value);
        ProjectSettings.SetSetting(prop, default_value);
        return default_value;
    }
}
#endif
