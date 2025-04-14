using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;
using static UnityEngine.Rendering.DebugUI;

public class GridParalelle : MonoBehaviour
{
    [field: SerializeField]
    public int colonnes { get; set; }

    [field: SerializeField]
    public int lignes { get; set; }
    //private Gridserie[,] gridArray;

    private int[,] gridArray;
    // private GameObject[,] gridArrayObjet;
    private ElementCircuit[,] gridArrayObjet;

    [SerializeField]
    private float cellSizeX;
    [SerializeField]
    private float cellSizeY;
    [SerializeField]
    private bool modeDebug;

    private float origineGrilleX;
    private float origineGrilleY;

    //private GameObject objet;
    private ElementCircuit objet;
  

    private TextMesh[,] debugTextArray;

    void Start()
    {
        gridArray = new int[colonnes, lignes];
        //gridArrayObjet = new GameObject[colonnes, lignes];
        gridArrayObjet = new ElementCircuit[colonnes, lignes];

        debugTextArray = new TextMesh[colonnes, lignes];


        origineGrilleX = (float)(0 - colonnes * cellSizeX * 0.5);
        origineGrilleY = (float)(0 - lignes * cellSizeY * 0.5);


        if (modeDebug)
        {
            for (int laColonne = 0; laColonne < gridArrayObjet.GetLength(0); laColonne++)
            {
                for (int laLigne = 0; laLigne < gridArrayObjet.GetLength(1); laLigne++)
                {
                    // gridArray[laColonne , j] = new Gridserie(1, lignes, 5f, laColonne +1);

                    debugTextArray[laColonne, laLigne] = UtilsClass.CreateWorldText(gridArray[laColonne, laLigne].ToString(), null, 
                        GetWorldPosition(laColonne, laLigne) + new Vector3(cellSizeX, cellSizeY) * 0.5f, 
                        5, Color.white, TextAnchor.MiddleCenter);

                    if (laColonne < gridArrayObjet.GetLength(0) - 1 && laLigne < gridArrayObjet.GetLength(1) - 1)
                    {
                        Debug.DrawLine(GetWorldPositionMoitie(laColonne, laLigne), GetWorldPositionMoitie(laColonne, laLigne + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPositionMoitie(laColonne, laLigne), GetWorldPositionMoitie(laColonne + 1, laLigne), Color.white, 100f);

                    }

                    gridArrayObjet[laColonne, laLigne] = new Resistance();

                    //Debug.Log(laColonne + " , " + j);
                }
            }
            Debug.DrawLine(GetWorldPositionMoitie(0, lignes - 1), GetWorldPositionMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
            Debug.DrawLine(GetWorldPositionMoitie(colonnes - 1, 0), GetWorldPositionMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
            //Debug.Log(width + " " + height);
            // SetValue(2, 1, 56);
        }
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            SetValue(UtilsClass.GetMouseWorldPosition(), 56, objet);
        }*/
    }

    private Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i * cellSizeY + origineGrilleX, j * cellSizeY + origineGrilleY);
        //return new Vector3(i , j )*cellSizeY;
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
    }

    private Vector3 GetWorldPositionMoitie(int i, int j)
    {
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
        return new Vector3(i * cellSizeY + 0.5f + origineGrilleX, j * cellSizeY + 0.5f + origineGrilleY);
    }


    private void GetXY(Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldposition.x - origineGrilleX) / cellSizeX);
        y = Mathf.FloorToInt((worldposition.y - origineGrilleY) / cellSizeY);
        // x = Mathf.FloorToInt((worldposition.x -7.5f ) / cellSizeX);
        //y = Mathf.FloorToInt((worldposition.y -2f) / cellSizeY);

    }
    public void SetValue(int i, int j, int value, ElementCircuit element)
    {
        if (i >= 0 && j >= 0 && i < colonnes && j < lignes)

        {
            gridArray[i, j] = value;
            debugTextArray[i, j].text = gridArray[i, j].ToString();
            //gridArrayObjet[i, j] = objet;
            gridArrayObjet[i, j] = element;


        }
    }

    //public void SetValue(Vector3 worldPosition, int value, GameObject objet)
        public void SetValue(Vector3 worldPosition, int value, ElementCircuit element)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        //SetValue(x, y, value, objet);
        SetValue(x, y, value, element);

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

    public void VerifierElements()
    {
        for (int laColonne = 0; laColonne < gridArray.GetLength(0); laColonne++)
        {
            for (int laLigne = 0; laLigne < gridArray.GetLength(1); laLigne++)
            {
               if(gridArrayObjet[laColonne, laLigne] == null && gridArray[laColonne, laLigne] != 0)
                {

                    Debug.Log("youppi!");
                    SetValue(laColonne, laLigne, 0, null);

                    /*debugTextArray[laColonne, laLigne] = UtilsClass.CreateWorldText(  "0"   , null,
                        GetWorldPosition(laColonne, laLigne) + new Vector3(cellSizeX, cellSizeY) * 0.5f, 
                        5, Color.white, TextAnchor.MiddleCenter);*/
                    /* gridArray[laColonne, laLigne] = 0;

                     debugTextArray[laColonne, laLigne] = UtilsClass.CreateWorldText(gridArray[laColonne, laLigne].ToString(), null, 
                         GetWorldPosition(laColonne, laLigne) + new Vector3(cellSizeX, cellSizeY) * 0.5f, 
                         5, Color.white, TextAnchor.MiddleCenter);
                    */
                }




            }
        }
    }
}
