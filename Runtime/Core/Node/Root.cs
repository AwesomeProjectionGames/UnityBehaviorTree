namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// The root node of the behavior tree
    /// </summary>
    public class Root : PassThrough
    {
        bool _hasFinished;
        protected override FrameResult OnUpdate()
        {
            if (_hasFinished) return FrameResult.Success;
            FrameResult result = Child?.Update() ?? FrameResult.Failure;
            if (result == FrameResult.Running) return FrameResult.Running;
            _hasFinished = true;
            return result;
        }
    }
}