using System;
using UnityBehaviorTree.Runtime.Core.Annotation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;

namespace UnityBehaviorTree.Runtime.Core
{
    /// <summary>
    /// The result of a behavior node at a given frame.
    /// </summary>
    public enum FrameResult
    {
        /// <summary>
        /// The behavior has successfully completed (or return true). No further updates are needed.
        /// </summary>
        Success,
        /// <summary>
        /// The behavior has failed (or return false). No further updates are needed.
        /// </summary>
        Failure,
        /// <summary>
        /// The behavior has not yet completed but is still running fine.
        /// </summary>
        Running
    }
    
    /// <summary>
    /// A behaviour node in a behavior tree that can be updated.
    /// Return the current state of the behaviour. Only running behaviours will continue to be updated.
    /// </summary>
    [Serializable]
    public abstract class BaseNodeBehavior
    {
#if UNITY_EDITOR
        [HideInEditorWindow]
        public Rect graphPosition = new Rect();
        
        [HideInEditorWindow]
        [NonSerialized]
        public Action<FrameResult> NotifyEditor;
        
        public static bool EnableLogging = false;
#endif
        /// <summary>
        /// The blackboard used to store and transmit data between behavior nodes.
        /// </summary>
        protected Blackboard Blackboard;

        private bool _didReturn = false;

        /// <summary>
        /// Call when the BehaviorTreeRunner is awaking.
        /// </summary>
        public virtual void Awake(Blackboard blackboard)
        {
#if UNITY_EDITOR
            Log("Calling Awake");
#endif
            Blackboard = blackboard;
            OnAwake();
        }
      
        /// <summary>
        /// Start the behavior node. Call this before the first update.
        /// </summary>
        public virtual void Run()
        {
#if UNITY_EDITOR
            Log("Calling Run");
#endif
            _didReturn = false;
            OnRun();
        }
        
        /// <summary>
        /// Abort running node (ex. when the condition changed)
        /// </summary>
        public virtual void Abort()
        {
#if UNITY_EDITOR
            Log("Calling Abort");
#endif
            OnAbort();
            
        }
        
        /// <summary>
        /// Call the update method on the behavior node (and all its children). Call OnUpdate to get the current state of the behavior.
        /// </summary>
        /// <returns>Return the current state of the leaf behavior node.</returns>
        public virtual FrameResult Update()
        {
#if UNITY_EDITOR
            Log("Calling Update");
#endif
            Assert.IsTrue(!_didReturn, $"The behavior node ({GetType().Name}) has already returned a value. You should not call Update again without calling Run first.");
            var status = OnUpdate();
            if(status != FrameResult.Running)
            {
                _didReturn = true;
            }
#if UNITY_EDITOR
            NotifyEditor?.Invoke(status);
#endif
            return status;
        }
        
        
#if UNITY_EDITOR
        private void Log(string message)
        {
            if(EnableLogging)
            {
                Debug.Log($"[Unity Behavior Tree] {GetType().Name} on {Blackboard?.Runner?.gameObject.name} : {message}");
            }
        }
#endif
        
        /// <summary>
        /// Called when the behaviorTreeRunner is awaking.
        /// </summary>
        protected virtual void OnAwake(){}
        
        /// <summary>
        /// Update the behavior node. Actions nodes will generally return running for multiple frames. Conditional nodes will return success (true) or failure (false).
        /// </summary>
        /// <returns>Returns the current state of the behavior node.</returns>
        protected abstract FrameResult OnUpdate();
        
        /// <summary>
        /// Called when the behavior node is starting. (Called before the first update)
        /// </summary>
        protected virtual void OnRun(){}
        
        /// <summary>
        /// On this node update has been canceled.
        /// </summary>
        protected virtual void OnAbort(){}
    }
}

