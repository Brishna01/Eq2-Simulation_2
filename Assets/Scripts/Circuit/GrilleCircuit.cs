using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

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

    public float origineX { get; private set; }
    public float origineY { get; private set; }

    private Dictionary<(Vector2, Vector2), ElementCircuit> matriceElements;
    private Dictionary<(Vector2, Vector2), FilElectrique> matriceFils;
    private Dictionary<(Vector2, Vector2), int> matriceValeurs;
    private Dictionary<(Vector2, Vector2), TextMesh> debugTextArray;

    void Awake()
    {
        matriceElements = new Dictionary<(Vector2, Vector2), ElementCircuit>();
        matriceFils = new Dictionary<(Vector2, Vector2), FilElectrique>();
        matriceValeurs = new Dictionary<(Vector2, Vector2), int>();
        debugTextArray = new Dictionary<(Vector2, Vector2), TextMesh>();

        origineX = (float)(0 - (colonnes - 1) * tailleCelluleX * 0.5);
        origineY = (float)(0 - (lignes - 1) * tailleCelluleY * 0.5);

        if (colonnes % 2 == 0)
        {
            origineX += 0.5f;
        }

        if (lignes % 2 == 0)
        {
            origineY += 0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (modeDebug)
        {
            for (int ligne = 0; ligne < lignes; ligne++)
            {
                for (int colonne = 0; colonne < colonnes; colonne++)
                {
                    Vector2 point = new Vector2(colonne, ligne);
                    Vector2 pointDroite = new Vector2(colonne + 1, ligne);
                    Vector2 pointHaut = new Vector2(colonne, ligne + 1);

                    if (colonne < colonnes - 1)
                    {
                        debugTextArray[(point, pointDroite)] = UtilsClass.CreateWorldText(
                            0.ToString(), 
                            null, 
                            GetPositionMonde(colonne, ligne) + new Vector3(tailleCelluleX, 0) * 0.5f, 
                            5, 
                            Color.white, 
                            TextAnchor.MiddleCenter
                        );
                    }
                    
                    if (ligne < lignes - 1)
                    {
                        debugTextArray[(point, pointHaut)] = UtilsClass.CreateWorldText(
                            0.ToString(), 
                            null, 
                            GetPositionMonde(colonne, ligne) + new Vector3(0, tailleCelluleY) * 0.5f, 
                            5, 
                            Color.white, 
                            TextAnchor.MiddleCenter
                        );
                    }
                }
            }
            

            for (int laColonne = 0; laColonne < colonnes; laColonne++)
            {
                for (int laLigne = 0; laLigne < lignes; laLigne++)
                {
                    // debugTextArray[(new Vector2(laColonne, laLigne), new Vector2(laColonne, laLigne + 1))] = UtilsClass.CreateWorldText(
                    //     0.ToString(), 
                    //     null, 
                    //     GetPositionMonde(laColonne, laLigne) + new Vector3(tailleCelluleX, tailleCelluleY) * 0.5f, 
                    //     5, 
                    //     Color.white, 
                    //     TextAnchor.MiddleCenter
                    // );

                    if (laColonne < colonnes - 1 && laLigne < lignes - 1)
                    {
                        Debug.DrawLine(GetPositionMondeMoitie(laColonne, laLigne), GetPositionMondeMoitie(laColonne, laLigne + 1), Color.white, 100f);
                        Debug.DrawLine(GetPositionMondeMoitie(laColonne, laLigne), GetPositionMondeMoitie(laColonne + 1, laLigne), Color.white, 100f);
                    }
                }
            }
            Debug.DrawLine(GetPositionMondeMoitie(0, lignes - 1), GetPositionMondeMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
            Debug.DrawLine(GetPositionMondeMoitie(colonnes - 1, 0), GetPositionMondeMoitie(colonnes - 1, lignes - 1), Color.white, 100f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ElementCircuit GetElement(Vector2 point1, Vector2 point2)
    {
        ElementCircuit element;
        matriceElements.TryGetValue((point1, point2), out element);

        return element;
    }

    public ElementCircuit GetElement(Vector3 positionMonde)
    {
        (Vector2 point1, Vector2 point2) = GetArete(positionMonde);

        return GetElement(point1, point2);
    }

    public void SetElement(Vector2 point1, Vector2 point2, ElementCircuit element)
    {
        if (element != null)
        {
            RetirerElement(element);
        }

        if (EstDedans(point1) && EstDedans(point2))
        {
            matriceElements[(point1, point2)] = element;
            matriceElements[(point2, point1)] = element;

            if (element != null)
            {
                element.point1 = point1;
                element.point2 = point2;
                SetValeur(point1, point2, 56);
            }
        }
    }

    public void SetElement(Vector3 positionMonde, ElementCircuit element)
    {
        (Vector2 point1, Vector2 point2) = GetArete(positionMonde);
        
        SetElement(point1, point2, element);
    }

    public void RetirerElement(ElementCircuit element)
    {
        if (element != null && GetElement(element.point1, element.point2) == element)
        {
            SetElement(element.point1, element.point2, null);
            SetValeur(element.point1, element.point2, 0);
        }
    }

    public int GetValeur(Vector2 point1, Vector2 point2)
    {
        int valeur;
        matriceValeurs.TryGetValue((point1, point2), out valeur);

        return valeur;
    }

    public int GetValeur(Vector3 positionMonde)
    {
        (Vector2 point1, Vector2 point2) = GetArete(positionMonde);

        return GetValeur(point1, point2);
    }

    public void SetValeur(Vector2 point1, Vector2 point2, int valeur)
    {
        if (EstDedans(point1) && EstDedans(point2))
        {
            matriceValeurs[(point1, point2)] = valeur;
            matriceValeurs[(point2, point1)] = valeur;
        
            if (modeDebug)
            {
                if (debugTextArray.ContainsKey((point1, point2)))
                {
                    debugTextArray[(point1, point2)].text = valeur.ToString();
                }
                else if (debugTextArray.ContainsKey((point2, point1))) 
                {
                    debugTextArray[(point2, point1)].text = valeur.ToString();
                }
            }
        }
    }

    public void SetValeur(Vector3 positionMonde, int valeur)
    {
        (Vector2 point1, Vector2 point2) = GetArete(positionMonde);

        SetValeur(point1, point2, valeur);
    }

    public (Vector2 point1, Vector2 point2) GetArete(Vector3 positionMonde)
    {
        Vector3 positionGrilleAlignee = GetPositionGrilleAlignee(positionMonde);
        Vector3 positionMondeAlignee = GetPositionMondeAlignee(positionMonde);

        Vector2 point1 = new Vector2(positionGrilleAlignee.x, positionGrilleAlignee.y);
        Vector2 point2;

        if (Math.Abs(positionMonde.x - positionMondeAlignee.x) > Math.Abs(positionMonde.y - positionMondeAlignee.y))
        {
            if (positionMonde.x > positionMondeAlignee.x && point1.x < colonnes - 1 || point1.x == 0)
            {
                point2 = new Vector2(point1.x + 1, point1.y);
            }
            else 
            {
                point2 = new Vector2(point1.x - 1, point1.y);
            }
        }
        else
        {
            if (positionMonde.y > positionMondeAlignee.y && point1.y < lignes - 1 || point1.y == 0)
            {
                point2 = new Vector2(point1.x, point1.y + 1);
            }
            else 
            {
                point2 = new Vector2(point1.x, point1.y - 1);
            }
        }

        return (point1, point2);
    }

    public Vector3 GetPositionGrille(Vector3 positionMonde)
    {
        return new Vector3(positionMonde.x - origineX, positionMonde.y - origineY);
    }
    
    public Vector3 GetPositionGrilleAlignee(Vector3 positionMonde)
    {
        int x = Mathf.RoundToInt((positionMonde.x - origineX) / tailleCelluleX);
        int y = Mathf.RoundToInt((positionMonde.y - origineY) / tailleCelluleY);

        return new Vector3(x, y, 0);
    }

    public Vector3 GetPositionMonde(float x, float y)
    {
        return new Vector3(x * tailleCelluleX + origineX, y * tailleCelluleY + origineY);
    }

    public Vector3 GetPositionMondeAlignee(Vector3 positionMonde)
    {
        return GetPositionGrilleAlignee(positionMonde) + new Vector3(origineX, origineY);
    }

    private Vector3 GetPositionMondeMoitie(int i, int j)
    {
        return new Vector3(i * tailleCelluleX + 0.5f + origineX, j * tailleCelluleY + 0.5f + origineY);
    }

    public bool EstDedans(Vector2 point)
    {
        return point.x >= 0 && point.x < colonnes && point.y >= 0 && point.y < lignes;
    }
}
