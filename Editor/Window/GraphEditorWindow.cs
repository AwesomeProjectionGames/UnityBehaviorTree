using System;
using System.Collections.Generic;
using UnityBehaviorTree.Runtime;
using UnityBehaviorTree.Runtime.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window
{
    public class GraphEditorWindow : EditorWindow
    {
        // GraphView window per GameObject
        private static readonly Dictionary<GameObject,GraphEditorWindow> Cache = new Dictionary<GameObject, GraphEditorWindow>();

        private GameObject Key { get; set; }

        public static void Show(BehaviorTreeRunner bt)
        {
            var window = Create(bt);
            window.Show();
            window.Focus();
        }

        private static GraphEditorWindow Create(BehaviorTreeRunner bt)
        {
            var key = bt.gameObject;
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            var window = CreateInstance<GraphEditorWindow>();
            StructGraphView(window, bt);
            window.titleContent = new GUIContent($"BehaviorTree Editor of {bt.gameObject.name}");
            window.Key = key;
            Cache[key] = window;
            return window;
        }

        private static void StructGraphView(GraphEditorWindow window, BehaviorTreeRunner behaviorTreeRunner)
        {
            window.rootVisualElement.Clear();
            var graphView = new BehaviorTreeView(behaviorTreeRunner, window);
            graphView.Restore();
            window.rootVisualElement.Add(window.CreateToolBar(graphView));
            window.rootVisualElement.Add(graphView);
        }

        private void OnDestroy()
        {
            if (Key != null && Cache.ContainsKey(Key))
            {
                Cache.Remove(Key);
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    Reload();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Reload();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playModeStateChange), playModeStateChange, null);
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Reload();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void Reload()
        {
            if (Key != null)
            {
                StructGraphView(this, Key.GetComponent<BehaviorTreeRunner>());
                Repaint();
            }
        }

        private VisualElement CreateToolBar(BehaviorTreeView graphView)
        {
            return new IMGUIContainer(
                () =>
                {
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);

                    if (!Application.isPlaying)
                    {
                        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                        {
                            var guiContent = new GUIContent();
                            if (graphView.Save())
                            {
                                guiContent.text = "Successfully updated.";
                                this.ShowNotification(guiContent);
                            }
                            else
                            {
                                guiContent.text = "Invalid tree. one or mode nodes have error.";
                                this.ShowNotification(guiContent);
                            }
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            );

        }


    }
}