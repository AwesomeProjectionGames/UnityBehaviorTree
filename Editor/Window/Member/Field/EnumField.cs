using System;
using System.Collections.Generic;
using UnityEditor.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member.Field
{
    public class EnumField : PopupField<Enum>
    {
        public EnumField(string label, List<Enum> choices, Enum defaultValue = null)
            : base(label, choices,  defaultValue, null, null) {
        }
    }
}