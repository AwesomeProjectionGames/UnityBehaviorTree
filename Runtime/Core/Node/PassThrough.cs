﻿using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// A base for all node that has a single child
    /// </summary>
    public abstract class PassThrough<T> : BaseNodeBehavior<T> where T : Blackboard
    {
        [field: SerializeReference]
        [CanBeNull]
        public BaseNodeBehavior<Blackboard> Child
        {
            get;
#if UNITY_EDITOR
            set;
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
    
    /// <summary>
    /// A base for all node that has a single child
    /// </summary>
    public abstract class PassThrough : PassThrough<Blackboard>
    {
    }
}