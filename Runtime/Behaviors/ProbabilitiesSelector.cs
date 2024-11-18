using System.Collections.Generic;
using System.Linq;
using UnityBehaviorTree.Runtime.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Behaviors
{
    /// <summary>
    /// Execute a list of behaviours in order until one succeeds.
    /// If one succeeds, the selector succeeds. If all fail, the selector fails.
    /// Every child has a probability of being selected.
    /// </summary>
    public class ProbabilitiesSelector : RandomizedOrderSelector
    {
        [Tooltip("If true, the probabilities will be reset on each run.")]
        public bool ResetProbabilitiesOnRun = false;
        
        /// <summary>
        /// The sum of all probabilities should be 1.
        /// </summary>
        protected List<float> Probabilities = new List<float>();

        protected override void OnAwake()
        {
            base.OnAwake();
            ResetProbabilities();
        }

        public override void Run()
        {
            CurrentBehaviour = 0;
            Assert.IsTrue(Children.Count > 0);
            //Change the order of the children based on the probabilities
            if (ResetProbabilitiesOnRun)  ResetProbabilities();
            // Reorder the children list based on the probabilities
            Children = Children
                .Select((child, index) => new { child, probability = Probabilities[index] }) // Pair child with its probability
                .OrderBy(x => Random.Range(0f, 1f) * (1 - x.probability)) // Shuffle based on weighted probability
                .Select(x => x.child) // Extract the children back
                .ToList();
            Children[CurrentBehaviour].Run();
        }
        
        /// <summary>
        /// Reset the probabilities to a new random order.
        /// </summary>
        protected void ResetProbabilities()
        {
            Probabilities.Clear();
            for (int i = 0; i < Children.Count; i++)
            {
                Probabilities.Add(Random.value);
            }
            var sum = Probabilities.Sum();
            for (int i = 0; i < Probabilities.Count; i++)
            {
                Probabilities[i] /= sum;
            }
        }
        
        protected override FrameResult OnUpdate()
        {
            var result = Children[CurrentBehaviour].Update();
            if (result == FrameResult.Failure)
            {
                CurrentBehaviour++;
                if (CurrentBehaviour >= Children.Count)
                {
                    return FrameResult.Failure;
                }
                Children[CurrentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}