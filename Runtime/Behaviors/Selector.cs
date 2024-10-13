using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one succeeds
    /// </summary>
    public class Selector : Composite
    {
        int _currentBehaviour = 0;
        
        protected override void OnRun()
        {
            _currentBehaviour = 0;
            Children[_currentBehaviour].Run();
        }

        protected override FrameResult OnUpdate()
        {
            var result = Children[_currentBehaviour].Update();
            if (result == FrameResult.Failure)
            {
                _currentBehaviour++;
                if (_currentBehaviour >= Children.Count)
                {
                    return FrameResult.Failure;
                }
                Children[_currentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}