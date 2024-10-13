using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Invert the result of a child behaviour
    /// </summary>
    public class Invertor : PassThrough
    {
        protected override FrameResult OnUpdate()
        {
            var result = Child.Update();
            if (result == FrameResult.Success)
            {
                return FrameResult.Failure;
            }
            if (result == FrameResult.Failure)
            {
                return FrameResult.Success;
            }
            return result;
        }
    }
}