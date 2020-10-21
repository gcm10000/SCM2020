using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class TreeNodeCollection<T> : CollectionBase
    {
        private TreeNode<T> TreeNodeCurrent;
        public TreeNodeCollection() {  }
        public TreeNodeCollection(TreeNode<T> treeNode) 
        {
            this.TreeNodeCurrent = treeNode;
        }
        public TreeNode<T> this[int index]
        {
            get
            {
                return (TreeNode<T>)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }
        public int IndexOf(TreeNode<T> treeNode)
        {
            if (treeNode != null)
            {
                return base.List.IndexOf(treeNode);
            }
            return -1;
        }
        public int Add(TreeNode<T> treeNode)
        {
            if (treeNode != null)
            {
                treeNode.Parent = TreeNodeCurrent;
                return this.List.Add(treeNode);
            }
            return -1;
        }
        public void Remove(TreeNode<T> treeNode)
        {
            this.InnerList.Remove(treeNode);
        }
        public void AddRange(TreeNodeCollection<T> treeNode)
        {
            if (treeNode != null)
            {
                this.InnerList.AddRange(treeNode);
            }
        }
        public void Insert(int index, TreeNode<T> treeNode)
        {
            if (index <= List.Count && treeNode != null)
            {
                this.List.Insert(index, treeNode);
            }
        }
        public bool Contains(TreeNode<T> treeNode)
        {
            return this.List.Contains(treeNode);
        }

    }
}
