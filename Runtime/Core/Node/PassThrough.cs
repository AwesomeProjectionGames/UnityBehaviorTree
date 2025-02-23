using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// A base for all node that has a single child
    /// </summary>
    public abstract class PassThrough : BaseNodeBehaviorWithImposedRun
    {
        [SerializeReference]
        [FormerlySerializedAs("<Child>k__BackingField")]
        private BaseNodeBehavior child;

        public BaseNodeBehavior Child
        {
            get => child;
#if UNITY_EDITOR
            set => child = value;
#endif
        }


        public override void Awake(Blackboard blackboard)
        {
            base.Awake(blackboard);
            Child?.Awake(blackboard);
        }

        public override void Run()
        {
            base.Run();
            Child?.Run();
        }

        public override void Abort()
        {
            base.Abort();
            Child?.Abort();
        }
    }
}