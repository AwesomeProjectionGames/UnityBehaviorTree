using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one fails
    /// </summary>
    public class Sequence : Composite
    {
        public bool RandomizeOrder = false;
        
        int _currentBehaviour = 0;
        
        protected override void OnRun()
        {
            _currentBehaviour = 0;
            Assert.IsTrue(children.Count > 0);
            if (RandomizeOrder) children = children.OrderBy(x => Random.value).ToList();
            Log($"Calling first child ({ children[_currentBehaviour].GetType().Name })");
            children[_currentBehaviour].Run();
        }

        protected override FrameResult OnUpdate()
        {
            var result = children[_currentBehaviour].Update();
            if (result == FrameResult.Success)
            {
                _currentBehaviour++;
                if (_currentBehaviour >= children.Count)
                {
                    Log("All children succeeded");
                    return FrameResult.Success;
                }
                Log($"Child succeeded. Calling next child ({ children[_currentBehaviour].GetType().Name })");
                children[_currentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}