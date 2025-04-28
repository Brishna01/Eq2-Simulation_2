using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class Resistance : ElementCircuit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ContribuerCircuit(Matrix<double> matrice, Vector<double> vecteurConnus, int noeud1, int noeud2, 
        int nombreNoeuds, int numeroSourceTension)
    {
        double contribution = 1/resistance;

        if (noeud1 != 0)
        {
            matrice[noeud1 - 1, noeud1 - 1] += contribution;
        }

        if (noeud2 != 0)
        {
            matrice[noeud2 - 1, noeud2 - 1] += contribution;
        }

        if (noeud1 != 0 && noeud2 != 0)
        {
            matrice[noeud1 - 1, noeud2 - 1] -= contribution;
            matrice[noeud2 - 1, noeud1 - 1] -= contribution;
        }
    }

    public override void MettreAJourParametres(Vector<double> vecteurInconnus, int noeud1, int noeud2, int nombreNoeuds, 
        int numeroSourceTension)
    {
        double tensionNoeud1 = noeud1 != 0 ? vecteurInconnus[noeud1 - 1] : 0;
        double tensionNoeud2 = noeud2 != 0 ? vecteurInconnus[noeud2 - 1] : 0;

        tension = Math.Abs(tensionNoeud1 - tensionNoeud2);
        intensite = tension / resistance;
        puissance = resistance * Math.Pow(intensite, 2);
    }
}
