using System;
using System.Linq;
using System.Reflection;
using UnityBehaviorTree.Editor.Window.Member.Field;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class EnumResolver : FieldResolver<EnumField,Enum>
    {

        public EnumResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override EnumField CreateEditorField(FieldInfo fieldInfo)
        {
            var enumValue = Enum.GetValues(fieldInfo.FieldType).Cast<Enum>().Select(v => v).ToList();
            return new EnumField(fieldInfo.Name, enumValue, enumValue[0]);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType.IsEnum;
    }
}