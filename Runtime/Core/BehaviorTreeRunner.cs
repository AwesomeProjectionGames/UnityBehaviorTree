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
        
        [Tooltip("If false, all runners are executed in the same frame. If true, the runners are executed in a sequence of frames to avoid lag spikes.")]
        [field:SerializeField] public bool DiviseUpdateForRunners { get; set; }
        public Blackboard Blackboard { get; private set; }
        
        [Space]
        [Tooltip("This is shown for debugging purpose. This can be usefull to apply modifications to a prefab.")]
        [Header("This is shown for debugging purpose. This can be usefull to apply modifications to a prefab.")]
        [SerializeReference] private Root<Blackboard> root = new Root<Blackboard>();

        public Root<Blackboard> Root
        {
            get => root;
#if UNITY_EDITOR
            set => root = value;
#endif
        }
        
        private void Awake() {
            Blackboard = CreateBlackboard();
            root.Awake(Blackboard);
            root.Run();
        }

        private void OnEnable()
        {
            Runners.Add(this);
        }
        
        private void OnDisable()
        {
            Runners.Remove(this);
        }

        private void Update()
        {
            if (UpdateType == UpdateType.Auto) ManualUpdate();
        }

        /// <summary>
        /// Call this method to update the behavior tree manually.
        /// </summary>
        public void ManualUpdate()
        {
            if (!DiviseUpdateForRunners || IsDiviseUpdateTurn(this)) root.Update();
        }

        protected virtual Blackboard CreateBlackboard()
        {
            return new Blackboard(this);
        }
    }
}