using System.Linq;
using UnityEngine;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in random order until one succeeds.
    /// </summary>
    public class RandomizedOrderSelector : Selector
    {
        protected override void OnRun()
        {
            Children = Children.OrderBy(x => Random.value).ToList();
            base.OnRun();
        }
    }
}