using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCM2020___Client
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
