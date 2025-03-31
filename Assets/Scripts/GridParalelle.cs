using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class GridParalelle : MonoBehaviour
{
    [field: SerializeField]
    public int colonnes { get; set; }
    [field: SerializeField]
    public int lignes { get; set; }
    //private Gridserie[,] gridArray;
    private int[,] gridArray;
    [SerializeField]
    private float cellSizeX;
    [SerializeField]
    private float cellSizeY;
    private TextMesh[,] debugTextArray;

    void Start()
    {
        gridArray = new int[colonnes, lignes];
        debugTextArray = new TextMesh[colonnes, lignes];

        for (int laColonne = 0; laColonne < gridArray.GetLength(0); laColonne++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                // gridArray[laColonne , j] = new Gridserie(1, lignes, 5f, laColonne +1);

                debugTextArray[laColonne, j] = UtilsClass.CreateWorldText(gridArray[laColonne, j].ToString(), null, GetWorldPosition(laColonne, j), 10, Color.white, TextAnchor.MiddleCenter);

                if (laColonne < gridArray.GetLength(0) - 1 && j < gridArray.GetLength(1) - 1)
                {
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(laColonne, j), GetWorldPosition(laColonne + 1, j), Color.white, 100f);

                }

                //Debug.Log(laColonne + " , " + j);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, lignes - 1), GetWorldPosition(colonnes - 1, lignes - 1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(colonnes - 1, 0), GetWorldPosition(colonnes - 1, lignes - 1), Color.white, 100f);

        //Debug.Log(width + " " + height);
        // SetValue(2, 1, 56);
    }

    void Update()
    {
         if (Input.GetMouseButtonDown(0))
         {
            SetValue(UtilsClass.GetMouseWorldPosition(), 56);
         }

    }

    private Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
    }

    private void GetXY(Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldposition.x + 7.5f) / cellSizeX);
        y = Mathf.FloorToInt((worldposition.y + 2f) / cellSizeY);
        // x = Mathf.FloorToInt((worldposition.x -7.5f ) / cellSizeX);
        //y = Mathf.FloorToInt((worldposition.y -2f) / cellSizeY);

    }
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < colonnes && y < lignes)
        //           if (x>= 0+7.5f && y>=0+2f && x< width +7.5f&& y < height + 2f)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();

        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);

    }

    public int GetValue(int x, int y)
    {

        if (x >= 0 && y >= 0 && x < colonnes && y < lignes)
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
