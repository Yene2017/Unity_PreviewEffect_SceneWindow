using UnityEditor;
using UnityEngine;

namespace Tools.PreviewEffect
{
    [InitializeOnLoad]
    class CommandMenu
    {
        const string NAME = "Editor Preview";
        const string KEY = "PreviewEffect";
        const string menuPath = "Effect/";
        static bool toggle;
        static PreviewEffectHandle handle;
        static Rect rect;
        static EffectInfo efInfo;

        static CommandMenu()
        {
            toggle = PlayerPrefs.GetInt(KEY) == 1;
            if (toggle)
            {
                efInfo.lockEffect = true;
                if (handle == null)
                {
                    handle = new PreviewEffectHandle();
                }
                SceneView.onSceneGUIDelegate -= DrawSceneView;
                SceneView.onSceneGUIDelegate += DrawSceneView;
            }
        }

        [MenuItem(menuPath + NAME)]
        static void PreviewEffect()
        {
            toggle = !toggle;
            PlayerPrefs.SetInt(KEY, toggle ? 1 : 0);
            PlayerPrefs.Save();
            if (toggle)
            {
                if (handle == null)
                {
                    handle = new PreviewEffectHandle();
                }
                SceneView.onSceneGUIDelegate -= DrawSceneView;
                SceneView.onSceneGUIDelegate += DrawSceneView;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= DrawSceneView;
            }
        }

        [MenuItem(menuPath + NAME, validate = true)]
        static bool PreviewEffectToggole()
        {
            Menu.SetChecked(menuPath + NAME, toggle);
            return true;
        }

        ~CommandMenu()
        {
            if (handle)
            {
                handle.Clear();
            }
        }

        static void DrawSceneView(SceneView sv)
        {
            if (handle == null)
            {
                return;
            }
            if (rect.size == Vector2.zero)
            {
                rect = sv.position;
                rect.xMax = rect.width - 240;
                rect.yMax = rect.height - 10;
                rect.xMin = rect.xMax - 220;
                rect.yMin = rect.yMax - 240;
            }
            Handles.BeginGUI();
            rect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), rect, DrawWizard, NAME);
            var e = Event.current;
            if ((e.type == EventType.MouseDown
                || e.type == EventType.MouseUp
                || e.type == EventType.Ignore)
                && rect.Contains(e.mousePosition))
            {
                e.Use();
            }
            Handles.EndGUI();
        }

        static void DrawWizard(int id)
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Restart"))
                {
                    if (!efInfo.lockEffect || !handle.selectEffect)
                    {
                        var select = Selection.activeGameObject;
                        handle.ChangeHandleEffect(select);

                        efInfo.effect = select;
                        efInfo.Collect();
                    }
                    handle.Play();
                }
                if (GUILayout.Button("Pause"))
                {
                    handle.Pause();
                }
                if (GUILayout.Button("Stop"))
                {
                    handle.Stop();
                }
            }
            if (handle.selectEffect)
            {
                efInfo.DrawInfo();
                EditorUtility.SetDirty(handle.selectEffect);
                HandleUtility.Repaint();
            }
            GUI.DragWindow();
        }

    }
}
