using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;

/// <summary>
/// Utilise la méthode d'analyse nodale modifiée.
/// 
/// Basé sur : https://spicesharp.github.io/SpiceSharp/articles/custom_components/modified_nodal_analysis.html
/// </summary>
public class SimulationCircuit
{
    private Dictionary<Vector2Int, int> dictionnaireNoeuds = new Dictionary<Vector2Int, int>();
    private int nombreNoeuds;

    public Matrix<double> matrice { get; private set; }
    public Vector<double> vecteurConnus { get; private set; }
    public Vector<double> vecteurInconnus { get; private set; }

    private GrilleCircuit grilleCircuit;

    public SimulationCircuit(GrilleCircuit grilleCircuit)
    {
        this.grilleCircuit = grilleCircuit;
    }

    public Vector<double> Resoudre()
    {
        if (vecteurInconnus != null)
        {
            return vecteurInconnus;
        }

        List<ElementCircuit> elementsCircuit = grilleCircuit.GetElements();
        int nombreSourcesTension = GetNombreSourcesTension(elementsCircuit);

        IdentifierNoeuds(elementsCircuit);
        
        matrice = Matrix<double>.Build.Dense(nombreNoeuds + nombreSourcesTension, nombreNoeuds + nombreSourcesTension);
        vecteurConnus = Vector<double>.Build.Dense(nombreNoeuds + nombreSourcesTension);

        int numeroSourceTension = 0;
        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            int noeud1 = dictionnaireNoeuds[elementCircuit.point1];
            int noeud2 = dictionnaireNoeuds[elementCircuit.point2];

            elementCircuit.ContribuerCircuit(matrice, vecteurConnus, noeud1, noeud2, nombreNoeuds, numeroSourceTension);

            if (elementCircuit is SourceTension)
            {
                numeroSourceTension++;
            }
        }

        vecteurInconnus = matrice.Solve(vecteurConnus);

        return vecteurInconnus;
    }

    private void IdentifierNoeuds(List<ElementCircuit> elementsCircuit)
    {
        dictionnaireNoeuds.Clear();

        int numeroNoeud = 0;
        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            if (!dictionnaireNoeuds.ContainsKey(elementCircuit.point1))
            {
                PropagerNoeud(elementCircuit.point1, elementCircuit.point1 - elementCircuit.point2, numeroNoeud);
                numeroNoeud++;
            }
            
            if (!dictionnaireNoeuds.ContainsKey(elementCircuit.point2))
            {
                PropagerNoeud(elementCircuit.point2, elementCircuit.point2 - elementCircuit.point1, numeroNoeud);
                numeroNoeud++;
            }
        }

        nombreNoeuds = Math.Max(numeroNoeud - 1, 0);
    }

    private void PropagerNoeud(Vector2Int point, Vector2Int direction, int numeroNoeud)
    {
        while (grilleCircuit.EstDedans(point) && !dictionnaireNoeuds.ContainsKey(point))
        {
            Vector2Int prochainPoint = point + direction;

            dictionnaireNoeuds[point] = numeroNoeud;
           
            if (direction.x != 0)
            {
                if (PeutPropagerNoeudVers(point, new Vector2Int(0, -1)))
                {
                    PropagerNoeud(point + new Vector2Int(0, -1), new Vector2Int(0, -1), numeroNoeud);
                }
                
                if (PeutPropagerNoeudVers(point, new Vector2Int(0, 1)))
                {
                    PropagerNoeud(point + new Vector2Int(0, 1), new Vector2Int(0, 1), numeroNoeud);
                }
            }
            else
            {
                if (PeutPropagerNoeudVers(point, new Vector2Int(-1, 0)))
                {
                    PropagerNoeud(point + new Vector2Int(-1, 0), new Vector2Int(-1, 0), numeroNoeud);
                }
                
                if (PeutPropagerNoeudVers(point, new Vector2Int(1, 0)))
                {
                    PropagerNoeud(point + new Vector2Int(1, 0), new Vector2Int(1, 0), numeroNoeud);
                }
            }

            if (!PeutPropagerNoeudVers(point, direction))
            {
                break;
            }

            point = prochainPoint;
        }
    }

    private bool PeutPropagerNoeudVers(Vector2Int point, Vector2Int direction)
    {
        Vector2Int prochainPoint = point + direction;

        return grilleCircuit.GetFil(point, prochainPoint) != null 
            && grilleCircuit.GetElement(point, prochainPoint) == null;
    }

    public void Appliquer()
    {
        if (vecteurInconnus == null)
        {
            return;
        }

        List<ElementCircuit> elementsCircuit = grilleCircuit.GetElements();

        int numeroSourceTension = 0;
        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            int noeud1 = dictionnaireNoeuds[elementCircuit.point1];
            int noeud2 = dictionnaireNoeuds[elementCircuit.point2];

            elementCircuit.MettreAJourParametres(vecteurInconnus, noeud1, noeud2, nombreNoeuds, numeroSourceTension);

            if (elementCircuit is SourceTension)
            {
                numeroSourceTension++;
            }
        }
    }

    public void AfficherMatrices()
    {
        if (vecteurInconnus == null)
        {
            return;
        }

        string s = "Simulation circuit\n";

        for (int i = 0; i < matrice.RowCount; i++)
        {
            for (int j = 0; j < matrice.ColumnCount; j++)
            {
                s += matrice[i, j] + "\t";
            }

            s += " | " +  vecteurInconnus[i] + "\t | " + vecteurConnus[i] + "\n"; 
        }

        Debug.Log(s);
    }

    public void AfficherNoeuds()
    {
        for (int y = 0; y <= grilleCircuit.nombreCellules.y; y++)
        {
            for (int x = 0; x <= grilleCircuit.nombreCellules.x; x++)
            {
                Vector2Int point = new Vector2Int(x, y);
                int noeud = GetNoeud(point);

                if (noeud != -1)
                {
                    grilleCircuit.SetTexteDebug(point, noeud.ToString());
                }
                else
                {
                    grilleCircuit.SetTexteDebug(point, "");
                } 
            }
        }
    }

    public int GetNoeud(Vector2Int point)
    {
        int noeud = -1;

        if (dictionnaireNoeuds.ContainsKey(point))
        {
            noeud = dictionnaireNoeuds[point];
        }

        return noeud;
    }

    private int GetNombreSourcesTension(List<ElementCircuit> elementsCircuit)
    {
        int nombreSourcesTension = 0;

        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            if (elementCircuit is SourceTension)
            {
                nombreSourcesTension++;
            }
        }

        return nombreSourcesTension;
    }
}
