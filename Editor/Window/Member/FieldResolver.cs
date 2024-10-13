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
        private readonly FieldInfo _fieldInfo;
        private T _editorField;

        protected FieldResolver(FieldInfo fieldInfo)
        {
            this._fieldInfo = fieldInfo;
            SetEditorField();
        }

        private void SetEditorField()
        {
            this._editorField = CreateEditorField(_fieldInfo);
        }

        protected abstract T CreateEditorField(FieldInfo fieldInfo);

        public VisualElement GetEditorField()
        {
            return this._editorField;
        }

        public void Restore(BaseNodeBehavior behavior)
        {
            _editorField.value = (K)_fieldInfo.GetValue(behavior);
        }

        public void Commit(BaseNodeBehavior behavior)
        {
           _fieldInfo.SetValue(behavior, _editorField.value);
        }
    }
}