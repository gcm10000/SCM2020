using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public static class TreeNodeExtensions
    {
        public static bool IsDescendant<T>(this TreeNode<T> child, TreeNode<T> parent)
        {
            var node = child.Parent;
            while (node != null)
            {
                if (node == parent)
                {
                    return true;
                }
                node = node.Parent;
            }

            return false;
        }
    }
}
