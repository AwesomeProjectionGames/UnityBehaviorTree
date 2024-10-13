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
        [field:SerializeField] public UpdateType UpdateType { get; set; }
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
            root.Awake(Blackboard);
            root.Run();
        }

        /// <summary>
        /// Call this method to update the behavior tree manually.
        /// </summary>
        public void Update()
        {
            if (UpdateType == UpdateType.Auto) root.Update();
        }
        
        protected virtual Blackboard CreateBlackboard()
        {
            return new Blackboard(this);
        }
    }
}