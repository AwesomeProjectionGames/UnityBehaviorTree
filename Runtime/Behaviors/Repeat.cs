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
    public class Repeat : PassThrough
    {
        [SerializeReference] public RepeatUntil Until;

        protected override FrameResult OnUpdate()
        {
            var result = Child.Update();
            if (Until == RepeatUntil.Success && result == FrameResult.Success)
            {
                Log("Final success (repeat until success)");
                return FrameResult.Success;
            }
            if (Until == RepeatUntil.Failure && result == FrameResult.Failure)
            {
                Log("Final failure (repeat until failure)");
                return FrameResult.Failure;
            }
            if (result != FrameResult.Running)
            {
                Log("Child finished, restarting");
                Child.Run();
            }
            return FrameResult.Running;
        }
    }
}