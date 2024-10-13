using System.Collections.Generic;
using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor.Experimental.GraphView;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public sealed class RootNode : BaseBehaviorTreeNode
    {
        public readonly Port Child;

        private BaseBehaviorTreeNode _cache;

        public RootNode() 
        {
            SetBehavior(typeof(Root));
            title = "Root";
            Child = CreateChildPort();
            outputContainer.Add(Child);
        }

        protected override void AddParent()
        {
        }

        protected override void OnRestore()
        {
            //TODO : Why ?
            //(NodeBehavior as Root).UpdateEditor = ClearStyle;
        }

        protected override bool OnValidate(Stack<BaseBehaviorTreeNode> stack)
        {
            if (!Child.connected)
            {
                return false;
            }
            stack.Push(Child.connections.First().input.node as BaseBehaviorTreeNode);
            return true;
        }
        protected override void OnCommit(Stack<BaseBehaviorTreeNode> stack)
        {
            var child = Child.connections.First().input.node as BaseBehaviorTreeNode;
            var newRoot = new Root();
            newRoot.Child = child.ReplaceBehavior();
            //newRoot.UpdateEditor = ClearStyle;
            NodeBehavior = newRoot;
            stack.Push(child);
            _cache = child;
        }

        public void PostCommit(BehaviorTreeRunner tree)
        {
            tree.Root = (NodeBehavior as Root); 
        }

        protected override void OnClearStyle()
        {
            _cache?.ClearStyle();
        }
    }
}