using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class RectIntResolver : FieldResolver<RectIntField,RectInt>
    {
        public RectIntResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override RectIntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new RectIntField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(RectInt);
    }
}