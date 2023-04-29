using System;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class ArrayExtensions{

        /// <summary>
        /// Returns a random item of the List
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> list){
            var random = UnityRandom.Range(0, list.Count);
            return list[random];
        }

        /// <summary>
        /// Returns a random item of the Array
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomItem<T>(this T[] array){
            var random = UnityRandom.Range(0, array.Length);
            return array[random];
        }

        /// <summary>
        /// Returns a new randomized(items changes places randomly) List.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Randomize<T>(this List<T> list){
            var newList = new List<T>(list);

            var n      = newList.Count;
            var random = new Random();

            while (n > 1){
                n--;

                var k = random.Next(n + 1);

                var value = newList[k];
                newList[k] = newList[n];
                newList[n] = value;
            }

            return newList;
        }

        /// <summary>
        /// Returns a new randomized(items changes places randomly) Array.
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Randomize<T>(this T[] array){
            var newArray = (T[])array.Clone();
            
            var n      = newArray.Length;
            var random = new Random();

            while (n > 1){
                n--;

                var k = random.Next(n + 1);

                var value = newArray[k];
                newArray[k] = newArray[n];
                newArray[n] = value;
            }

            return newArray;
        }
        
    }
    
}