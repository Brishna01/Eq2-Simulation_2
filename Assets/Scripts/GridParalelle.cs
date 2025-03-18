using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class  GridParalelle {
    private int width;
    private int height;
    //private Gridserie[,] gridArray;
    private int[,] gridArray;
    private float cellSizeX;
    private float cellSizeY;
    private TextMesh[,] debugTextArray;


    public GridParalelle(int width, int height, float cellSizeX, float cellSizeY)
    {
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;


        //gridArray = new Gridserie[width, height];
        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        

        for (int laColonne = 0; laColonne < gridArray.GetLength(0); laColonne++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                // gridArray[laColonne , j] = new Gridserie(1, lignes, 5f, laColonne +1);

                debugTextArray[laColonne,j] = UtilsClass.CreateWorldText(gridArray[laColonne, j].ToString(), null, GetWorldPosition(laColonne, j), 10, Color.white, TextAnchor.MiddleCenter);
               
                if(laColonne<gridArray.GetLength(0)-1 && j <gridArray.GetLength(1) -1) {
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne + 1, j), Color.white, 100f);

                }

                 //Debug.Log(laColonne + " , " + j);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height-1), GetWorldPosition(width-1, height-1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width-1, 0), GetWorldPosition(width-1, height-1), Color.white, 100f);
        

        //Debug.Log(width + " " + height);
       // SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int i, int j) 
    {
        return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
    }
   
    private void GetXY (Vector3 worldposition, out int x, out int y)
    {
       x = Mathf.FloorToInt( (worldposition.x +7.5f ) / cellSizeX);
       y = Mathf.FloorToInt( (worldposition.y +2f) / cellSizeY);
       // x = Mathf.FloorToInt((worldposition.x -7.5f ) / cellSizeX);
        //y = Mathf.FloorToInt((worldposition.y -2f) / cellSizeY);

    }
    public void SetValue(int x, int y, int value )
    {
       if (x >= 0  && y >= 0 && x < width   && y < height )
 //           if (x>= 0+7.5f && y>=0+2f && x< width +7.5f&& y < height + 2f)
        {
            gridArray[x , y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
           
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
