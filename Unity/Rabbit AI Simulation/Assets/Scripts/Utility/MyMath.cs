using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath : MonoBehaviour
{
    // Get geometric mean of an array of floats
    static public float GeometricMean(List<float> arr)
    {
        if (arr.Count == 0) return 0;

        // declare product variable and initialize it to 1. 
        float product = 1;

        // Compute the product of all the elements in the array. 
        for (int i = 0; i < arr.Count; i++)
            product = product * arr[i];

        // compute geometric mean by formula pow(product, 1/n) 
        float gm = (float)Mathf.Pow(product, (float)1 / arr.Count);
        return gm;
    }
}
