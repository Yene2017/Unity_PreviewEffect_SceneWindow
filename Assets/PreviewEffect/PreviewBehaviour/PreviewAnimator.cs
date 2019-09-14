using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Tools.PreviewEffect
{
    [ExecuteInEditMode]
    [MarkPreview(typeof(Animator))]
    public class PreviewAnimator : PreviewBehaviour<Animator>
    {
        public AnimationClip clip;

        protected override void UpdatePreview()
        {
            if (isPlaying)
            {
                current = EditorApplication.timeSinceStartup - startTime;
                if (clip)
                {
                    clip.SampleAnimation(gameObject, (float)current);
                }
            }
        }

        protected override void OnPlay()
        {
            if (behaviour)
            {
                clip = behaviour.GetCurrentAnimatorClipInfo(0)[0].clip;
            }
            if (clip)
            {
                clip.SampleAnimation(gameObject, 0);
            }
            else
            {
                GameObject.DestroyImmediate(this);
            }
        }

        protected override void OnStop()
        {
            if (clip)
            {
                clip.SampleAnimation(gameObject, clip.length);
            }
            clip = null;
        }

    }

}