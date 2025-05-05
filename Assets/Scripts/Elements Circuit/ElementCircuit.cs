using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

/// <summary>
/// Représente un élément de circuit.
/// </summary>
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

    /// <summary>
    /// Ajoute la contribution de l'élément à la matrice et vecteur de connus
    /// de l'analyse nodale modifiée.
    /// </summary>
    /// <param name="matrice">la matrice</param>
    /// <param name="vecteurConnus">le vecteur de variables connus</param>
    /// <param name="noeud1">le noeud à la première borne</param>
    /// <param name="noeud2">le noeud à la deuxième borne</param>
    /// <param name="nombreNoeuds">le nombre de noeuds dans le circuit</param>
    /// <param name="numeroSourceTension">le numéro de la source de tension</param>
    public virtual void ContribuerCircuit(Matrix<double> matrice, Vector<double> vecteurConnus, int noeud1, int noeud2, 
        int nombreNoeuds, int numeroSourceTension)
    {
        
    }

    /// <summary>
    /// Met à jour les paramètres de l'élément selon les résultats de l'analyse
    /// nodale modifiée.
    /// </summary>
    /// <param name="vecteurInconnus">le vecteur de variables inconnus</param>
    /// <param name="noeud1">le noeud à la première borne</param>
    /// <param name="noeud2">le noeud à la deuxième borne</param>
    /// <param name="nombreNoeuds">le nombre de noeuds dans le circuit</param>
    /// <param name="numeroSourceTension">le numéro de la source de tension</param>
    public virtual void MettreAJourParametres(Vector<double> vecteurInconnus, int noeud1, int noeud2, int nombreNoeuds, 
        int numeroSourceTension)
    {

    }
}
