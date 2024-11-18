using UnityEngine.Assertions;

namespace UnityBehaviorTree.Runtime.Core
{
    /// <summary>
    /// A behavior node that requires Run to be called before Update
    /// </summary>
    public abstract class BaseNodeBehaviorWithImposedRun : BaseNodeBehavior
    {
        private bool _didReturn = false;

        public override void Run()
        {
            base.Run();
            _didReturn = false;
        }

        public override FrameResult Update()
        {
            var status = base.Update();
            Assert.IsTrue(!_didReturn, $"The behavior node ({GetType().Name}) has already returned a value. You should not call Update again without calling Run first.");
            if(status != FrameResult.Running)
            {
                _didReturn = true;
            }
            return status;
        }
    }
}