using System.Collections.Generic;
using System.Linq;
using UnityBehaviorTree.Editor.Window.Menu;
using UnityBehaviorTree.Editor.Window.Window;
using UnityBehaviorTree.Runtime.Behaviors;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class AborterNode : PassThroughNode
    {
        private Port _conditionPort;

        public Port Condition => _conditionPort;

        private BaseBehaviorTreeNode _cache;

        public AborterNode()
        {
            _conditionPort = CreateChildPort("Condition (abort if succeed)");
            _conditionPort.tooltip = "Condition to abort the main child running if it fails";
            outputContainer.Add(_conditionPort);
        }

        protected override bool OnValidate(Stack<BaseBehaviorTreeNode> stack)
        {
            if (!_conditionPort.connected)
            {
                return false;
            }
            stack.Push(_conditionPort.connections.First().input.node as BaseBehaviorTreeNode);
            return base.OnValidate(stack);
        }

        protected override void OnCommit(Stack<BaseBehaviorTreeNode> stack)
        {
            if (!_conditionPort.connected)
            {
                (NodeBehavior as Aborter)!.Condition = null;
                _cache = null;
                return;
            }
            var condition = _conditionPort.connections.First().input.node as BaseBehaviorTreeNode;
            (NodeBehavior as Aborter)!.Condition = condition?.ReplaceBehavior();
            stack.Push(condition);
            _cache = condition;
            base.OnCommit(stack);
        }
    }
}