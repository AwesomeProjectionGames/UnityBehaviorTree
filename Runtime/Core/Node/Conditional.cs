namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// A simple leaf node that check a condition
    /// </summary>
    public abstract class Conditional<T> : BaseNodeBehavior<T> where T : Blackboard
    {
        protected override FrameResult OnUpdate()
        {
            return CheckCondition() ? FrameResult.Success : FrameResult.Failure;
        }
        
        /// <summary>
        /// Check the condition of the node
        /// </summary>
        /// <returns>The result of the condition</returns>
        protected abstract bool CheckCondition();
    }
    
    /// <summary>
    /// A simple leaf node that check a condition
    /// </summary>
    public abstract class Conditional : Conditional<Blackboard>
    {
    }
}