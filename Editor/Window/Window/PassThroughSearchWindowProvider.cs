using System;
using System.Collections.Generic;
using UnityBehaviorTree.Editor.Window.Node;
using UnityBehaviorTree.Runtime.Core.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Window
{
    public class PassThroughSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private readonly BaseBehaviorTreeNode node;

        public PassThroughSearchWindowProvider(BaseBehaviorTreeNode node)
        {
            this.node = node;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Select Pass Through")));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(PassThrough)))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
                    }
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as System.Type;
            this.node.SetBehavior(type);
            return true;
        }

    }
}