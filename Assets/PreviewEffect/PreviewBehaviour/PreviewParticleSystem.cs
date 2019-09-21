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
                ps.Simulate((float)deltaTime, false, false);
            }
        }

        protected override void OnPlay()
        {
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
            ps.Pause(false);
        }

        protected override void OnStop()
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public override bool EnablePreview()
        {
            return true;
        }
    }

}