using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class TreeNode<T>
    {
        public T Item { get; set; }
        public TreeNodeCollection<T> Nodes { get; set; }
        [JsonIgnore]
        public TreeNode<T> Parent { get; set; }
        public TreeNode()
        {
            Nodes = new TreeNodeCollection<T>(this);
        }
        public TreeNode(T item)
        {
            this.Item = item;
            Nodes = new TreeNodeCollection<T>(this);
        }
    }
}
