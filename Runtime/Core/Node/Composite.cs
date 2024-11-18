using System.Collections.Generic;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// A composite node that can have multiple children
    /// </summary>
    public abstract class Composite : BaseNodeBehaviorWithImposedRun
    {
        [field: SerializeReference]
        public List<BaseNodeBehavior> Children
        {
            get;
            protected set;
        } = new List<BaseNodeBehavior>();

        public override void Awake(Blackboard blackboard)
        {
            base.Awake(blackboard);
            Children.ForEach(node => node.Awake(blackboard));
        }
        
        protected abstract override void OnRun();
        
#if UNITY_EDITOR
        /// <summary>
        /// Add a child to the composite node.
        /// </summary>
        /// <param name="child">A empty node to add as a child.</param>
        public void AddChild(BaseNodeBehavior child)
        {
            Children.Add(child);
        }
#endif
    }
}