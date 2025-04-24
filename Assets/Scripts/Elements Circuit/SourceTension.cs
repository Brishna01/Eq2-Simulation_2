using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using UnityEngine;

public class SourceTension : ElementCircuit
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
        vecteurConnus[nombreNoeuds + numeroSourceTension] = tension;
        
        if (noeud1 != 0)
        {
            matrice[noeud1, nombreNoeuds + numeroSourceTension] = 1;
            matrice[nombreNoeuds + numeroSourceTension, noeud1] = 1;
        }

        if (noeud2 != 0)
        {
            matrice[noeud2, nombreNoeuds + numeroSourceTension] = -1;
            matrice[nombreNoeuds + numeroSourceTension, noeud2] = -1;
        }
    }

    public override void MettreAJourParametres(Vector<double> vecteurInconnus, int noeud1, int noeud2, int nombreNoeuds, 
        int numeroSourceTension)
    {
        intensite = Math.Abs(vecteurInconnus[nombreNoeuds + numeroSourceTension]);
        puissance = tension * intensite;
    }
}
