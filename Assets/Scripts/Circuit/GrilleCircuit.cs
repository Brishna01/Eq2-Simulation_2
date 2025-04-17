using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float origineGrilleX { get; private set; }
    public float origineGrilleY { get; private set; }

    private ElementCircuit[,] matriceElements;

    void Awake()
    {
        matriceElements = new ElementCircuit[colonnes, lignes];

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ElementCircuit GetElement(int x, int y)
    {
        return matriceElements[x, y];
    }

    public void SetElement(int x, int y, ElementCircuit element)
    {
        matriceElements[x, y] = element;

        if (element != null)
        {
            if (element.positionGrilleX > 0 && element.positionGrilleY > 0)
            {
                SetElement(element.positionGrilleX, element.positionGrilleY, null);
            }

            element.positionGrilleX = x;
            element.positionGrilleY = y;
        }
    }

    public Vector3 GetPositionGrille(Vector3 positionMonde)
    {
        int x = Mathf.FloorToInt((positionMonde.x - origineGrilleX) / tailleCelluleX);
        int y = Mathf.FloorToInt((positionMonde.y - origineGrilleY) / tailleCelluleY);

        return new Vector3(x, y, 0);
    }
}
