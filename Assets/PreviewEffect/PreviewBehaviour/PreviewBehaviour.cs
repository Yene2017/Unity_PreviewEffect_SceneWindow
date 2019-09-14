using UnityEditor;
using UnityEngine;


namespace Tools.PreviewEffect
{

    public class PreviewBehaviour : MonoBehaviour
    {
        protected double startTime;
        protected double current;
        public double lastTime;
        protected bool isPlaying;

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
            current = EditorApplication.timeSinceStartup;
            UpdatePreview();
            lastTime = current;
        }

        public void Play()
        {
            hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
            startTime = EditorApplication.timeSinceStartup;
            isPlaying = true;
            OnPlay();
        }

        public void Pause()
        {
            isPlaying = false;
            OnPause();
        }

        public void Stop()
        {
            isPlaying = false;
            OnStop();
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

    }

    public class PreviewBehaviour<T> : PreviewBehaviour where T : Behaviour
    {
        protected T behaviour;

        private void Update()
        {
            if (!behaviour || behaviour.enabled)
            {
                current = EditorApplication.timeSinceStartup;
                UpdatePreview();
                lastTime = current;
            }
            else
            {
                startTime = EditorApplication.timeSinceStartup;
            }
        }

        public new void Play()
        {
            hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
            if (!behaviour)
            {
                behaviour = GetComponent<T>();
            }
            startTime = EditorApplication.timeSinceStartup;
            isPlaying = true;
            OnPlay();
        }
    }

}