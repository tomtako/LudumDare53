using System.Collections.Generic;
using KennethDevelops.ProLibrary.Algorithms.Pathfinding;

namespace KennethDevelops.ProLibrary.DataStructures{
    
    public class PriorityQueue<T>{

        private List<WeightedNode<T>> _queue = new List<WeightedNode<T>>();

        /// <summary>
        /// Adds an element to the Queue 
        /// </summary>
        /// <param name="element"></param>
        public void Enqueue(WeightedNode<T> element){
            _queue.Add(element);
        }

        /// <summary>
        /// Returns the element with minimum value in the Queue
        /// </summary>
        /// <returns></returns>
        public WeightedNode<T> Dequeue(){
            var min = default(WeightedNode<T>);
            var minWeight = float.MaxValue;

            foreach (var element in _queue){
                if (element.Weight >= minWeight) continue;
                min     = element;
                minWeight = element.Weight;
            }

            var newQueue = new List<WeightedNode<T>>();

            foreach (var element in _queue)
                if (element != min) newQueue.Add(element);

            _queue = newQueue;

            return min;
        }

        /// <summary>
        /// Returns true if the Queue does not contain any elements
        /// </summary>
        public bool IsEmpty{
            get{ return _queue.Count == 0; }
        }

    }
    
}