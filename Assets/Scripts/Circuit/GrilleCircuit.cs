using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GrilleCircuit : MonoBehaviour
{
    [field: SerializeField]
    public int colonnes { get; private set; }
    [field: SerializeField]
    public int lignes { get; private set; }

    [SerializeField]
    private float tailleCelluleX;
    [SerializeField]
    private float tailleCelluleY;
    [SerializeField]
    private bool modeDebug;

    public float origineGrilleX { get; private set; }
    public float origineGrilleY { get; private set; }

    private ElementCircuit[,] matriceElements;
    private int[,] matriceValeurs;
    private TextMesh[,] debugTextArray;

    void Awake()
    {
        matriceElements = new ElementCircuit[colonnes, lignes];
        matriceValeurs = new int[colonnes, lignes];
        debugTextArray = new TextMesh[colonnes, lignes];

        origineGrilleX = (float)(0 - colonnes * tailleCelluleX * 0.5);
        origineGrilleY = (float)(0 - lignes * tailleCelluleY * 0.5);

        if (colonnes % 2 == 0)
        {
            origineGrilleX += 0.5f;
        }

        if (lignes % 2 == 0)
        {
            origineGrilleY += 0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (modeDebug)
        {
            for (int laColonne = 0; laColonne < matriceValeurs.GetLength(0); laColonne++)
            {
                for (int laLigne = 0; laLigne < matriceValeurs.GetLength(1); laLigne++)
                {
                    // gridArray[laColonne , j] = new Gridserie(1, lignes, 5f, laColonne +1);

                    debugTextArray[laColonne, laLigne] = UtilsClass.CreateWorldText(
                        matriceValeurs[laColonne, laLigne].ToString(), 
                        null, 
                        GetPositionMonde(laColonne, laLigne) + new Vector3(tailleCelluleX, tailleCelluleY) * 0.5f, 
                        5, 
                        Color.white, 
                        TextAnchor.MiddleCenter
                    );

                    if (laColonne < matriceValeurs.GetLength(0) - 1 && laLigne < matriceValeurs.GetLength(1) - 1)
                    {
                        Debug.DrawLine(GetPositionMondeMoitie(laColonne, laLigne), GetPositionMondeMoitie(laColonne, laLigne + 1), Color.white, 100f);
                        Debug.DrawLine(GetPositionMondeMoitie(laColonne, laLigne), GetPositionMondeMoitie(laColonne + 1, laLigne), Color.white, 100f);

                    }
                    //Debug.Log(laColonne + " , " + j);
                }
            }
            Debug.DrawLine(GetPositionMondeMoitie(0, lignes - 1), GetPositionMondeMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
            Debug.DrawLine(GetPositionMondeMoitie(colonnes - 1, 0), GetPositionMondeMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
            //Debug.Log(width + " " + height);
            // SetValue(2, 1, 56);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ElementCircuit GetElement(int x, int y)
    {
        return matriceElements[x, y];
    }

    public ElementCircuit GetElement(Vector3 positionMonde)
    {
        int x, y;
        GetXY(positionMonde, out x, out y);
        return GetElement(x, y);
    }

    public void SetElement(int x, int y, ElementCircuit element)
    {
        matriceElements[x, y] = element;

        if (element != null)
        {
            if (element.positionGrilleX >= 0 && element.positionGrilleY >= 0)
            {
                SetElement(element.positionGrilleX, element.positionGrilleY, null);
                SetValeur(element.positionGrilleX, element.positionGrilleY, 0);
            }

            element.positionGrilleX = x;
            element.positionGrilleY = y;
        }
    }

    public void SetElement(Vector3 positionMonde, ElementCircuit element)
    {
        int x, y;
        GetXY(positionMonde, out x, out y);
        SetElement(x, y, element);
    }

    public int GetValeur(int x, int y)
    {

        if (x >= 0 && y >= 0 && x < colonnes && y < lignes)
        {
            return matriceValeurs[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValeur(Vector3 positionMonde)
    {
        int x, y;
        GetXY(positionMonde, out x, out y);
        return GetValeur(x, y);
    }

    public void SetValeur(int i, int j, int valeur)
    {
        if (i >= 0 && j >= 0 && i < colonnes && j < lignes)
        {
            matriceValeurs[i, j] = valeur;
            debugTextArray[i, j].text = matriceValeurs[i, j].ToString();
        }
    }

    public void SetValeur(Vector3 positionMonde, int valeur)
    {
        int x, y;
        GetXY(positionMonde, out x, out y);
        SetValeur(x, y, valeur);
    }

    private void GetXY(Vector3 positionMonde, out int x, out int y)
    {
        x = Mathf.FloorToInt((positionMonde.x - origineGrilleX) / tailleCelluleX);
        y = Mathf.FloorToInt((positionMonde.y - origineGrilleY) / tailleCelluleY);
    }

    public Vector3 GetPositionGrille(Vector3 positionMonde)
    {
        int x = Mathf.FloorToInt((positionMonde.x - origineGrilleX) / tailleCelluleX);
        int y = Mathf.FloorToInt((positionMonde.y - origineGrilleY) / tailleCelluleY);

        return new Vector3(x, y, 0);
    }

    private Vector3 GetPositionMonde(int i, int j)
    {
        return new Vector3(i * tailleCelluleX + origineGrilleX, j * tailleCelluleY + origineGrilleY);
        //return new Vector3(i , j )*cellSizeY;
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
    }

    private Vector3 GetPositionMondeMoitie(int i, int j)
    {
        //return new Vector3(i * cellSizeX - 7.5f, j * cellSizeY - 2f);
        //return new Vector3(i , j )*cellSizeY;
        return new Vector3(i * tailleCelluleX + 0.5f + origineGrilleX, j * tailleCelluleY + 0.5f + origineGrilleY);
    }
}
