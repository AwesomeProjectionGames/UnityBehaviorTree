using System.Collections.Generic;
using UnityBehaviorTree.Editor.Window.Menu;
using UnityBehaviorTree.Editor.Window.Window;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window.Node
{
    public class ActionNode : BaseBehaviorTreeNode
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.MenuItems().Add(new BehaviorTreeDropdownMenuAction("Change Behavior", (a) =>
            {
                var provider = new ActionSearchWindowProvider(this);
                SearchWindow.Open(new SearchWindowContext(evt.localMousePosition), provider);
            }));
        }

        protected override bool OnValidate(Stack<BaseBehaviorTreeNode> stack) => true;

        protected override void OnCommit(Stack<BaseBehaviorTreeNode> stack)
        {
        }

        protected override void OnClearStyle()
        {
        }
    }
}