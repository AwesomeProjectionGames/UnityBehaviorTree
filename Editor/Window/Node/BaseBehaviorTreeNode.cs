using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityBehaviorTree.Editor.Window.Member;
using UnityBehaviorTree.Runtime.Core;
using UnityBehaviorTree.Runtime.Core.Annotation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityBehaviorTree.Editor.Window.Node
{
    /// <summary>
    /// The base for representing a behavior tree node in the GraphView editor.
    /// </summary>
    public abstract class BaseBehaviorTreeNode : UnityEditor.Experimental.GraphView.Node {
        /// <summary>
        /// The status of the node for coloring purposes.
        /// </summary>
        protected enum Status
        {
            Error,
            Running,
            Success,
            None
        }
        /// <summary>
        /// The attached behavior of the node this represents.
        /// </summary>
        protected BaseNodeBehavior NodeBehavior { set;  get; }
        private Type _dirtyNodeBehaviorType;
        
        public Port Parent { private set; get; }
        
        private readonly VisualElement _container;
        private readonly FieldResolverFactory _fieldResolverFactory;
        private readonly List<IFieldResolver> _resolvers = new List<IFieldResolver>();
        
        protected BaseBehaviorTreeNode()
        {
            _fieldResolverFactory = new FieldResolverFactory();
            _container = new VisualElement();
            Initialize();
        }

        private void Initialize()
        {
            mainContainer.Add(_container);
            AddParent();
        }

        public void Restore(BaseNodeBehavior behavior)
        {
            NodeBehavior = behavior;
            _resolvers.ForEach(e => e.Restore(NodeBehavior));
            NodeBehavior.NotifyEditor = MarkAsExecuted;
            OnRestore();
        }

        protected virtual void OnRestore()
        {

        }

        public BaseNodeBehavior ReplaceBehavior()
        {
            NodeBehavior = Activator.CreateInstance(GetBehavior()) as BaseNodeBehavior;
            return NodeBehavior;
        }

        protected virtual void AddParent()
        {
            Parent = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
            Parent.portName = "Parent";
            inputContainer.Add(Parent);
        }

        protected Port CreateChildPort(string portName = "Child")
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
            port.portName = portName;
            return port;
        }

        private Type GetBehavior()
        {
            return _dirtyNodeBehaviorType;
        }

        public void Commit(Stack<BaseBehaviorTreeNode> stack)
        {
            OnCommit(stack);
            _resolvers.ForEach( r => r.Commit(NodeBehavior));
            NodeBehavior.graphPosition = GetPosition();
            NodeBehavior.NotifyEditor = MarkAsExecuted;
        }
        protected abstract void OnCommit(Stack<BaseBehaviorTreeNode> stack);

        public bool Validate(Stack<BaseBehaviorTreeNode> stack)
        {
            var valid = GetBehavior() != null && OnValidate(stack);
            SetNodeStatus(valid ? Status.None : Status.Error);
            return valid;
        }

        protected abstract bool OnValidate(Stack<BaseBehaviorTreeNode> stack);

        public void SetBehavior(System.Type nodeBehavior)
        {
            if (_dirtyNodeBehaviorType != null)
            {
                _dirtyNodeBehaviorType = null;
                _container.Clear();
                _resolvers.Clear();
            }
            _dirtyNodeBehaviorType = nodeBehavior;

            nodeBehavior
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => field.GetCustomAttribute<HideInEditorWindow>() == null)
                .Concat(GetAllFields(nodeBehavior))
                .Where(field => field.IsInitOnly == false)
                .ToList().ForEach((p) =>
                {
                    var fieldResolver = _fieldResolverFactory.Create(p);
                    var defaultValue = Activator.CreateInstance(nodeBehavior) as BaseNodeBehavior;
                    fieldResolver.Restore(defaultValue);
                    _container.Add( fieldResolver.GetEditorField());
                    _resolvers.Add(fieldResolver);
                });
            title = nodeBehavior.Name;
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            return t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.GetCustomAttribute<SerializeField>() != null)
                .Where(field => field.GetCustomAttribute<HideInEditorWindow>() == null).Concat(GetAllFields(t.BaseType));
        }

        private void MarkAsExecuted(FrameResult status)
        {
            switch (status)
            {
                case FrameResult.Failure:
                {
                    SetNodeStatus(Status.Error);
                    break;
                }
                case FrameResult.Running:
                {
                    SetNodeStatus(Status.Running);
                    break;
                }
                case FrameResult.Success:
                {
                    SetNodeStatus(Status.Success);
                    break;
                }
            }
        }

        public void ClearStyle()
        {
            SetNodeStatus(Status.None);
            OnClearStyle();
        }

        protected abstract void OnClearStyle();

        protected void SetNodeStatus(Status status)
        {
            IStyle titleStyle = this.Q("title").style;
            switch (status)
            {
                case Status.Error:
                    titleStyle.backgroundColor = new StyleColor(new Color(0.5f, 0, 0));
                    break;
                case Status.Running:
                    titleStyle.backgroundColor = new StyleColor(new Color(0.35f, 0.35f, 0));
                    break;
                case Status.Success:
                    titleStyle.backgroundColor = new StyleColor(new Color(0, 0.5f, 0));
                    break;
                case Status.None:
                    titleStyle.backgroundColor = new StyleColor(StyleKeyword.Null);
                    break;
            }
        }
    }
}