using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjet : MonoBehaviour
{
    // public int colonnes { get; set; }
    private double tension, resistance, intesite, puissance;
    private double tensionEQ ;
    private double resistanceEQ ;
    private double intesiteEQ;
    private double puissanceEQ;



    public GameObjet(double tension, double resistance, double intesite, double puissance)
    {
        this.tension = tension;
        this.resistance = resistance;
        this.intesite = intesite;
        this.puissance = puissance;
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

    public double TrouverPuissnceEQ()
    {

        return 0;
    }
}
