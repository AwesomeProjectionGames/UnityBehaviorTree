using System;
using System.Collections.Generic;
using UnityBehaviorTree.Editor.Window.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UnityBehaviorTree.Editor.Window.Window
{
    public class ActionSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private readonly BaseBehaviorTreeNode node;

        public ActionSearchWindowProvider(BaseBehaviorTreeNode node)
        {
            this.node = node;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Select Action")));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Action)))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
                    }
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;
            this.node.SetBehavior(type);

            return true;
        }

    }
}