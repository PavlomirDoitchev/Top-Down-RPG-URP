using Assets.Scripts.Utility.Animation;

using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    [CustomEditor(typeof(AnimationEventStateBehaviour))]
    public class AnimationEventStateBehaviourEditor : Editor
    {
        AnimationClip previewClip;
        float previewTime;


    }
}
