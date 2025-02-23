using JetBrains.Annotations;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// An aborter node that will abort the child node if the condition is not met.
    /// Abort the child node run when the condition succeed.
    /// </summary>
    public class Aborter : PassThrough
    {
        [SerializeReference]
        [FormerlySerializedAs("<Condition>k__BackingField")]
        private BaseNodeBehavior condition;

        [Tooltip("Abort the child node run when the condition succeed.")]
        [CanBeNull]
        public BaseNodeBehavior Condition
        {
            get => condition;
#if UNITY_EDITOR
            set => condition = value;
#endif
        }
        
        public override void Awake(Blackboard blackboard)
        {
            base.Awake(blackboard);
            Condition?.Awake(blackboard);
        }

        public override void Run()
        {
            base.Run();
            Condition?.Run();
        }

        protected override FrameResult OnUpdate()
        {
            bool isConditionMet = (Condition?.Update() ?? FrameResult.Failure) == FrameResult.Success;
            if (isConditionMet)
            {
                Log("Aborting child node");
                Child?.Abort();
                return FrameResult.Failure;
            }
            return Child?.Update() ?? FrameResult.Failure;
        }
    }
}