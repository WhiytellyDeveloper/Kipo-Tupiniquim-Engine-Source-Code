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
    }
}
