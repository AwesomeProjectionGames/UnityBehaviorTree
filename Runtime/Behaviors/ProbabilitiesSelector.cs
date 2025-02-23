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

        protected override void OnRun()
        {
            CurrentBehaviour = 0;
            Assert.IsTrue(children.Count > 0);
            //Change the order of the children based on the probabilities
            if (ResetProbabilitiesOnRun)  ResetProbabilities();
            // Reorder the children list based on the probabilities
            children = children
                .Select((child, index) => new { child, probability = Probabilities[index] }) // Pair child with its probability
                .OrderBy(x => Random.Range(0f, 1f) * (1 - x.probability)) // Shuffle based on weighted probability
                .Select(x => x.child) // Extract the children back
                .ToList();
            Log($"Reordered children. Calling first child ({ children[CurrentBehaviour].GetType().Name }");
            children[CurrentBehaviour].Run();
        }
        
        /// <summary>
        /// Reset the probabilities to a new random order.
        /// </summary>
        protected void ResetProbabilities()
        {
            Log("Resetting probabilities");
            Probabilities.Clear();
            for (int i = 0; i < children.Count; i++)
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
            var result = children[CurrentBehaviour].Update();
            if (result == FrameResult.Failure)
            {
                CurrentBehaviour++;
                if (CurrentBehaviour >= children.Count)
                {
                    Log("All children failed");
                    return FrameResult.Failure;
                }
                Log($"Child failed. Calling next child ({ children[CurrentBehaviour].GetType().Name })");
                children[CurrentBehaviour].Run();
                return FrameResult.Running;
            }
            return result;
        }
    }
}