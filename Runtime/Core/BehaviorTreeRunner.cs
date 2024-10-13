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
        [HideInInspector] [SerializeReference] private Root root = new Root();

        [field:SerializeField] public UpdateType UpdateType { get; set; }

        public Root Root
        {
            get => root;
#if UNITY_EDITOR
            set => root = value;
#endif
        }
        
        private void Awake() {
            root.Awake(CreateBlackboard());
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