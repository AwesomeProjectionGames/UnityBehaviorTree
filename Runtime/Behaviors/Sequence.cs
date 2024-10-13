using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one fails
    /// </summary>
    public class Sequence : Composite
    {
        int _currentBehaviour = 0;
        
        protected override void OnRun()
        {
            _currentBehaviour = 0;
            Assert.IsTrue(Children.Count > 0);
            Children[_currentBehaviour].Run();
        }

        protected override FrameResult OnUpdate()
        {
            var result = Children[_currentBehaviour].Update();
            if (result == FrameResult.Success)
            {
                _currentBehaviour++;
                if (_currentBehaviour >= Children.Count)
                {
                    return FrameResult.Success;
                }
                Children[_currentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}