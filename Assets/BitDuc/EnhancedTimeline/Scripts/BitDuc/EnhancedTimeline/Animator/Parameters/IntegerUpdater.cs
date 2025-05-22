namespace BitDuc.EnhancedTimeline.Animator.Parameters
{
    /// @cond EXCLUDE
    internal class IntegerUpdater : ParameterUpdater
    {
        readonly int hash;
        readonly PostEndValue postEndValue;
        
        int previousValue;

        public IntegerUpdater(int hash, PostEndValue postEndValue)
        {
            this.hash = hash;
            this.postEndValue = postEndValue;
        }

        public void Start(UnityEngine.Animator animator) => previousValue = animator.GetInteger(hash);

        public void Update(UnityEngine.Animator animator, AnimatorParameterBehaviour behavior) =>
            animator.SetInteger(hash, behavior.IntegerValue);

        public void End(UnityEngine.Animator animator)
        {
            if (postEndValue == PostEndValue.LeaveAsIs)
                return;

            var finalValue = postEndValue == PostEndValue.Revert ? previousValue : default;
            animator.SetInteger(hash, finalValue);
        }
    }
}