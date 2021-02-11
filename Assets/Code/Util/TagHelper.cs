using System.Collections.Generic;
using UnityEngine;

public static class TagHelper
{
    /// <summary>
    /// Recurse upward in hierarchy to find the first parent with the given tag
    /// </summary>
    /// <param name="child">Game object to start from</param>
    /// <param name="tag">The given tag</param>
    /// <param name="checkSelf">Should the given object also be checked?</param>
    /// <returns></returns>
    public static GameObject FindTagInParents(in GameObject child, in string tag, bool checkSelf = true)
    {
        Transform node = child.transform;

        if (!checkSelf) {
            node = node.parent;
        }

        while (node != null) {
            bool found = node.CompareTag(tag);

            if (found) {
                return node.gameObject;
            }

            node = node.parent;
        }

        return null;
    }

    /// <summary>
    /// Perform a depth first search to find the first child with the given tag
    /// </summary>
    /// <param name="root">Game object to start from</param>
    /// <param name="tag">The given tag</param>
    /// <param name="checkSelf">Should the given object also be checked?</param>
    /// <returns></returns>
    public static GameObject FindTagInChildren(in GameObject root, in string tag)
    {
        if (!root || tag.CompareTo("") == 0) {
            return null;
        }

        Transform node = root.transform;

        Stack<Transform> yetToCheck = new Stack<Transform>();

        yetToCheck.Push(node);

        while (yetToCheck.Count > 0) {
            node = yetToCheck.Pop();
            if (node.CompareTag(tag)) {
                return node.gameObject;
            }

            foreach(Transform children in node.transform) {
                yetToCheck.Push(children);
            }
        }

        return null;
    }
}
