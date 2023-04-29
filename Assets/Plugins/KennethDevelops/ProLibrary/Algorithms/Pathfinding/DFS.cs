using System;
using System.Collections.Generic;

namespace KennethDevelops.ProLibrary.Algorithms.Pathfinding{

    public static class DFS{

        /// <summary>
        /// Calculates a path using Depth First Search. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/DFS
        /// </summary>
        /// <param name="start">The node where it starts calculating the path</param>
        /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
        /// <param name="explode">A function that returns all the near neighbours of a given node</param>
        /// <typeparam name="T">Node type</typeparam>
        /// <returns>Returns a path from start node to goal</returns>
        public static IEnumerable<T> CalculatePath<T>(T start, Func<T, bool> isGoal, Func<T, IEnumerable<T>> explode){
            var path  = new List<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count > 0){
                var popped = stack.Pop();

                path.Add(popped);

                if (isGoal(popped)) return path;

                var toStack = explode(popped);

                foreach (var n in toStack) stack.Push(n);
            }

            return path;
        }

    }

}