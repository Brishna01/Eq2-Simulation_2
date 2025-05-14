using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;

/// <summary>
/// Utilise la méthode d'analyse nodale modifiée.
/// 
/// Basé sur : https://spicesharp.github.io/SpiceSharp/articles/custom_components/modified_nodal_analysis.html
/// et https://lpsa.swarthmore.edu/Systems/Electrical/mna/MNA3.html
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

    /// <summary>
    /// Résoud le circuit à l'aide de l'analyse nodale modifiée.
    /// </summary>
    /// <returns>le vecteur d'inconnus</returns>
    public Vector<double> Resoudre()
    {
        if (vecteurInconnus != null)
        {
            return vecteurInconnus;
        }

        List<ElementCircuit> elementsCircuit = grilleCircuit.GetElements();
        // Utile pour connaître la taille des matrices
        int nombreSourcesTension = GetNombreSourcesTension(elementsCircuit); 

        IdentifierNoeuds(elementsCircuit);
        
        // Créer les matrices
        matrice = Matrix<double>.Build.Dense(nombreNoeuds + nombreSourcesTension, nombreNoeuds + nombreSourcesTension);
        vecteurConnus = Vector<double>.Build.Dense(nombreNoeuds + nombreSourcesTension);

        // Permettre chaque élément de modifier les matrices
        int numeroSourceTension = 0;
        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            int noeud1 = GetNoeud(elementCircuit.point1);
            int noeud2 = GetNoeud(elementCircuit.point2);

            if (noeud1 == -1 || noeud2 == -1)
            {
                continue;
            }

            elementCircuit.ContribuerCircuit(matrice, vecteurConnus, noeud1, noeud2, nombreNoeuds, numeroSourceTension);

            if (elementCircuit is SourceTension)
            {
                numeroSourceTension++;
            }
        }

        // Résoudre le système d'équations
        vecteurInconnus = matrice.Solve(vecteurConnus);

        return vecteurInconnus;
    }

    /// <summary>
    /// Identifie tous les noeuds du circuit en associant les points à un noeud.
    /// Pour chaque borne d'un élément de circuit, un numéro de noeud est généré
    /// et est propagé sur les fils électriques jusqu'à ce qu'une autre borne
    /// soit trouvée.
    /// </summary>
    /// <param name="elementsCircuit">la liste d'éléments du circuit</param>
    private void IdentifierNoeuds(List<ElementCircuit> elementsCircuit)
    {
        dictionnaireNoeuds.Clear();

        int numeroNoeud = 0;
        foreach (ElementCircuit elementCircuit in elementsCircuit)
        {
            if (!dictionnaireNoeuds.ContainsKey(elementCircuit.point1) && EstValidePourNoeud(elementCircuit.point1))
            {
                PropagerNoeud(elementCircuit.point1, elementCircuit.point1 - elementCircuit.point2, numeroNoeud);
                numeroNoeud++;
            }
            
            if (!dictionnaireNoeuds.ContainsKey(elementCircuit.point2) && EstValidePourNoeud(elementCircuit.point2))
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
                // La direction est présentement horizontale, on essaie de propager verticalement
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
                // La direction est présentement verticale, on essaie de propager horizontalement
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

    /// <summary>
    /// Retourne si on peut propager un noeud dans la direction donnée. On peut
    /// s'il y a un fil et pas d'élément.
    /// </summary>
    /// <param name="point">le point de départ</param>
    /// <param name="direction">la direction</param>
    /// <returns>si on peut propager dans la direction</returns>
    private bool PeutPropagerNoeudVers(Vector2Int point, Vector2Int direction)
    {
        Vector2Int prochainPoint = point + direction;

        return grilleCircuit.GetFil(point, prochainPoint) != null 
            && grilleCircuit.GetElement(point, prochainPoint) == null;
    }

    /// <summary>
    /// Retourne si le point peut faire partie d'un noeud. C'est le cas s'il y
    /// a un fil libre dans au moins une direction.
    /// </summary>
    /// <param name="point">le point à vérifier</param>
    /// <returns>si le point est valide</returns>
    private bool EstValidePourNoeud(Vector2Int point)
    {
        return PeutPropagerNoeudVers(point, new Vector2Int(0, -1))
            || PeutPropagerNoeudVers(point, new Vector2Int(0, 1))
            || PeutPropagerNoeudVers(point, new Vector2Int(-1, 0))
            || PeutPropagerNoeudVers(point, new Vector2Int(1, 0));
    }

    /// <summary>
    /// Applique le résultat de l'analyse nodale modifiée sur les éléments du
    /// circuit.
    /// </summary>
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
            int noeud1 = GetNoeud(elementCircuit.point1);
            int noeud2 = GetNoeud(elementCircuit.point2);

            if (noeud1 == -1 || noeud2 == -1)
            {
                continue;
            }

            elementCircuit.MettreAJourParametres(vecteurInconnus, noeud1, noeud2, nombreNoeuds, numeroSourceTension);

            if (elementCircuit is SourceTension)
            {
                numeroSourceTension++;
            }
        }
    }

    /// <summary>
    /// Affiche la matrice et les vecteurs du système d'équations sur la console.
    /// </summary>
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

    /// <summary>
    /// Affiche les numéros de noeud sur les fils du circuit en utilisant les
    /// textes de débogage.
    /// </summary>
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
                    grilleCircuit.SetTexteDebogage(point, noeud.ToString());
                }
                else
                {
                    grilleCircuit.SetTexteDebogage(point, "");
                } 
            }
        }
    }

    /// <summary>
    /// Retourne le numéro du noeud au point donné ou -1.
    /// </summary>
    /// <param name="point">le point sur la grille</param>
    /// <returns>le numéro du noeud</returns>
    public int GetNoeud(Vector2Int point)
    {
        int noeud = -1;

        if (dictionnaireNoeuds.ContainsKey(point))
        {
            noeud = dictionnaireNoeuds[point];
        }

        return noeud;
    }

    /// <summary>
    /// Retourne le nombre de sources de tension dans le circuit.
    /// </summary>
    /// <param name="elementsCircuit">la liste d'éléments de circuit</param>
    /// <returns>le nombre de sources de tension</returns>
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
