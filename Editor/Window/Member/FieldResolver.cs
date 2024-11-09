using System;
using System.Reflection;
using UnityBehaviorTree.Runtime.Core;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window.Member
{
    sealed class Ordered : Attribute
    {
        public int Order = 100;
    }

    public interface IFieldResolver
    {
        VisualElement GetEditorField();

        void Restore(BaseNodeBehavior behavior);

        void Commit(BaseNodeBehavior behavior);
    }

    public abstract class FieldResolver<T, K> :IFieldResolver where T: BaseField<K>
    {
        internal readonly FieldInfo FieldInfo;
        internal T EditorField;

        protected FieldResolver(FieldInfo fieldInfo)
        {
            this.FieldInfo = fieldInfo;
            SetEditorField();
        }

        private void SetEditorField()
        {
            this.EditorField = CreateEditorField(FieldInfo);
        }

        protected abstract T CreateEditorField(FieldInfo fieldInfo);

        public VisualElement GetEditorField()
        {
            return this.EditorField;
        }

        public virtual void Restore(BaseNodeBehavior behavior)
        {
            EditorField.value = (K)FieldInfo.GetValue(behavior);
        }

        public virtual void Commit(BaseNodeBehavior behavior)
        {
           FieldInfo.SetValue(behavior, EditorField.value);
        }
    }
}