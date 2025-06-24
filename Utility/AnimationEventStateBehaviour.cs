using UnityEngine;
using UnityEngine.Events;
namespace Assets.Scripts.Utility.Animation
{
    public class AnimationEventStateBehaviour : StateMachineBehaviour
    {
        public string eventName;
        [Range(0f, 1f)] public float triggerTime;

        bool hasTriggered;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            hasTriggered = false;
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float currentTime = stateInfo.normalizedTime % 1f; //Disregard looped time

            if (!hasTriggered && currentTime >= triggerTime)
            {
                NotifyReceiver(animator);
                hasTriggered = true;
            }
        }

        private void NotifyReceiver(Animator animator)
        {
            AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();
            if (receiver != null)
            {
                receiver.OnAnimationEventTriggered(eventName);
            }
        }
    }
}