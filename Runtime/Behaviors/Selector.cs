﻿using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one succeeds
    /// </summary>
    public class Selector<T> : Composite<T> where T : Blackboard
    {
        public bool RandomizeOrder = false;
        
        int _currentBehaviour = 0;
        
        protected override void OnRun()
        {
            _currentBehaviour = 0;
            Assert.IsTrue(Children.Count > 0);
            if (RandomizeOrder) Children = Children.OrderBy(x => Random.value).ToList();
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
    
    /// <summary>
    /// Execute a list of behaviours in order until one succeeds
    /// </summary>
    public class Selector : Selector<Blackboard>
    {
    }
}