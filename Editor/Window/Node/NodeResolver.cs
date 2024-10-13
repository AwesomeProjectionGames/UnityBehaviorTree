using System;
using UnityBehaviorTree.Runtime.Behaviors;
using UnityBehaviorTree.Runtime.Core.Node;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class NodeResolver
    {
        public BaseBehaviorTreeNode CreateNodeInstance(Type type)
        {
            BaseBehaviorTreeNode node;
            if (type == typeof(Root))
            {
                node = new RootNode();
            } else if (type.IsSubclassOf(typeof(Composite)))
            {
                node = new CompositeNode();
            } else if (type.IsSubclassOf(typeof(Aborter)))
            {
                node = new AborterNode();
            } else if (type.IsSubclassOf(typeof(PassThrough)))
            {
                node = new PassThroughNode();
            } else if (type.IsSubclassOf(typeof(Conditional)))
            {
                node = new ConditionalNode();
            } else
            {
                node = new ActionNode();
            }
            node.SetBehavior(type);
            return node;
        }
    }
}