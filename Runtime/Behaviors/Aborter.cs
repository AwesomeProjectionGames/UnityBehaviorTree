using JetBrains.Annotations;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// An aborter node that will abort the child node if the condition is not met.
    /// Run the child node until the condition it fails, then abort it.
    /// </summary>
    public class Aborter : PassThrough
    {
        [field: SerializeReference]
        [CanBeNull]
        public BaseNodeBehavior Condition
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }
        
        protected override FrameResult OnUpdate()
        {
            bool isConditionMet = (Condition?.Update() ?? FrameResult.Failure) == FrameResult.Success;
            if (!isConditionMet)
            {
                Child?.Abort();
                return FrameResult.Failure;
            }
            else
            {
                return Child?.Update() ?? FrameResult.Failure;
            }
        }
    }
}