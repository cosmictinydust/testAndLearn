using System;

namespace searchWithMiddle
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 21, 3, 6, 34, 35, 11, 3, 48, 84, 1 };
            int temp = 0;
            for (int i = 0; i <= arr.Length; i++) {
                for (int j = i; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        temp = arr[j];
                        arr[j] = arr[i];
                        arr[i] = temp;
                    }
                }
            }
            foreach (int num in arr)
            {
                Console.WriteLine(num);
            }
            int start = 0, end = arr.Length - 1, middle = (start + end) / 2;
            int arrayIndex = -1;
            int toBeFound=0 ;
            Console.WriteLine("请输入一个要查找的数字!");
            toBeFound = Convert.ToInt32(Console.ReadLine());
            while (arrayIndex == -1)
            {
                if (toBeFound == arr[middle])
                {
                    arrayIndex = middle;
                }
                if (start == middle)
                {
                    if (toBeFound == arr[end])
                    {
                        arrayIndex = end;
                    }
                    break;
                }
                if (toBeFound > arr[middle])
                {
                    start = middle + 1;
                    middle = (start + end) / 2;
                }
                else
                {
                    end = middle - 1;
                    middle = (start + end) / 2;
                }
            }
            if (arrayIndex == -1)
            {
                Console.WriteLine("要查找的值不存在!");
            }
            else
            {
                Console.WriteLine($"找到了,数组下标为 : {arrayIndex}");
            }
            Console.ReadKey();
        }
    }
}
