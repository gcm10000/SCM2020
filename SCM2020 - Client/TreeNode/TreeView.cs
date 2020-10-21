using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class TreeView<T>
    {
        public TreeView()
        {
            Nodes = new TreeNodeCollection<T>();
        }
        public TreeNodeCollection<T> Nodes { get; }

        private List<TreeNode<T>> MyNodes;
        /// <summary>
        /// Recebe todos os nós horizontalmente.
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<T>> GetAllNodes()
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
