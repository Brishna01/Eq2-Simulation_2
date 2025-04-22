using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementCircuit : MonoBehaviour
{
    [field: SerializeField]
    public virtual double tension { get; set; }
    [field: SerializeField]
    public virtual double resistance { get; set; }
    public virtual double intensite { get; set; }
    public abstract double puissance { get; set; }

    public Vector2Int point1 { get; set; }
    public Vector2Int point2 { get; set; }
    public bool estDansGrille { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double TrouverResistanceEQ(GameObject[,] gridArrayObjet)
    {
        double[] resistances = new double[gridArrayObjet.GetLength(0)];

        for (int laColonne = 0; laColonne < gridArrayObjet.GetLength(0); laColonne++)
        {
            for (int j = 0; j < gridArrayObjet.GetLength(1); j++)
            {
            }
        }
        return 0;
    }

    public double TrouverTensionEQ()
    {

        return 0;
    }

    public double TrouverIntensiteEQ()
    {

        return 0;
    }

    public double TrouverPuissanceEQ()
    {

        return 0;
    }
}
