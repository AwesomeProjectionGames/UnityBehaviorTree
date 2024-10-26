using System.Collections.Generic;
using System.Linq;
using UnityBehaviorTree.Editor.Window.Menu;
using UnityBehaviorTree.Editor.Window.Window;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Blackboard = UnityBehaviorTree.Runtime.Core.Blackboard;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class CompositeNode : BaseBehaviorTreeNode
    {
        public readonly List<Port> ChildPorts = new List<Port>();

        private readonly List<BaseBehaviorTreeNode> _cache = new List<BaseBehaviorTreeNode>();

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.MenuItems().Add(new BehaviorTreeDropdownMenuAction("Change Behavior", (a) =>
            {
                var provider = new CompositeSearchWindowProvider(this);
                SearchWindow.Open(new SearchWindowContext(evt.localMousePosition), provider);
            }));
            evt.menu.MenuItems().Add(new BehaviorTreeDropdownMenuAction("Add Child", (a) => AddChild()));
            evt.menu.MenuItems().Add(new BehaviorTreeDropdownMenuAction("Remove Unnecessary Children", (a) => RemoveUnnecessaryChildren()));
        }

        public CompositeNode() 
        {
            AddChild();
        }

        public void AddChild()
        {
            var child = CreateChildPort();
            ChildPorts.Add(child);
            outputContainer.Add(child);
        }

        private void RemoveUnnecessaryChildren()
        {
            var unnecessary = ChildPorts.Where(p => !p.connected).ToList();
            unnecessary.ForEach(e =>
            {
                ChildPorts.Remove(e);
                outputContainer.Remove(e);
            });
        }

        protected override bool OnValidate(Stack<BaseBehaviorTreeNode> stack)
        {
            if (ChildPorts.Count <= 0) return false;

            foreach (var port in ChildPorts)
            {
                if (!port.connected)
                {
                    SetNodeStatus(Status.Error);
                    return false;
                }
                stack.Push(port.connections.First().input.node as BaseBehaviorTreeNode);
            }
            SetNodeStatus(Status.None);
            return true;
        }

        protected override void OnCommit(Stack<BaseBehaviorTreeNode> stack)
        {
            _cache.Clear();
            foreach (var port in ChildPorts)
            {
                var child = port.connections.First().input.node as BaseBehaviorTreeNode;
                (NodeBehavior as Composite<Blackboard>)!.AddChild(child?.ReplaceBehavior());
                stack.Push(child);
                _cache.Add(child);
            }
        }

        protected override void OnClearStyle()
        {
            foreach (var node in _cache)
            {
                node.ClearStyle();
            }
        }
    }
}