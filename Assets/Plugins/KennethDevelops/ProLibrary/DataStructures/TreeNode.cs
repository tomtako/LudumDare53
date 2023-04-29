using System;
using System.Collections.Generic;
using UnityEngine;

namespace KennethDevelops.ProLibrary.DataStructures{
    
    [Serializable]
    public class TreeNode<T>{
        
        public T element;
        public TreeNode<T> parent;
        public List<TreeNode<T>> leaves = new List<TreeNode<T>>();


        public TreeNode(T element, TreeNode<T> parent){
            this.element = element;
            this.parent = parent;
        }

        public void AddLeaf(TreeNode<T> leaf){
            leaves.Add(leaf);
        }

        public void AddLeaf(T leaf){
            AddLeaf(new TreeNode<T>(leaf, this));
        }

        public void RemoveLeaf(TreeNode<T> leaf){
            if (leaves.Contains(leaf)) leaves.Remove(leaf);
        }

        public bool IsRoot(){
            return parent == null;
        } 
    }
    
}