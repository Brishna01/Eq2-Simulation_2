using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class  GridParalelle: MonoBehaviour {
    private int width;
    private int height;
    //private Gridserie[,] gridArray;
    private int[,] gridArray;
    private float cellSize;
    private int lignes = 4;
    public GridParalelle(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;


        //gridArray = new Gridserie[width, height];
        gridArray = new int[width, height];
        

        for (int laColonne = 0; laColonne < gridArray.GetLength(0); laColonne++)
        {


            for (int j = 0; j < gridArray.GetLength(1); j++)
            {

                // gridArray[laColonne , j] = new Gridserie(1, lignes, 5f, laColonne +1);

                UtilsClass.CreateWorldText(gridArray[laColonne, j].ToString(), null, GetWorldPosition(laColonne, j), 20, Color.white, TextAnchor.MiddleCenter);
                if(laColonne<gridArray.GetLength(0)-1 && j <gridArray.GetLength(1) -1) {
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne + 1, j), Color.white, 100f);

                }

                // Debug.Log(i + " , " + j);
                //  // UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null, GetWorldPosition(i, j), 50, Color.white, TextAnchor.MiddleCenter);

                //SetValuee(2, 1, 56 );
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height-1), GetWorldPosition(width-1, height-1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width-1, 0), GetWorldPosition(width-1, height-1), Color.white, 100f);

        Debug.Log(width + " " + height);
    }

    private Vector3 GetWorldPosition(int i, int j) 
    {
        return new Vector3(i * cellSize, j * cellSize - 4.5f);
    }
   
    private void GetXY (Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldposition.x / cellSize);
        y = Mathf.FloorToInt(worldposition.y +4.5f/ cellSize);

    }
    public void SetValue(int x, int y, int value )
    {
        if (x>=0 && y>=0 && x<width-1 && y < height-1)
        {
            gridArray[x, y] = value;
           // debugTextArray[x, y].text = gridArray[x, y].ToString();
        }

    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);

    }

    public int GetValue(int x, int y) {

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
