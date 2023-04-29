using System;
using System.Collections;
using System.Collections.Generic;
using KennethDevelops.ProLibrary.DataStructures;
using UnityEngine.U2D;

namespace KennethDevelops.ProLibrary.Algorithms.Pathfinding{
    
    public static class Dijkstra{

        /// <summary>
        /// Calculates a path using Dijkstra. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/Dijkstra
        /// </summary>
        /// <param name="start">The node where it starts calculating the path</param>
        /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
        /// <param name="explode">A function that returns all the near neighbours of a given node</param>
        /// <typeparam name="T">Node type</typeparam>
        /// <returns>Returns a path from start node to goal. Returns null if a path could not be found</returns>
        public static IEnumerable<T> CalculatePath<T>(T                                     start,
                                                      Func<T, bool>                         isGoal,
                                                      Func<T, IEnumerable<WeightedNode<T>>> explode){
            var queue     = new PriorityQueue<T>();
            var distances = new Dictionary<T, float>();
            var parents   = new Dictionary<T, T>();
            var visited   = new HashSet<T>();

            distances[start] = 0;
            queue.Enqueue(new WeightedNode<T>(start, 0));

            while (!queue.IsEmpty){
                var dequeued = queue.Dequeue();
                visited.Add(dequeued.Element);

                if (isGoal(dequeued.Element)) return CommonUtils.CreatePath(parents, dequeued.Element);

                var toEnqueue = explode(dequeued.Element);

                foreach (var transition in toEnqueue){
                    var element                 = transition.Element;
                    var elementToPoppedDistance = transition.Weight;

                    var startToElementDistance = distances.ContainsKey(element) ? distances[element] : float.MaxValue;
                    var startToPoppedDistance  = distances[dequeued.Element];

                    var newDistance = startToPoppedDistance + elementToPoppedDistance;

                    if (!visited.Contains(element) && startToElementDistance > newDistance){
                        distances[element] = newDistance;
                        parents[element]   = dequeued.Element;

                        queue.Enqueue(new WeightedNode<T>(element, newDistance));
                    }
                }
            }

            return null;
        }



    }
    
}