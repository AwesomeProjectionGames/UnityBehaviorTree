using System.Reflection;
using UnityEditor.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class DoubleResolver : FieldResolver<DoubleField,double>
    {
        public DoubleResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override DoubleField CreateEditorField(FieldInfo fieldInfo)
        {
            return new DoubleField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(double);
    }
}