using System;
using UnityEngine.Events;

namespace Assets.Scripts.Utility.Animation
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent OnAnimationEvent;
    }
}
