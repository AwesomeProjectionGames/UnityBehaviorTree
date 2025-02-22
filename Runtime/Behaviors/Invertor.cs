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
                Log("Success inverted to Failure");
                return FrameResult.Failure;
            }
            if (result == FrameResult.Failure)
            {
                Log("Failure inverted to Success");
                return FrameResult.Success;
            }
            return result;
        }
    }
}