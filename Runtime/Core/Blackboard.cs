using System;

namespace UnityBehaviorTree.Runtime.Core
{
    /// <summary>
    /// Inherit this to store additional data for the behavior tree nodes
    /// </summary>
    [Serializable]
    public class Blackboard
    {
        public Blackboard(BehaviorTreeRunner runner)
        {
            Runner = runner;
        }
        
        /// <summary>
        /// The reference to the behavior tree runner executing this node
        /// </summary>
        public BehaviorTreeRunner Runner { get; set; }
    }
}