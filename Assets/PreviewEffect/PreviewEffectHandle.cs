using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.PreviewEffect
{
    public class PreviewEffectHandle
    {
        static Dictionary<Type, Type> markPreviewMap = new Dictionary<Type, Type>();

        static PreviewEffectHandle()
        {
            foreach (var ab in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in ab.GetTypes())
                {
                    var objs = t.GetCustomAttributes(typeof(MarkPreviewAttribute), false);
                    if (objs.Length == 0) continue;
                    markPreviewMap.Add((objs[0] as MarkPreviewAttribute).type, t);
                }
            }
        }

        public GameObject selectEffect;
        public bool isPlaying;
        public bool isPaused;
        public double stepInterval = 0.1f;

        public void ChangeHandleEffect(GameObject newEf)
        {
            if (!newEf)
            {
                return;
            }
            if (selectEffect)
            {
                foreach (var m in selectEffect.GetComponentsInChildren<PreviewBehaviour>())
                {
                    GameObject.DestroyImmediate(m);
                }
            }
            selectEffect = newEf;
            foreach (var mark in markPreviewMap)
            {
                foreach (var m in selectEffect.GetComponentsInChildren(mark.Key))
                {
                    if (!m.GetComponent(mark.Value))
                        m.gameObject.AddComponent(mark.Value);
                }
            }
        }

        public void Play()
        {
            if (!selectEffect)
            {
                return;
            }
            isPlaying = true;
            isPaused = false;
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                p.Play();
            }
        }

        public void Pause()
        {
            if (!selectEffect)
            {
                return;
            }
            isPlaying = false;
            isPaused = true;
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                p.Pause();
            }
        }

        public void Stop()
        {
            if (!selectEffect)
            {
                return;
            }
            isPlaying = false;
            isPaused = false;
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                p.Stop();
            }
        }

        public void Clear()
        {
            if (!selectEffect)
            {
                return;
            }
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                GameObject.DestroyImmediate(p);
            }
        }

        public static implicit operator bool(PreviewEffectHandle handle)
        {
            return handle != null;
        }

        public void Step()
        {
            if (!selectEffect)
            {
                return;
            }
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                p.Step(stepInterval);
            }
        }

        public void Strides()
        {
            if (!selectEffect)
            {
                return;
            }
            var ef = selectEffect;
            var ps = ef.GetComponentsInChildren<PreviewBehaviour>();
            foreach (var p in ps)
            {
                p.Step(stepInterval);
                p.Step(stepInterval);
                p.Step(stepInterval);
            }
        }
    }

}
