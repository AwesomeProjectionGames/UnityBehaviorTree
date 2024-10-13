using System;

namespace UnityBehaviorTree.Editor
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns true if the type is a class or subclass of the parent type.
        /// </summary>
        /// <param name="type">The type to check (can inherit)</param>
        /// <param name="parentType">The parent type to check against</param>
        /// <returns>True if the type is a class or subclass of the parent type (is or inherits)</returns>
        public static bool IsClassOrSubclassOf(this Type type, Type parentType)
        {
            return type.IsSubclassOf(parentType) || type == parentType;
        }
    }
}