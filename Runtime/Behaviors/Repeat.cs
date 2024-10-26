using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    public enum RepeatUntil
    {
        Success,
        Failure,
        Indefinitely
    }
    /// <summary>
    /// Repeat a behaviour until a condition is met
    /// </summary>
    public class Repeat<T> : PassThrough<T> where T : Blackboard
    {
        [SerializeReference] public RepeatUntil Until;

        protected override FrameResult OnUpdate()
        {
            var result = Child.Update();
            if (Until == RepeatUntil.Success && result == FrameResult.Success)
            {
                return FrameResult.Success;
            }
            if (Until == RepeatUntil.Failure && result == FrameResult.Failure)
            {
                return FrameResult.Failure;
            }
            if (result != FrameResult.Running)
            {
                Child.Run();
            }
            return FrameResult.Running;
        }
    }
}