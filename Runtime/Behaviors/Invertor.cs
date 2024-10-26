using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Invert the result of a child behaviour
    /// </summary>
    public class Invertor<T> : PassThrough<T> where T : Blackboard
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
    
    /// <summary>
    /// Invert the result of a child behaviour
    /// </summary>
    public class Invertor : Invertor<Blackboard>
    {
    }
}