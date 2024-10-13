using System.Collections.Generic;
using UnityBehaviorTree.Editor.Window.Menu;
using UnityBehaviorTree.Editor.Window.Node;
using UnityBehaviorTree.Editor.Window.Window;
using UnityBehaviorTree.Runtime.Behaviors;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window
{
    public class BehaviorTreeView: GraphView
    {
        private readonly struct EdgePair
        {
            public readonly BaseNodeBehavior NodeBehavior;
            public readonly Port ParentOutputPort;

            public EdgePair(BaseNodeBehavior nodeBehavior, Port parentOutputPort)
            {
                NodeBehavior = nodeBehavior;
                ParentOutputPort = parentOutputPort;
            }
        }

        private readonly BehaviorTreeRunner _behaviorTree;

        private RootNode _root;

        private readonly NodeResolver _nodeResolver = new NodeResolver();

        public BehaviorTreeView(BehaviorTreeRunner bt, EditorWindow editor)
        {
            _behaviorTree = bt;
            style.flexGrow = 1;
            style.flexShrink = 1;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            Insert(0, new GridBackground());

            var contentDragger = new ContentDragger();
            //default activator requires Alt key. Alt key is tired.
            contentDragger.activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.LeftMouse,
            });
            // item dragging is prior to view dragging.
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(contentDragger);

            var provider = ScriptableObject.CreateInstance<NodeSearchWindowProvider>();
            provider.Initialize(this, editor);

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), provider);
            };
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            var remainTargets = evt.menu.MenuItems().FindAll(e =>
            {
                switch (e)
                {
                    case BehaviorTreeDropdownMenuAction _ : return true;
                    case DropdownMenuAction a: return a.name == "Create Node" || a.name == "Delete";
                    default: return false;
                }
            });
            //Remove needless default actions .
            evt.menu.MenuItems().Clear();
            remainTargets.ForEach(evt.menu.MenuItems().Add);
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter) {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }

        public void Restore()
        {
            var stack = new Stack<EdgePair>();
            stack.Push(new EdgePair(_behaviorTree.Root, null));
            while (stack.Count > 0)
            {
                // create node
                var edgePair = stack.Pop();
                if (edgePair.NodeBehavior == null)
                {
                    continue;
                }
                var node = _nodeResolver.CreateNodeInstance(edgePair.NodeBehavior.GetType());
                node.Restore(edgePair.NodeBehavior);
                AddElement(node);
                node.SetPosition( edgePair.NodeBehavior.graphPosition);

                // connect parent
                if (edgePair.ParentOutputPort != null)
                {
                    var edge = ConnectPorts(edgePair.ParentOutputPort, node.Parent);
                    AddElement(edge);
                }

                // seek child
                switch (edgePair.NodeBehavior)
                {
                    case Composite nb:
                    {
                        var compositeNode = node as CompositeNode;
                        var addible = nb.Children.Count - compositeNode.ChildPorts.Count;
                        for (var i = 0; i < addible; i++)
                        {
                            compositeNode.AddChild();
                        }

                        for (var i = 0; i < nb.Children.Count; i++)
                        {
                            stack.Push(new EdgePair(nb.Children[i], compositeNode.ChildPorts[i]));
                        }
                        break;
                    }
                    case Root nb:
                    {
                        _root = node as RootNode;
                        if (nb.Child != null)
                        {
                            stack.Push(new EdgePair(nb.Child, _root?.Child));
                        }
                        break;
                    }
                    case Aborter nb:
                    {
                        var aborterNode = node as AborterNode;
                        stack.Push(new EdgePair(nb.Child, aborterNode?.Child));
                        stack.Push(new EdgePair(nb.Condition, aborterNode?.Condition));
                        break;
                    }
                    case PassThrough nb:
                    {
                        var decoratorNode = node as PassThroughNode;
                        stack.Push(new EdgePair(nb.Child, decoratorNode?.Child));
                        break;
                    }
                }
            }
        }


        public bool Save()
        {
            if (Validate())
            {
                Commit();
                return true;
            }

            return false;
        }

        private bool Validate()
        {
            //validate nodes by DFS.
            var stack = new Stack<BaseBehaviorTreeNode>();
            stack.Push(_root);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (!node.Validate(stack))
                {
                    return false;
                }
            }
            return true;
        }

        private void Commit()
        {
            var stack = new Stack<BaseBehaviorTreeNode>();
            stack.Push(_root);
            
            // save new components
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                node.Commit(stack);
            }
            
            _root.PostCommit(_behaviorTree);
            
            // notify to unity editor that the tree is changed.
            EditorUtility.SetDirty(_behaviorTree);
        }

        private static Edge ConnectPorts(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            return tempEdge;
        }

    }
}