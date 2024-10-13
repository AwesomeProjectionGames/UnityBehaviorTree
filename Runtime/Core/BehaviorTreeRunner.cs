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

        [SerializeField] UpdateType updateType;

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

        private void Update()
        {
            if (updateType == UpdateType.Auto) root.Update();
        }
        
        protected virtual Blackboard CreateBlackboard()
        {
            return new Blackboard
            {
                Runner = this
            };
        }
    }
}