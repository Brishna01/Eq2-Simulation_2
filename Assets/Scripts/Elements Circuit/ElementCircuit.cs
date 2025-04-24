using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public abstract class ElementCircuit : MonoBehaviour
{
    [field: SerializeField]
    public double tension { get; set; }
    [field: SerializeField]
    public double resistance { get; set; }
    public double intensite { get; set; }
    public double puissance { get; set; }

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

    public virtual void ContribuerCircuit(Matrix<double> matrice, Vector<double> vecteurConnus, int noeud1, int noeud2, 
        int nombreNoeuds, int numeroSourceTension)
    {
        
    }

    public virtual void MettreAJourParametres(Vector<double> vecteurInconnus, int noeud1, int noeud2, int nombreNoeuds, 
        int numeroSourceTension)
    {

    }
}
