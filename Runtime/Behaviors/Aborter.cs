using JetBrains.Annotations;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// An aborter node that will abort the child node if the condition is not met.
    /// Abort the child node run when the condition succeed.
    /// </summary>
    public class Aborter : PassThrough
    {
        [field: SerializeReference]
        [Tooltip("Abort the child node run when the condition succeed.")]
        [CanBeNull]
        public BaseNodeBehavior Condition
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }
        
        public override void Awake(Blackboard blackboard)
        {
            base.Awake(blackboard);
            Condition?.Awake(blackboard);
        }
        
        protected override FrameResult OnUpdate()
        {
            bool isConditionMet = (Condition?.Update() ?? FrameResult.Failure) == FrameResult.Success;
            if (isConditionMet)
            {
                Child?.Abort();
                return FrameResult.Failure;
            }
            return Child?.Update() ?? FrameResult.Failure;
        }
    }
}