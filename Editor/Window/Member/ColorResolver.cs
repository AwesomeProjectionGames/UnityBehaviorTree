using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class ColorResolver : FieldResolver<ColorField,Color>
    {
        public ColorResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override ColorField CreateEditorField(FieldInfo fieldInfo)
        {
            return new ColorField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(Color);
    }
}