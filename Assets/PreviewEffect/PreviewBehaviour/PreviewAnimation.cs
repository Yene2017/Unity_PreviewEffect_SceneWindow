using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Tools.PreviewEffect
{

    [ExecuteInEditMode]
    [MarkPreview(typeof(Animation))]
    public class PreviewAnimation : PreviewBehaviour<Animation>
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
                clip = behaviour.clip;
                if (!clip)
                {
                    clip = behaviour.clip;
                }
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