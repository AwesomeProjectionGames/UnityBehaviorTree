using System.Reflection;
using UnityEditor.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class FloatResolver : FieldResolver<FloatField,float>
    {
        public FloatResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override FloatField CreateEditorField(FieldInfo fieldInfo)
        {
            return new FloatField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(float);
    }
}