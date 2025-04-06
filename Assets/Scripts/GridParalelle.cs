using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class  GridParalelle {
    private int colonnes;
    private int lignes;

    private float origineGrilleX;
    private float origineGrilleY;

    private int[,] gridArray;
    private GameObject[,] gridArrayObjet;

    private float cellSizeX;
    private float cellSizeY;
    private TextMesh[,] debugTextArray;


    public GridParalelle(int colonnes, int lignes, float cellSizeX, float cellSizeY)
    {
        this.colonnes = colonnes;
        this.lignes = lignes;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;

        origineGrilleX = (float)(0 -colonnes*cellSizeX* 0.5);
        origineGrilleY = (float)(0 - lignes * cellSizeY * 0.5);

       
        gridArray = new int[colonnes, lignes];
        gridArrayObjet = new GameObject[colonnes, lignes];

        debugTextArray = new TextMesh[colonnes, lignes];
        

        for (int laColonne = 0; laColonne < gridArrayObjet.GetLength(0); laColonne++)
        {
            for (int j = 0; j < gridArrayObjet.GetLength(1); j++)
            {
              
               debugTextArray[laColonne,j] = UtilsClass.CreateWorldText(gridArray[laColonne, j].ToString(), null, GetWorldPosition(laColonne, j) + new Vector3(cellSizeX, cellSizeY)*0.5f, 5, Color.white, TextAnchor.MiddleCenter);   
                //a changer pour mettre objet
               
                if(laColonne<gridArrayObjet.GetLength(0)-1 && j <gridArrayObjet.GetLength(1) -1) {
                    Debug.DrawLine(GetWorldPositionMoitie(laColonne , j), GetWorldPositionMoitie(laColonne, j+1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPositionMoitie(laColonne, j), GetWorldPositionMoitie(laColonne + 1 , j), Color.white, 100f);

                }

                 //Debug.Log(laColonne + " , " + j);
            }
        }
        Debug.DrawLine(GetWorldPositionMoitie(0, lignes-1), GetWorldPositionMoitie(colonnes-1, lignes-1), Color.white, 100f);
        Debug.DrawLine(GetWorldPositionMoitie(colonnes-1, 0), GetWorldPositionMoitie(colonnes-1, lignes-1), Color.white, 100f);
        

        //Debug.Log(width + " " + height);
    }

    private Vector3 GetWorldPosition(int i, int j) 
    {
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
        return new Vector3(i * cellSizeY + origineGrilleX, j * cellSizeY+origineGrilleY) ;
    }

    private Vector3 GetWorldPositionMoitie(int i, int j)
    {
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
        return new Vector3(i * cellSizeY +0.5f +origineGrilleX, j * cellSizeY + 0.5f + origineGrilleY) ;
    }

    private void GetXY (Vector3 worldposition, out int x, out int y)
    {
       x = Mathf.FloorToInt( (worldposition.x  -origineGrilleX  ) / cellSizeX);
       y = Mathf.FloorToInt( (worldposition.y - origineGrilleY ) / cellSizeY);
       // x = Mathf.FloorToInt((worldposition.x -7.5f ) / cellSizeX);
        //y = Mathf.FloorToInt((worldposition.y -2f) / cellSizeY);

    }
    public void SetValue(int x, int y, int value , GameObject objet)
    {
       if (x >= 0  && y >= 0 && x < colonnes   && y < lignes )
 //           if (x>= 0+7.5f && y>=0+2f && x< width +7.5f&& y < height + 2f)
        {
            gridArray[x , y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();

            gridArrayObjet[x, y] = objet;

        }

    }
    public void SetValue(Vector3 worldPosition, int value, GameObject objet)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value, objet);
     
    }





    public int GetValue(int x, int y) {

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
