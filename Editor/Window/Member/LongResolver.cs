using System.Reflection;
using UnityEditor.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class LongResolver : FieldResolver<LongField,long>
    {
        public LongResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override LongField CreateEditorField(FieldInfo fieldInfo)
        {
            return new LongField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(long);
    }
}