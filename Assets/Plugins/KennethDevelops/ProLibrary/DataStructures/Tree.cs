using System;

namespace KennethDevelops.ProLibrary.DataStructures{
    
    [Serializable]
    public class Tree<T>{

        public TreeNode<T> root;


        public Tree(T element){
            root = new TreeNode<T>(element, null);
        }

        public Tree(TreeNode<T> root){
            this.root = root;
        }

    }
}