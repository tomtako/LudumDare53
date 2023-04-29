using System;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Algorithms.Pathfinding{
    
    [Serializable]
    public class WeightedNode<T>{
        
        [SerializeField]
        private T _element;
        [SerializeField]
        private float _weight;

        public T Element{
            get{ return _element; }
        }

        public float Weight{
            get{ return _weight; }
        }


        public WeightedNode(T element, float weight){
            _element = element;
            _weight = weight;
        }
        
    }
    
}