using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class  GridParalelle:MonoBehaviour {
    private int width;
    private int height;
    private Gridserie[,] gridArray;
    //private int[,] serie;
    private float cellSize;
    public GridParalelle(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new Gridserie[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {


            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                int colonne = 1;
                gridArray[i , j] = new Gridserie(1, 4, 8f, colonne);
                colonne++;
                // Debug.Log(i + " , " + j);
               //  // UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null, GetWorldPosition(i, j), 50, Color.white, TextAnchor.MiddleCenter);

            }
        }
        Debug.Log(width + " " + height);
    }

    private Vector3 GetWorldPosition(int i, int j) 
    {
        return new Vector3(i, j) * cellSize;
    }
}
