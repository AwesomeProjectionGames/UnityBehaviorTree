using System;
using UnityBehaviorTree.Runtime.Behaviors;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class NodeResolver
    {
        public BaseBehaviorTreeNode CreateNodeInstance(Type type)
        {
            BaseBehaviorTreeNode node;
            if (type == typeof(Root<Blackboard>))
            {
                node = new RootNode();
            } else if (type.IsClassOrSubclassOf(typeof(Composite<Blackboard>)))
            {
                node = new CompositeNode();
            } else if (type.IsClassOrSubclassOf(typeof(Aborter<Blackboard>)))
            {
                node = new AborterNode();
            } else if (type.IsClassOrSubclassOf(typeof(PassThrough<Blackboard>)))
            {
                node = new PassThroughNode();
            } else if (type.IsClassOrSubclassOf(typeof(Conditional<Blackboard>)))
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