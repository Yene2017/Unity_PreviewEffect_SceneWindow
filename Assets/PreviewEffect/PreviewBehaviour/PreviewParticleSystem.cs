using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.PreviewEffect
{
    [ExecuteInEditMode]
    [MarkPreview(typeof(ParticleSystem))]
    public class PreviewParticleSystem : PreviewBehaviour
    {
        public ParticleSystem ps;

        protected override void UpdatePreview()
        {
            if (ps)
            {
                if (isPlaying)
                {
                    var delta = current - lastTime;
                    ps.Simulate((float)delta, false, false);
                }
                else
                {
                    ps.Pause();
                }
            }
        }

        protected override void OnPlay()
        {
            lastTime = EditorApplication.timeSinceStartup;
            if (!ps)
            {
                ps = GetComponent<ParticleSystem>();
            }
            if (!ps)
            {
                GameObject.DestroyImmediate(this);
            }
            ps.Simulate(0, false, true);
        }

        protected override void OnPause()
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        protected override void OnStop()
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

}