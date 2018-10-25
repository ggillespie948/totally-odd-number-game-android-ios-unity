using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void AddElementsS<T>( this List<T> list, int count )
    {
        list.AddRange( new T[count] );
    }
}
