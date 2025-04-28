using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class TestMatrices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Matrix<double> A = Matrix<double>.Build.Dense(3, 3);
        Vector<double> b = Vector<double>.Build.Dense(3);

        A[0, 0] = 3;
        A[0, 1] = 2;
        A[0, 2] = -1;
        A[1, 0] = 2;
        A[1, 1] = -2;
        A[1, 2] = 4;
        A[2, 0] = -1;
        A[2, 1] = 0.5;
        A[2, 2] = -1;
        b[0] = 1;
        b[1] = -2;
        b[2] = 0;
        
        Vector<double> x = A.Solve(b);

        foreach (float valeur in x.AsArray())
        {
            Debug.Log(valeur);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
