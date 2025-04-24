using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GrilleCircuit : MonoBehaviour
{
    [field: SerializeField]
    public Vector2Int nombreCellules { get; private set; }
    [field: SerializeField]
    public Vector2 tailleCellule { get; private set; }
    [SerializeField]
    private GameObject prefabLigneGrille;
    [SerializeField]
    private Color couleurGrille;

    public Vector2 origine { get; private set; }

    private Dictionary<(Vector2Int, Vector2Int), ElementCircuit> matriceElements;
    private Dictionary<(Vector2Int, Vector2Int), FilElectrique> matriceFils;
    private Dictionary<Vector2Int, TextMeshPro> textesDebug;

    void Awake()
    {
        matriceElements = new Dictionary<(Vector2Int, Vector2Int), ElementCircuit>();
        matriceFils = new Dictionary<(Vector2Int, Vector2Int), FilElectrique>();
        textesDebug = new Dictionary<Vector2Int, TextMeshPro>();

        origine = -0.5f * (Vector2)nombreCellules * tailleCellule;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int ligne = 0; ligne <= nombreCellules.y; ligne++)
        {
            GameObject ligneGrille = CreerLigneGrille();
            LineRenderer afficheurLigne = ligneGrille.GetComponent<LineRenderer>();
            
            Vector3[] positions = {GetPositionMonde(0, ligne), GetPositionMonde(nombreCellules.x, ligne)};
            afficheurLigne.SetPositions(positions);
        }

        for (int colonne = 0; colonne <= nombreCellules.x; colonne++)
        {
            GameObject ligneGrille = CreerLigneGrille();
            LineRenderer afficheurLigne = ligneGrille.GetComponent<LineRenderer>();
            
            Vector3[] positions = {GetPositionMonde(colonne, 0), GetPositionMonde(colonne, nombreCellules.y)};
            afficheurLigne.SetPositions(positions);
        }

        GameObject conteneurDebug = gameObject.transform.Find("Debug").gameObject;
        for (int ligne = 0; ligne <= nombreCellules.y; ligne++)
        {
            for (int colonne = 0; colonne <= nombreCellules.x; colonne++)
            {
                GameObject objetDebug = new GameObject("TexteDebug");
                objetDebug.transform.position = GetPositionMonde(colonne, ligne);
                objetDebug.transform.parent = conteneurDebug.transform;

                TextMeshPro texte = objetDebug.AddComponent<TextMeshPro>();
                texte.fontSize = 20;
                texte.alignment = TextAlignmentOptions.Center;
                texte.sortingLayerID = SortingLayer.NameToID("Layer 3");

                RectTransform rect = objetDebug.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1, 1, 1);

                textesDebug[new Vector2Int(colonne, ligne)] = texte;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreerLigneGrille()
    {
        GameObject ligneGrille = Instantiate(prefabLigneGrille);
        ligneGrille.transform.parent = transform.Find("Grillage").transform;

        LineRenderer afficheurLigne = ligneGrille.GetComponent<LineRenderer>();
        afficheurLigne.startColor = couleurGrille;
        afficheurLigne.endColor = couleurGrille;

        return ligneGrille;
    }

    public ElementCircuit GetElement(Vector2Int point1, Vector2Int point2)
    {
        ElementCircuit element;
        matriceElements.TryGetValue((point1, point2), out element);

        return element;
    }

    public ElementCircuit GetElement(Vector3 positionMonde)
    {
        (Vector2Int point1, Vector2Int point2) = GetArete(positionMonde);

        return GetElement(point1, point2);
    }

    /// <summary>
    ///     Inspiré de : https://stackoverflow.com/a/1462128
    /// </summary>
    /// <returns></returns>
    public List<ElementCircuit> GetElements()
    {
        List<ElementCircuit> listeElements = new List<ElementCircuit>();
        HashSet<ElementCircuit> elementsTrouves = new HashSet<ElementCircuit>();

        foreach (ElementCircuit elementCircuit in matriceElements.Values)
        {
            if (elementsTrouves.Add(elementCircuit))
            {
                listeElements.Add(elementCircuit);
            }
        }

        return listeElements;
    }

    public void SetElement(Vector2Int point1, Vector2Int point2, ElementCircuit element)
    {
        if (element != null && element.estDansGrille)
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
                element.estDansGrille = true;
            }
        }
    }

    public void SetElement(Vector3 positionMonde, ElementCircuit element)
    {
        (Vector2Int point1, Vector2Int point2) = GetArete(positionMonde);
        
        SetElement(point1, point2, element);
    }

    public void RetirerElement(ElementCircuit element)
    {
        if (element != null && element.estDansGrille && GetElement(element.point1, element.point2) == element)
        {
            matriceElements.Remove((element.point1, element.point2));
            matriceElements.Remove((element.point2, element.point1));
            element.estDansGrille = false;
        }
    }

    public FilElectrique GetFil(Vector2Int point1, Vector2Int point2)
    {
        FilElectrique fil;
        matriceFils.TryGetValue((point1, point2), out fil);

        return fil;
    }

    public void SetFil(Vector2Int point1, Vector2Int point2, FilElectrique fil)
    {
        if (fil != null && fil.estDansGrille)
        {
            RetirerElement(fil);
        }

        if (EstDedans(point1) && EstDedans(point2))
        {
            matriceFils[(point1, point2)] = fil;
            matriceFils[(point2, point1)] = fil;

            if (fil != null)
            {
                fil.point1 = point1;
                fil.point2 = point2;
                fil.estDansGrille = true;
            }
        }
    }

    public void RetirerFil(FilElectrique fil)
    {
        if (fil != null && fil.estDansGrille && GetFil(fil.point1, fil.point2) == fil)
        {
            matriceFils.Remove((fil.point1, fil.point2));
            matriceFils.Remove((fil.point2, fil.point1));
            fil.estDansGrille = false;
        }
    }

    public void SetTexteDebug(Vector2Int point, string texte)
    {
        if (EstDedans(point))
        {
            textesDebug[point].text = texte;
        }
    }

    public Vector2Int GetPoint(Vector3 positionMonde)
    {
        Vector3 positionGrilleAlignee = GetPositionGrilleAlignee(positionMonde);

        return new Vector2Int(
            Math.Clamp((int)positionGrilleAlignee.x, 0, nombreCellules.x), 
            Math.Clamp((int)positionGrilleAlignee.y, 0, nombreCellules.y)
        );
    }

    public (Vector2Int point1, Vector2Int point2) GetArete(Vector3 positionMonde)
    {
        Vector3 positionMondeAlignee = GetPositionMondeAlignee(positionMonde);

        Vector2Int point1 = GetPoint(positionMonde);
        Vector2Int point2;

        if (Math.Abs(positionMonde.x - positionMondeAlignee.x) > Math.Abs(positionMonde.y - positionMondeAlignee.y))
        {
            if (positionMonde.x > positionMondeAlignee.x && point1.x < nombreCellules.x + 1 || point1.x == 0)
            {
                point2 = new Vector2Int(point1.x + 1, point1.y);
            }
            else 
            {
                point2 = new Vector2Int(point1.x - 1, point1.y);
            }
        }
        else
        {
            if (positionMonde.y > positionMondeAlignee.y && point1.y < nombreCellules.y + 1 || point1.y == 0)
            {
                point2 = new Vector2Int(point1.x, point1.y + 1);
            }
            else 
            {
                point2 = new Vector2Int(point1.x, point1.y - 1);
            }
        }

        return (point1, point2);
    }

    public Vector3 GetPositionGrille(Vector3 positionMonde)
    {
        return new Vector3((positionMonde.x - origine.x) / tailleCellule.x, (positionMonde.y - origine.y) / tailleCellule.y);
    }
    
    public Vector3 GetPositionGrilleAlignee(Vector3 positionMonde)
    {
        int x = Mathf.RoundToInt((positionMonde.x - origine.x) / tailleCellule.x);
        int y = Mathf.RoundToInt((positionMonde.y - origine.y) / tailleCellule.y);

        return new Vector3(x, y);
    }

    public Vector3 GetPositionMonde(float x, float y)
    {
        return new Vector3(x * tailleCellule.x + origine.x, y * tailleCellule.y + origine.y);
    }

    public Vector3 GetPositionMondeAlignee(Vector3 positionMonde)
    {
        float x = Mathf.RoundToInt((positionMonde.x - origine.x) / tailleCellule.x) * tailleCellule.x + origine.x;
        float y = Mathf.RoundToInt((positionMonde.y - origine.y) / tailleCellule.y) * tailleCellule.y + origine.y;

        return new Vector3(x, y);
    }

    public bool EstDedans(Vector2Int point)
    {
        return point.x >= 0 && point.x < nombreCellules.x + 1
            && point.y >= 0 && point.y < nombreCellules.y + 1;
    }

    public bool EstDedans(Vector3 positionMonde)
    {
        return positionMonde.x >= origine.x && positionMonde.x < origine.x + nombreCellules.x * tailleCellule.x 
            && positionMonde.y >= origine.y && positionMonde.y < origine.y + nombreCellules.y * tailleCellule.y;
    }

    public bool EstHorizontal(Vector2Int point1, Vector2Int point2)
    {
        return point1.y == point2.y;
    }

    public bool EstVertical(Vector2Int point1, Vector2Int point2)
    {
        return point1.x == point2.x;
    }

    public bool SontAdjacents(Vector2Int point1, Vector2Int point2)
    {
        return EstHorizontal(point1, point2) && Math.Abs(point2.x - point1.x) <= 1
            || EstVertical(point1, point2) && Math.Abs(point2.y - point1.y) <= 1;
    }

    public Vector3 GetPositionMondeArete(Vector2Int point1, Vector2Int point2)
    {
        return GetPositionMonde((float)(point1.x + point2.x) / 2, (float)(point1.y + point2.y) / 2);
    }

    public bool EstAssezProcheArete(Vector2Int point1, Vector2Int point2, Vector3 positionMonde, float distanceMaximale)
    {
        Vector3 positionGrille = GetPositionGrille(positionMonde);
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);

        float distance;

        if (point1.x == point2.x)
        {
            distance = Math.Abs(positionMonde.x - positionMondeArete.x);
        }
        else if (point1.y == point2.y)
        {
            distance = Math.Abs(positionMonde.y - positionMondeArete.y);
        }
        else
        {
            distance = (positionMonde - positionMondeArete).magnitude;
        }

        return distance <= distanceMaximale
            && (point1.x == point2.x 
                && positionGrille.y >= Math.Min(point1.y, point2.y) && positionGrille.y <= Math.Max(point1.y, point2.y)
            || point1.y == point2.y 
                && positionGrille.x >= Math.Min(point1.x, point2.x) && positionGrille.x <= Math.Max(point1.x, point2.x));
    }
}
