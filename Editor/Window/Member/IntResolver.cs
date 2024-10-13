using System.Reflection;
using UnityEditor.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class IntResolver : FieldResolver<IntegerField,int>
    {
        public IntResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override IntegerField CreateEditorField(FieldInfo fieldInfo)
        {
            return new IntegerField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(int);
    }
}