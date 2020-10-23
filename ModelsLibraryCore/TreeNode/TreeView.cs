using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class TreeView<T>
    {
        public TreeView()
        {
            Nodes = new TreeNodeCollection<T>();
        }
        public TreeNodeCollection<T> Nodes { get; }

        private ICollection<TreeNode<T>> MyNodes;
        public ICollection<TreeNode<T>> GetAllNodes()
        {
            MyNodes = new List<TreeNode<T>>();
            foreach (TreeNode<T> subnode in Nodes)
            {
                MyNodes.Add(subnode);
                ListNodes(subnode);
            }
            return MyNodes;
        }

        private void ListNodes(TreeNode<T> node)
        {
            foreach (TreeNode<T> subnode in node.Nodes)
            {
                MyNodes.Add(subnode);
                ListNodes(subnode);
            }
        }
    }
}
