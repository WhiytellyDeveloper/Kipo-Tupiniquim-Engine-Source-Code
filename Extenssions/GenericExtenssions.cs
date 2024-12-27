using System;
using System.Collections.Generic;
using System.Text;

namespace KipoTupiniquimEngine.Extenssions
{
    public static class GenericExtenssions
    {
        public static List<int> GenerateLowList(int start, int step, int totalCount)
        {
            List<int> numberList = [];

            int currentNumber = start;

            for (int i = 0; i < totalCount; i++)
            {
                numberList.Add(currentNumber);
                currentNumber -= step;
            }

            return numberList;
        }

        public static List<T> Reverse<T>(this List<T> list)
        {
            List<T> reverseList = [];

            for (int i = list.Count - 1; i >= 0; i--)
                reverseList.Add(list[i]);

            return reverseList;
        }

        public static T ToWeighted<T, A>(this object obj, int weighted) where T : WeightedSelection<A>, new() where A : class
        {
            var castedObject = obj as A;

            if (castedObject == null)
                throw new InvalidCastException($"Unable to convert object to type {typeof(A).Name}.");

            T weightedSelection = new T
            {
                selection = castedObject,
                weight = weighted
            };

            return weightedSelection;
        }

        public static List<WeightedSelection<T>> Convert<T>(this List<T> list, int defaultWeight) where T : class
        {
            List<WeightedSelection<T>> weightedList = new List<WeightedSelection<T>>();
            foreach (T item in list)
            {
                weightedList.Add(new WeightedSelection<T>
                {
                    selection = item,
                    weight = defaultWeight
                });
            }
            return weightedList;
        }
    }
}
