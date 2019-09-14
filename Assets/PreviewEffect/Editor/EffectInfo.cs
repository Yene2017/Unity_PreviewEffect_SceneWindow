using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.PreviewEffect
{
    public struct EffectInfo
    {
        public GameObject effect;
        public bool lockEffect;
        public int goCount;
        public int particleSystemCount;
        public int particleCount;
        public int animCount;
        public int curveCount;
        public int keyframeCount;
        public int rendererCount;
        public int materialCount;
        public int textureCount;
        public int equalTextureSize;
        public int meshCount;
        public int vertexCount;


        public void DrawInfo()
        {
            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                Lock();
                this.effect = EditorGUILayout.ObjectField(this.effect,
                    typeof(GameObject), true) as GameObject;
            }
            Label("");
            GUILayout.Button("资源使用", "Label");
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    Label("物体", "物体数量\n");
                    EditorGUILayout.IntField(this.goCount);
                    Label("渲染", "渲染物数量\n\n此处仅为参考，以TA提供的实时数据为主\n");
                    EditorGUILayout.IntField(this.rendererCount);
                }
                using (new GUILayout.HorizontalScope())
                {
                    Label("发射器", "ParticleSystem 数量\n");
                    EditorGUILayout.IntField(this.particleSystemCount);
                    Label("粒子", "估计维持的粒子总数\n");
                    EditorGUILayout.IntField(this.particleCount);
                }
                Label("");
                using (new GUILayout.HorizontalScope())
                {
                    Label("动画", "动作资源数量\n\n曲线数量总和\n\n曲线关键帧数量的平均值\n");
                    EditorGUILayout.IntField(this.animCount);
                    EditorGUILayout.IntField(this.curveCount);
                    EditorGUILayout.IntField(this.keyframeCount);
                }
                using (new GUILayout.HorizontalScope())
                {
                    Label("贴图", "引用的贴图资源数量\n");
                    EditorGUILayout.IntField(this.textureCount);
                    Label("尺寸", "等价尺寸，贴图面积总和等价方形图片的尺寸\n");
                    EditorGUILayout.IntField(this.equalTextureSize);
                }
                using (new GUILayout.HorizontalScope())
                {
                    Label("网格", "引用网格资源数量，不含生成粒子");
                    EditorGUILayout.IntField(this.meshCount);
                    Label("顶点", "顶点总和\n");
                    EditorGUILayout.IntField(this.vertexCount);
                }
                using (new GUILayout.HorizontalScope())
                {
                    Label("材质", "材质球资源数量\n\n此处仅为参考，以TA提供的实时数据为主\n");
                    EditorGUILayout.IntField(this.materialCount);
                }
            }
        }

        void Label(string str, string tips = null)
        {
            GUILayout.Button(new GUIContent(str, tips), "Label", GUILayout.Width(40));
        }

        void Lock()
        {
            var label = lockEffect ? "锁" : "选";
            if (GUILayout.Button(label, GUILayout.Width(30)))
            {
                lockEffect = !lockEffect;
            }
        }

        public void Collect()
        {
            if (!this.effect)
            {
                return;
            }
            this.goCount = effect.GetComponentsInChildren<Transform>().Length;
            var clips = CollectClips(effect);
            this.animCount = clips.Count;
            this.curveCount = clips.Sum(c =>
            {
                return AnimationUtility.GetCurveBindings(c).Length;
            });
            if (clips.Count < 1)
            {
                this.keyframeCount = 0;
            }
            else
            {
#pragma warning disable CS0618 // 类型或成员已过时
                this.keyframeCount = (int)clips.SelectMany(c =>
                {
                    return AnimationUtility.GetAllCurves(c);
                }).Average(c =>
                {
                    return c.curve.keys.Length;
                });
#pragma warning restore CS0618 // 类型或成员已过时

            }
            var mats = CollectMaterials(effect);
            this.rendererCount = effect.GetComponentsInChildren<Renderer>().Length;
            this.materialCount = mats.Count();
            var textureRefs = mats.SelectMany(m =>
            {
                return AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(m));
            }).Select(p =>
            {
                return AssetDatabase.LoadAssetAtPath<Texture>(p);
            }).Distinct().ToList();
            textureRefs.RemoveAll(t => { return t == null; });
            this.textureCount = textureRefs.Count();
            this.equalTextureSize = (int)Mathf.Sqrt(textureRefs.Sum(t =>
            {
                return t.width * t.height;
            }));
            var meshes = CollectMeshes(effect);
            this.meshCount = meshes.Count;
            this.vertexCount = meshes.Sum(m => { return m.vertexCount; });
            var ps = effect.GetComponentsInChildren<ParticleSystem>();
            this.particleSystemCount = ps.Length;
            this.particleCount = (int)ps.Sum(p =>
            {
                return Mathf.Min(p.main.startLifetimeMultiplier * 10, p.main.maxParticles);
            });
        }

        List<AnimationClip> CollectClips(GameObject effect)
        {
            var counter = new List<AnimationClip>();
            counter.AddRange(effect.GetComponentsInChildren<Animation>()
                .Select(m => { return m.clip; }));
            counter.RemoveAll(m => { return m == null; });
            var list = new List<AnimationClip>();
            list.AddRange(counter.Distinct());
            counter.Clear();
            return list;
        }

        List<Material> CollectMaterials(GameObject effect)
        {
            var matCounter = new List<Material>();
            matCounter.AddRange(effect.GetComponentsInChildren<Renderer>()
                .SelectMany(m => { return m.sharedMaterials; }));
            matCounter.RemoveAll(m => { return m == null; });
            var mats = new List<Material>();
            mats.AddRange(matCounter.Distinct());
            matCounter.Clear();
            return mats;
        }

        List<Mesh> CollectMeshes(GameObject effect)
        {
            var meshCounter = new List<Mesh>();
            meshCounter.AddRange(effect.GetComponentsInChildren<MeshFilter>()
                .Select(m => { return m.sharedMesh; }));
            meshCounter.AddRange(effect.GetComponentsInChildren<SkinnedMeshRenderer>()
                .Select(m => { return m.sharedMesh; }));
            meshCounter.AddRange(effect.GetComponentsInChildren<ParticleSystemRenderer>()
                .Select(m => { return m.mesh; }));
            meshCounter.RemoveAll(m => { return m == null; });
            var meshes = new List<Mesh>();
            meshes.AddRange(meshCounter.Distinct());
            meshCounter.Clear();
            return meshes;
        }
    }
}
