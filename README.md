# Squiggles Behaviour Tree (Mono)

This is a fairly bare-bones implementation of a behaviour tree, made with Godot 4.1 Mono. Adding new tree nodes is fairly easy, and they are significantly lighter than using Godot `Node` objects. The only downside, is that the trees are modelled in JSON.

# Recommended alternatives

If you don't need something written in C#, I highly recommend checking out [Bitbrain's Beehave](https://github.com/bitbrain/beehave). It is a significantly more robust implementation that uses GDScript. It also has some pretty well made documentation and tutorials that I actually used as reference for making this addon.

# PRs?

I'm open to PRs for this, but I can't imagine this getting that much more popular than Beehave. If you feel like making this better but don't know what to do, a graphical editor would be super cool. Especially if it can edit the JSON file directly.

# How to add new nodes?

Adding a new node is quite simple. You simply need to make a new C# class that extends `BTNode`. But for simplicity, you can extend `Compositor`, `Decorator` or `Leaf` depending on the kind of node you are making. `Compositor` allows infinite child nodes, `Decorator` allows one node, and `Leaf` disallows children.

# How are the trees laid out?

They are JSON files, imported with a custom importer, so you make need to reimport the file with "BehaviourTree" selected in the importer.

Here's an example of a tree:
```JSON
{
	"type": "Root",
	"label": "RootNode",
	"children" : [
        {
            "type":"Sequence",
            "children" : [
                {
                    "type" : "BlackboardHas",
                    "key" : "run_main_loop"
                },
                {
                    "type" : "Debugging",
                    "children": [
                        {
                            "type": "ActionWaitForSeconds",
                            "seconds" : 0.1
                        }
                    ]
                },
                {
                    "type" : "Limiter",
                    "count" : 5,
                    "children" : [
                        {
                            "type": "DebugPrint",
                            "label": "Print five times, once per X seconds",
                            "message": "Doing debug print"
                        }
                    ]
                }
            ]
        },
	]
}
```

Each "node" is a dictionary with at least `type` defined as a string value. That value must be the fully qualified name of the class. So if you made a class at `com.yourgame.bt.MySuperCoolCompositor`, you will need to use that entire string to function properly.

A node with `children` defined as an array of dictionaries, will have those children initialized and added to that node as a tree would normally do.

`label` is useful for debugging such that the node will display as a custom text instead of the type of the node.

Any other values will be added to a `System.Collections.Generic.Dictionary<string, Variant>` stored in all `BTNode`s. This parameter dictionary can be referenced in the `Tick` function of the node to alter the functionality. For example, in `BlackboardHas`, there is a `key` that equals `"run_main_loop"`. The function of the `BlackboardHas` node is to query whether the local blackboard has a particular value.

