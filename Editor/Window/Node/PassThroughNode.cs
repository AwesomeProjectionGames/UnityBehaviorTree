using System.Collections.Generic;
using System.Linq;
using UnityBehaviorTree.Editor.Window.Menu;
using UnityBehaviorTree.Editor.Window.Window;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Blackboard = UnityBehaviorTree.Runtime.Core.Blackboard;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class PassThroughNode : BaseBehaviorTreeNode
    {
        private readonly Port _childPort;

        public Port Child => _childPort;

        private BaseBehaviorTreeNode cache;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.MenuItems().Add(new BehaviorTreeDropdownMenuAction("Change Behavior", (a) =>
            {
                var provider = new PassThroughSearchWindowProvider(this);
                SearchWindow.Open(new SearchWindowContext(evt.localMousePosition), provider);
            }));
        }

        public PassThroughNode()
        {
            _childPort = CreateChildPort();
            outputContainer.Add(_childPort);
        }

        protected override bool OnValidate(Stack<BaseBehaviorTreeNode> stack)
        {
            if (!_childPort.connected)
            {
                return false;
            }
            stack.Push(_childPort.connections.First().input.node as BaseBehaviorTreeNode);
            return true;
        }

        protected override void OnCommit(Stack<BaseBehaviorTreeNode> stack)
        {
            if (!_childPort.connected)
            {
                (NodeBehavior as PassThrough<Blackboard>)!.Child = null;
                cache = null;
                return;
            }
            var child = _childPort.connections.First().input.node as BaseBehaviorTreeNode;
            (NodeBehavior as PassThrough<Blackboard>)!.Child = child?.ReplaceBehavior() as BaseNodeBehavior<Blackboard>;
            stack.Push(child);
            cache = child;
        }

        protected override void OnClearStyle()
        {
            cache?.ClearStyle();
        }
    }
}