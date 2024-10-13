using System.Reflection;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class BoolResolver : FieldResolver<Toggle,bool>
    {
        public BoolResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override Toggle CreateEditorField(FieldInfo fieldInfo)
        {
            return new Toggle(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(bool);
    }
}