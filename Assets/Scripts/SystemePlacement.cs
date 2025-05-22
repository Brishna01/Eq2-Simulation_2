using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Permet de placer et de déplacer des objets sur la grille.
/// </summary>
public class SystemePlacement : MonoBehaviour
{
    private static float DISTANCE_DETECTION_ARETE = 0.3f;

    [SerializeField]
    private GameObject circuit;
    private GrilleCircuit grilleCircuit;
    [SerializeField]
    private GameObject conteneurObjets;

    private GameObject objetAPlacer;
    private Vector3? positionInitiale;
    private bool estClone;
    private bool estModeDragEtDrop;
    public bool estActif { get => objetAPlacer != null; }

    public Action<GameObject, bool> onPlacementCommence;
    public Action<GameObject, bool> onObjetPlace;
    public Action<GameObject, bool> onPlacementArrete;

    private new Camera camera;
    private EventSystem systemeEvenements;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = circuit.GetComponent<GrilleCircuit>();

        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        systemeEvenements = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objetAPlacer != null)
        {
            objetAPlacer.transform.position = CalculerPositionCibleMonde();

            if (estModeDragEtDrop && Input.GetMouseButtonUp(0))
            {
                if (!systemeEvenements.IsPointerOverGameObject())
                {
                    PlacerObjet();
                }
                else
                {
                    ArreterPlacement();
                }
            }
            else if (!estModeDragEtDrop && Input.GetMouseButtonDown(0) && !systemeEvenements.IsPointerOverGameObject())
            {
                PlacerObjet();
            }
        }
    }

    /// <summary>
    /// Commence le placement de l'objet donné.
    /// </summary>
    /// <param name="objet">l'objet à placer</param>
    /// <param name="cloner">si on place plutôt un clone de l'objet</param>
    public void CommencerPlacement(GameObject objet, bool cloner)
    {
        CommencerPlacement(objet, cloner, false);
    }

    /// <summary>
    /// Commence le placement de l'objet donné.
    /// </summary>
    /// <param name="objet">l'objet à placer</param>
    /// <param name="cloner">si on place plutôt un clone de l'objet</param>
    /// <param name="modeDragEtDrop">si on active le mode drag et drop</param>
    public void CommencerPlacement(GameObject objet, bool cloner, bool modeDragEtDrop)
    {
        if (objet == null)
        {
            objetAPlacer = null;
            return;
        }

        estClone = cloner;
        estModeDragEtDrop = modeDragEtDrop;

        if (cloner)
        {
            objetAPlacer = Instantiate(objet, transform);
            objetAPlacer.name = objet.name;
            positionInitiale = null;
        }
        else
        {
            objetAPlacer = objet;
            positionInitiale = objet.transform.position;
        }

        objetAPlacer.transform.position = CalculerPositionCibleMonde();

        if (onPlacementCommence != null)
        {
            onPlacementCommence(objetAPlacer, modeDragEtDrop);
        }
    }

    /// <summary>
    /// Place l'objet pris en charge à sa position actuelle. L'objet est ajouté
    /// à la grille du circuit s'il est à une position valide et retiré de la
    /// grille sinon.
    /// </summary>
    private void PlacerObjet()
    {
        if (objetAPlacer == null)
        {
            return;
        }

        if (conteneurObjets != null)
        {
            objetAPlacer.transform.parent = conteneurObjets.transform;
        }

        ElementCircuit elementCircuit = objetAPlacer.GetComponent<ElementCircuit>();

        if (elementCircuit != null)
        {
            (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(objetAPlacer.transform.position);
            Vector3 positionMondeArete = grilleCircuit.GetPositionMondeArete(point1, point2);

            if ((objetAPlacer.transform.position - positionMondeArete).magnitude < 0.1f)
            {
                // L'objet est à une position de la grille
                ElementCircuit elementExistant = grilleCircuit.GetElement(objetAPlacer.transform.position);

                if (elementExistant != null && elementExistant != elementCircuit)
                {
                    if (positionInitiale != null)
                    {
                        // Échanger les positions des éléments
                        elementExistant.gameObject.transform.position = (Vector3)positionInitiale;
                        grilleCircuit.SetElement((Vector3)positionInitiale, elementExistant);
                    }
                    else
                    {
                        // Remplacer l'élément existant
                        grilleCircuit.RetirerElement(elementExistant);
                        Destroy(elementExistant.gameObject);
                    }
                }

                grilleCircuit.SetElement(objetAPlacer.transform.position, elementCircuit);
            }
            else
            {
                // L'objet n'est pas à une position de la grille
                grilleCircuit.RetirerElement(elementCircuit);
            }
        }

        if (onObjetPlace != null)
        {
            onObjetPlace(objetAPlacer, estModeDragEtDrop);
        }

        objetAPlacer = null;
        positionInitiale = null;
    }

    /// <summary>
    /// Annule le placement d'un objet. Ce dernier est détruit s'il s'agit d'un
    /// clone et retourné à sa position initiale sinon.
    /// </summary>
    public void ArreterPlacement()
    {
        if (objetAPlacer != null && objetAPlacer.transform.parent == transform)
        {
            if (estClone)
            {
                Destroy(objetAPlacer);
            }
            else if (positionInitiale != null)
            {
                objetAPlacer.transform.position = (Vector3)positionInitiale;
            }

            if (onPlacementArrete != null)
            {
                onPlacementArrete(objetAPlacer, estModeDragEtDrop);
            }

            objetAPlacer = null;
            positionInitiale = null;
        }
    }

    /// <summary>
    /// Met fin au placement seulement si l'objet donné est actuellement pris en
    /// charge.
    /// </summary>
    /// <param name="objet">l'objet ciblé</param>
    public void ArreterPlacement(GameObject objet)
    {
        if (objetAPlacer != null && objet == objetAPlacer)
        {
            ArreterPlacement();
        }
    }

    /// <summary>
    /// Retourne la position où mettre l'objet, soit la position de la souris.
    /// Si la souris est proche d'une arête, la position de cette dernière est
    /// utilisée.
    /// </summary>
    /// <returns>la position où mettre l'objet</returns>
    private Vector3 CalculerPositionCibleMonde()
    {
        Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
        positionMonde.z = 0;
        (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(positionMonde);
        
        if (grilleCircuit.EstAssezProcheArete(point1, point2, positionMonde, DISTANCE_DETECTION_ARETE))
        {
            return grilleCircuit.GetPositionMondeArete(point1, point2);
        }

        return positionMonde;
    }
}
