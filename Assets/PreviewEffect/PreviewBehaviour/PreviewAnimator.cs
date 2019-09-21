using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Tools.PreviewEffect
{
    [ExecuteInEditMode]
    [MarkPreview(typeof(Animator))]
    public class PreviewAnimator : PreviewBehaviour
    {
        public Animator target;
        public AnimationClip clip;

        protected override void UpdatePreview()
        {
            if (clip)
            {
                var current = this.current - startTime;
                clip.SampleAnimation(gameObject, (float)current);
            }
        }

        protected override void OnPlay()
        {
            if (!target)
            {
                target = GetComponent<Animator>();
            }
            if (target)
            {
                clip = target.GetCurrentAnimatorClipInfo(0)[0].clip;
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

        public override bool EnablePreview()
        {
            return target && target.enabled;
        }

    }

}