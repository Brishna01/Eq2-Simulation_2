using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class Gridserie 
{
    private int width;
    private int height;
    //private bool[,] gridSerie;
    private int[,] gridSerie;
    private float cellSize;

    public Gridserie(int width, int height, float cellSize, int colonne)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSerie = new int[1, 4];
        int numero = 0;

        for (int i = 0; i < gridSerie.GetLength(0); i++)
        {
            for (int j = 0; j < gridSerie.GetLength(1); j++)
            {
                numero++;
                gridSerie[i, j] = numero ;
                 Debug.Log(i + " , " + j);
                UtilsClass.CreateWorldText(gridSerie[i, j].ToString(), null, GetWorldPosition(i+colonne, j), 20, Color.white, TextAnchor.MiddleCenter);

            }
        }
        Debug.Log(width + " " + height);
    }
    private Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i, j) * cellSize;
    }

}
