#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[CustomEditor(typeof(AnimationEventStateBehaviour))]
public class AnimationEventStateBehaviourEditor : Editor
{
    Motion previewClip;
    float previewTime;
    bool isPreviewing;

    PlayableGraph playableGraph;
    AnimationMixerPlayable mixer;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var stateBehaviour = (AnimationEventStateBehaviour)target;

        if (Validate(stateBehaviour, out string errorMessage))
        {
            GUILayout.Space(10);

            if (isPreviewing)
            {
                if (GUILayout.Button("Stop Preview"))
                {
                    ResetTransformPose();
                    isPreviewing = false;
                    AnimationMode.StopAnimationMode();
                    playableGraph.Destroy();
                }
                else
                {
                    PreviewAnimationClip(stateBehaviour);
                }

                GUILayout.Label($"Previewing at {previewTime:F2}s", EditorStyles.helpBox);
            }
            else if (GUILayout.Button("Preview"))
            {
                isPreviewing = true;
                AnimationMode.StartAnimationMode();
            }
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    void PreviewAnimationClip(AnimationEventStateBehaviour stateBehaviour)
    {
        var controller = GetValidAnimatorController(out _);
        if (controller == null) return;

        var matchingState = controller.layers
            .Select(layer => FindMatchingState(layer.stateMachine, stateBehaviour))
            .FirstOrDefault(state => state.state != null);

        if (matchingState.state == null) return;

        var motion = matchingState.state.motion;

        if (motion is BlendTree blendTree)
        {
            SampleBlendTreeAnimation(stateBehaviour, stateBehaviour.triggerTime);
            return;
        }

        if (motion is AnimationClip clip)
        {
            previewTime = stateBehaviour.triggerTime * clip.length;
            AnimationMode.SampleAnimationClip(Selection.activeGameObject, clip, previewTime);
        }
    }

    void SampleBlendTreeAnimation(AnimationEventStateBehaviour stateBehaviour, float normalizedTime)
    {
        var animator = Selection.activeGameObject.GetComponent<Animator>();
        if (playableGraph.IsValid()) playableGraph.Destroy();

        playableGraph = PlayableGraph.Create("BlendTreePreviewGraph");
        mixer = AnimationMixerPlayable.Create(playableGraph, 1);
        var output = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
        output.SetSourcePlayable(mixer);

        var controller = GetValidAnimatorController(out _);
        var matchingState = controller.layers
            .Select(layer => FindMatchingState(layer.stateMachine, stateBehaviour))
            .FirstOrDefault(state => state.state != null);

        if (matchingState.state.motion is not BlendTree blendTree) return;

        float maxThreshold = blendTree.children.Max(child => child.threshold);
        float targetWeight = Mathf.Clamp(normalizedTime * maxThreshold, blendTree.minThreshold, maxThreshold);

        var clipPlayables = new AnimationClipPlayable[blendTree.children.Length];
        float[] weights = new float[blendTree.children.Length];
        float totalWeight = 0f;

        for (int i = 0; i < blendTree.children.Length; i++)
        {
            var child = blendTree.children[i];
            var weight = CalculateWeightForChild(blendTree, child, targetWeight);
            weights[i] = weight;
            totalWeight += weight;

            var clip = GetAnimationClipFromMotion(child.motion);
            clipPlayables[i] = AnimationClipPlayable.Create(playableGraph, clip);
        }

        for (int i = 0; i < weights.Length; i++) weights[i] /= totalWeight;

        mixer.SetInputCount(clipPlayables.Length);
        for (int i = 0; i < clipPlayables.Length; i++)
        {
            mixer.ConnectInput(i, clipPlayables[i], 0);
            mixer.SetInputWeight(i, weights[i]);
        }

        AnimationMode.SamplePlayableGraph(playableGraph, 0, normalizedTime);
    }

    float CalculateWeightForChild(BlendTree blendTree, ChildMotion child, float targetWeight)
    {
        if (blendTree.blendType == BlendTreeType.Simple1D)
        {
            ChildMotion? lower = null, upper = null;

            foreach (var motion in blendTree.children)
            {
                if (motion.threshold <= targetWeight && (lower == null || motion.threshold > lower.Value.threshold))
                    lower = motion;
                if (motion.threshold >= targetWeight && (upper == null || motion.threshold < upper.Value.threshold))
                    upper = motion;
            }

            if (lower.HasValue && upper.HasValue)
            {
                if (Mathf.Approximately(child.threshold, lower.Value.threshold))
                    return 1.0f - Mathf.InverseLerp(lower.Value.threshold, upper.Value.threshold, targetWeight);
                else if (Mathf.Approximately(child.threshold, upper.Value.threshold))
                    return Mathf.InverseLerp(lower.Value.threshold, upper.Value.threshold, targetWeight);
            }

            return Mathf.Approximately(targetWeight, child.threshold) ? 1f : 0f;
        }

        if (blendTree.blendType is BlendTreeType.FreeformCartesian2D or BlendTreeType.FreeformDirectional2D)
        {
            var targetPos = new Vector2(
                GetBlendParameterValue(blendTree, blendTree.blendParameter),
                GetBlendParameterValue(blendTree, blendTree.blendParameterY));
            float dist = Vector2.Distance(targetPos, child.position);
            return Mathf.Clamp01(1.0f / (dist + 0.001f));
        }

        return 0f;
    }

    float GetBlendParameterValue(BlendTree blendTree, string param)
    {
        var method = typeof(BlendTree).GetMethod("GetInputBlendValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (method == null)
        {
            Debug.LogError("GetInputBlendValue method not found.");
            return 0f;
        }
        return (float)method.Invoke(blendTree, new object[] { param });
    }

    ChildAnimatorState FindMatchingState(AnimatorStateMachine machine, AnimationEventStateBehaviour behaviour)
    {
        foreach (var state in machine.states)
            if (state.state.behaviours.Contains(behaviour)) return state;

        foreach (var subMachine in machine.stateMachines)
        {
            var result = FindMatchingState(subMachine.stateMachine, behaviour);
            if (result.state != null) return result;
        }

        return default;
    }

    bool Validate(AnimationEventStateBehaviour behaviour, out string errorMessage)
    {
        var controller = GetValidAnimatorController(out errorMessage);
        if (controller == null) return false;

        var matchingState = controller.layers
            .Select(layer => FindMatchingState(layer.stateMachine, behaviour))
            .FirstOrDefault(state => state.state != null);

        previewClip = GetAnimationClipFromMotion(matchingState.state?.motion);
        if (previewClip == null)
        {
            errorMessage = "No valid AnimationClip found.";
            return false;
        }

        return true;
    }

    AnimationClip GetAnimationClipFromMotion(Motion motion)
    {
        return motion switch
        {
            AnimationClip clip => clip,
            BlendTree tree => tree.children
                .Select(child => GetAnimationClipFromMotion(child.motion))
                .FirstOrDefault(c => c != null),
            _ => null
        };
    }

    AnimatorController GetValidAnimatorController(out string errorMessage)
    {
        errorMessage = "";
        var go = Selection.activeGameObject;
        if (!go)
        {
            errorMessage = "Select a GameObject with an Animator.";
            return null;
        }

        var animator = go.GetComponent<Animator>();
        if (!animator)
        {
            errorMessage = "GameObject lacks an Animator.";
            return null;
        }

        if (animator.runtimeAnimatorController is not AnimatorController controller)
        {
            errorMessage = "Animator does not use an AnimatorController.";
            return null;
        }

        return controller;
    }
    [Serializable]
    public class TransformPose
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    static string GetTransformPath(Transform root, Transform target)
    {
        if (target == root) return "";
        return AnimationUtility.CalculateTransformPath(target, root);
    }
    static void CacheBindPose(Transform root)
    {
        bindPoseCache.Clear();
        foreach (var t in root.GetComponentsInChildren<Transform>(true))
        {
            string path = GetTransformPath(root, t);
            bindPoseCache[path] = new TransformPose
            {
                localPosition = t.localPosition,
                localRotation = t.localRotation,
                localScale = t.localScale
            };
        }

        Debug.Log("Bind pose cached for generic rig.");
    }
    static Dictionary<string, TransformPose> bindPoseCache = new();

    static void ResetTransformPose()
    {
        var selected = Selection.activeGameObject;
        if (!selected) return;

        var animator = selected.GetComponent<Animator>();
        if (animator == null || animator.avatar == null) return;

        if (animator.avatar.isHuman)
        {
            try
            {
                var handler = new HumanPoseHandler(animator.avatar, animator.transform);
                var pose = new HumanPose();
                handler.GetHumanPose(ref pose);

                pose.bodyPosition = Vector3.zero;
                pose.bodyRotation = Quaternion.identity;
                pose.muscles = new float[HumanTrait.MuscleCount];

                handler.SetHumanPose(ref pose);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to reset humanoid pose: " + ex.Message);
            }
        }
        else
        {
            // Handle Generic rig by resetting to cached bind pose
            if (bindPoseCache.Count == 0)
            {
                CacheBindPose(selected.transform);
            }

            foreach (var pair in bindPoseCache)
            {
                var t = selected.transform.Find(pair.Key);
                if (t != null)
                {
                    t.localPosition = pair.Value.localPosition;
                    t.localRotation = pair.Value.localRotation;
                    t.localScale = pair.Value.localScale;
                }
            }

            Debug.Log("Generic rig pose reset to cached bind pose.");
        }
    }

}
#endif
