using System;
using System.Collections.Generic;
using KennethDevelops.ProLibrary.DataStructures;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Algorithms.Pathfinding{
    
    public static class ThetaStar{

        /// <summary>
        /// Calculates a path using ThetaStar. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/ThetaStar
        /// </summary>
        /// <param name="start">The node where it starts calculating the path</param>
        /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
        /// <param name="explode">A function that returns all the near neighbours of a given node</param>
        /// <param name="getHeuristic">A function that returns the heuristic of the given node</param>
        /// <param name="getCost">A function that returns the cost to transition from one node to another</param>
        /// <param name="lineOfSight">A function that returns whether the path from one node to another can be direct or not (because of obstacles in the way)</param>
        /// <typeparam name="T">Node type</typeparam>
        /// <returns>Returns a path from start node to goal. Returns null if a path could not be found</returns>
        public static IEnumerable<T> CalculatePath<T>(T                       start,
                                                      Func<T, bool>           isGoal,
                                                      Func<T, IEnumerable<T>> explode,
                                                      Func<T, T, float>       getCost,
                                                      Func<T, T, bool>        lineOfSight,
                                                      Func<T, float>          getHeuristic){
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

                foreach (var neighbour in toEnqueue){
                    var neighbourToDequeueDistance = getCost(dequeued.Element, neighbour);

                    var startToNeighbourDistance =
                        distances.ContainsKey(neighbour) ? distances[neighbour] : float.MaxValue;
                    var startToDequeuedDistance = distances[dequeued.Element];

                    var newDistance = startToDequeuedDistance + neighbourToDequeueDistance;

                    if (visited.Contains(neighbour)) continue;
                    T parent;

                    if (parents.TryGetValue(dequeued.Element, out parent) && lineOfSight(parent, neighbour)){
                        var startToParentDistance     = distances[parent];
                        var parentToNeighbourDistance = getCost(parent, neighbour);

                        newDistance = startToParentDistance + parentToNeighbourDistance;

                        if (newDistance < startToNeighbourDistance){
                            distances[neighbour] = newDistance;
                            parents[neighbour]   = parent;

                            queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));
                        }
                    }
                    else if (newDistance < startToNeighbourDistance){
                        distances[neighbour] = newDistance;
                        parents[neighbour]   = dequeued.Element;
                        queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));
                    }

                }

            }

            return null;
        }

    }
    
}