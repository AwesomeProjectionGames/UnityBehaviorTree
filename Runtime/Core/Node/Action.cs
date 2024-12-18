﻿namespace UnityBehaviorTree.Runtime.Core.Node
{
    /// <summary>
    /// An action node leaf in the behavior tree
    /// </summary>
    public abstract class Action<T> : BaseNodeBehavior<T> where T : Blackboard
    {
        
    }
    
    /// <summary>
    /// An action node leaf in the behavior tree
    /// </summary>
    public abstract class Action : Action<Blackboard>
    {
    }
}