using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one succeeds
    /// </summary>
    public class Selector : Composite
    {
        protected int CurrentBehaviour = 0;
        
        protected override void OnRun()
        {
            CurrentBehaviour = 0;
            Assert.IsTrue(Children.Count > 0);
            Children[CurrentBehaviour].Run();
        }

        protected override FrameResult OnUpdate()
        {
            var result = Children[CurrentBehaviour].Update();
            if (result == FrameResult.Failure)
            {
                CurrentBehaviour++;
                if (CurrentBehaviour >= Children.Count)
                {
                    return FrameResult.Failure;
                }
                Children[CurrentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}