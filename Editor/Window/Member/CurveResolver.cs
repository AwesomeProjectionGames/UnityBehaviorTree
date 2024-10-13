using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Member
{
    public class CurveResolver : FieldResolver<CurveField, AnimationCurve>
    {
        public CurveResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override CurveField CreateEditorField(FieldInfo fieldInfo)
        {
            return new CurveField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(AnimationCurve);
    }
}