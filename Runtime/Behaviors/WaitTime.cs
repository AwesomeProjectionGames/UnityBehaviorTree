using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Wait for a given amount of time before returning success
    /// </summary>
    public class WaitTime : Action
    {
        public float TimeToWait;
        private float _time;
        
        protected override void OnRun()
        {
            _time = 0;
        }

        protected override FrameResult OnUpdate()
        {
            _time += Time.deltaTime;
            if (_time >= TimeToWait)
            {
                return FrameResult.Success;
            }
            return FrameResult.Running;
        }
    }
}