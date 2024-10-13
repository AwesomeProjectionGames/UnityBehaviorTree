using UnityBehaviorTree.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window
{
    [CustomEditor(typeof(BehaviorTreeRunner), true)]
    public class BehaviorTreeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI ();

            if (GUILayout.Button("Open Behavior Tree"))
            {
                var bt = target as BehaviorTreeRunner;
                GraphEditorWindow.Show(bt);
            }
        }
    }

}