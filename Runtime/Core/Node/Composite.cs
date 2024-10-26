using System.Collections.Generic;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// A composite node that can have multiple children
    /// </summary>
    public abstract class Composite<T> : BaseNodeBehavior<T> where T : Blackboard
    {
        [field: SerializeReference]
        public List<BaseNodeBehavior<Blackboard>> Children
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        } = new List<BaseNodeBehavior<Blackboard>>();

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
        public void AddChild(BaseNodeBehavior<Blackboard> child)
        {
            Children.Add(child);
        }
#endif
    }
}