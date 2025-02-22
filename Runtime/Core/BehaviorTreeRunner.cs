using System;
using System.Collections.Generic;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Core
{
    public enum UpdateType
    {
        Auto,
        Manual
    }
    public class BehaviorTreeRunner : MonoBehaviour
    {
        /// <summary>
        /// The list of all runners in the scene. This is used to update all the runners in the scene.
        /// </summary>
        public static List<BehaviorTreeRunner> Runners { get; } = new List<BehaviorTreeRunner>();
        private static bool IsDiviseUpdateTurn(BehaviorTreeRunner runner)
        {
            int currentIndex = CurrentRunnerIndex % Runners.Count;
            BehaviorTreeRunner runnerToExecute = Runners[currentIndex];
            if (runnerToExecute == runner)
            {
                CurrentRunnerIndex++;
                return true;
            }
            return false;
        }
        private static int CurrentRunnerIndex { get; set; } = 0;
        
        [field:SerializeField] public UpdateType UpdateType { get; set; }
        
        [Tooltip("Enable detailed debug logging for this behavior tree")]
        [field:SerializeField] public bool EnableDebugLogging { get; set; }
        
        [Tooltip("If false, all runners are executed in the same frame. If true, the runners are executed in a sequence of frames to avoid lag spikes.")]
        [field:SerializeField] public bool DiviseUpdateForRunners { get; set; }
        public Blackboard Blackboard { get; private set; }
        
        [Space]
        [Tooltip("This is shown for debugging purpose. This can be usefull to apply modifications to a prefab.")]
        [Header("This is shown for debugging purpose. This can be usefull to apply modifications to a prefab.")]
        [SerializeReference] private Root root = new Root();

        public Root Root
        {
            get => root;
#if UNITY_EDITOR
            set => root = value;
#endif
        }
        
        private void Awake() {
            Blackboard = CreateBlackboard();
            Log("Initializing behavior tree runner", GetType().Name);
            root.Awake(Blackboard);
            root.Run();
        }

        private void OnEnable()
        {
            Log("Enabling behavior tree runner", GetType().Name);
            Runners.Add(this);
        }
        
        private void OnDisable()
        {
            Log("Disabling behavior tree runner", GetType().Name);
            Runners.Remove(this);
        }

        private void Update()
        {
            if (UpdateType == UpdateType.Auto) {
                Log("Auto-updating behavior tree", GetType().Name);
                ManualUpdate();
            }
        }

        /// <summary>
        /// Call this method to update the behavior tree manually.
        /// </summary>
        public virtual void ManualUpdate()
        {
            if (!DiviseUpdateForRunners || IsDiviseUpdateTurn(this)) {
                Log("Updating root node", GetType().Name);
                root.Update();
            }
        }

        /// <summary>
        /// Try to Log a Unity Behavior Tree message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="componentName">The name of components (generally, GetType().Name)</param>
        public void Log(string message, string componentName)
        {
            if (EnableDebugLogging)
                Debug.Log($"[Unity Behavior Tree] {componentName} on {gameObject.name}: {message}", gameObject);
        }

        protected virtual Blackboard CreateBlackboard()
        {
            Log("Creating blackboard", GetType().Name);
            return new Blackboard(this);
        }
    }
}