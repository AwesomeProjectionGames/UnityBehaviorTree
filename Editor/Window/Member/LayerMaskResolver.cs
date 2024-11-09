using System.Reflection;
using UnityBehaviorTree.Runtime.Core;
using UnityEditor.UIElements;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class LayerMaskResolver : FieldResolver<LayerMaskField,int>
    {
        public LayerMaskResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override LayerMaskField CreateEditorField(FieldInfo fieldInfo)
        {
            return new LayerMaskField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(LayerMask);

        public override void Restore(BaseNodeBehavior behavior)
        {
            EditorField.value = (LayerMask)FieldInfo.GetValue(behavior);
        }

        public override void Commit(BaseNodeBehavior behavior)
        {
            FieldInfo.SetValue(behavior, (LayerMask)EditorField.value);
        }
    }
}