using System;
using UnityEditor;
using UnityEngine;


namespace Tools.PreviewEffect
{

    public class PreviewBehaviour : MonoBehaviour
    {
        public double startTime;
        public double current;
        public double lastTime;
        public double deltaTime;
        public bool isPlaying;
        public bool isPaused;
        public bool isStoped
        {
            get
            {
                return !isPaused && !isPlaying;
            }
        }

        private void OnEnable()
        {
            Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Update()
        {
            Sample();
        }

        public void Play()
        {
            hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
            startTime = EditorApplication.timeSinceStartup;
            lastTime = EditorApplication.timeSinceStartup;
            isPlaying = true;
            isPaused = false;
            OnPlay();
        }

        public void Pause()
        {
            isPlaying = false;
            isPaused = true;
            OnPause();
        }

        public void Stop()
        {
            isPlaying = false;
            isPaused = false;
            OnStop();
        }

        public void Step(double interval)
        {
            if (EnablePreview())
            {
                var time = EditorApplication.timeSinceStartup;
                startTime += time - current - interval;
                current = time;
                deltaTime = interval;
                lastTime = current - interval;
                UpdatePreview();
                lastTime = time;
            }
        }

        private void Sample()
        {
            var time = EditorApplication.timeSinceStartup;
            if (isPlaying && EnablePreview())
            {
                current = time;
                deltaTime = time - lastTime;
                UpdatePreview();
                lastTime = time;
            }
            else
            {
                startTime = time;
            }
        }

        protected virtual void OnPlay()
        {

        }

        protected virtual void OnPause()
        {

        }

        protected virtual void OnStop()
        {

        }

        protected virtual void UpdatePreview()
        {
        }

        public virtual bool EnablePreview()
        {
            return true;
        }

    }
}