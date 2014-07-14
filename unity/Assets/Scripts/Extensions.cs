using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{

    public static void Shuffle<T>(this List<T> list)
    {
        int lastUnusedIndex = list.Count - 1;

        while (lastUnusedIndex > 0)
        {
            //Unity's generator generates [i,j); we need [i,j], where i and j are the first and last indexes of the list
            int randomIndex = UnityEngine.Random.Range(0, lastUnusedIndex + 1); 

            list.Swap(randomIndex, lastUnusedIndex);

            lastUnusedIndex--;
        }

    }

    public static void Swap<T>(this List<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}
