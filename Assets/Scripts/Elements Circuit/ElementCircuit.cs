using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementCircuit : MonoBehaviour
{
    // public int colonnes { get; set; }
    public abstract double tension { get; set; }
    public abstract double resistance { get; set; }
    public abstract double intensite { get; set; }
    public abstract double puissance { get; set; }

    private double tensionEQ ;
    private double resistanceEQ ;
    private double intensiteEq;
    private double puissanceEQ;

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
        double[] resitances = new double[gridArrayObjet.GetLength(0)];

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
