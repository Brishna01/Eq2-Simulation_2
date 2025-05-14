using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Contrôle l'utilisation et le changement d'outils pour intéragir avec les
/// éléments du circuit.
/// </summary>
public class ControleurOutils : MonoBehaviour
{
    private static float DISTANCE_DETECTION_POINT = 0.5f;
    private static float DISTANCE_DETECTION_ARETE = 0.1f;

    [SerializeField]
    private GameObject conteneurFilsElectriques;
    [SerializeField]
    private GameObject prefabFilElectrique;

    public Outil outilSelectionne { get; private set; } = Outil.TracerFils;
    public bool boutonGaucheEnfonce { get; set; }
    private Vector2Int? pointPrecedent;

    private GrilleCircuit grilleCircuit;
    private SystemePlacement systemePlacement;
    private new Camera camera;
    private EventSystem systemeEvenements;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = GameObject.Find("Circuit").GetComponent<GrilleCircuit>();
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        systemeEvenements = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        systemePlacement.onPlacementCommence += (objet, modeDragEtDrop) => outilSelectionne = Outil.DeplacerElements;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Q))
        {
            outilSelectionne = Outil.TracerFils;
            systemePlacement.ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            outilSelectionne = Outil.DeplacerElements;
            systemePlacement.ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            outilSelectionne = Outil.SupprimerFils;
            systemePlacement.ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            outilSelectionne = Outil.SupprimerElements;
            systemePlacement.ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            grilleCircuit.SetDebogageVisible(!grilleCircuit.GetDebogageVisible());
        }

        if (Input.GetMouseButtonDown(0))
        {
            boutonGaucheEnfonce = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            boutonGaucheEnfonce = false;
        }

        if (!systemePlacement.estActif)
        {
            if (outilSelectionne == Outil.TracerFils)
            {
                UpdateTracerFils();
            }
            else if (outilSelectionne == Outil.SupprimerFils)
            {
                UpdateSupprimerFils();
            }
        }
    }

    /// <summary>
    /// Méthode appelée chaque frame lorsque l'outil Tracer Fils est actif.
    /// Lorsque le bouton gauche de la souris est maintenu, le point ciblé et le 
    /// point précédent sont utilisés pour créer un fil s'ils sont adjacents.
    /// </summary>
    private void UpdateTracerFils()
    {
        if (boutonGaucheEnfonce)
        {
            Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
            positionMonde.z = 0;

            Vector2Int point = grilleCircuit.GetPoint(positionMonde);
            Vector3 positionMondePoint = grilleCircuit.GetPositionMonde(point.x, point.y);
            
            if ((positionMonde - positionMondePoint).magnitude < DISTANCE_DETECTION_POINT)
            {
                if (pointPrecedent == null && Input.GetMouseButtonDown(0))
                {
                    // Il s'agit du premier point
                    pointPrecedent = point;
                }
                else if (pointPrecedent != null && point != pointPrecedent
                    && grilleCircuit.SontAdjacents(point, (Vector2Int)pointPrecedent))
                {
                    if (!grilleCircuit.GetFil(point, (Vector2Int)pointPrecedent))
                    {
                        // Créer le fil électrique
                        GameObject filElectrique = Instantiate(prefabFilElectrique);
                        LineRenderer afficheurLigne = filElectrique.GetComponent<LineRenderer>();

                        Vector3[] positions = {
                            grilleCircuit.GetPositionMonde(((Vector2)pointPrecedent).x, ((Vector2)pointPrecedent).y),
                            grilleCircuit.GetPositionMonde(point.x, point.y)
                        };
                        afficheurLigne.SetPositions(positions);

                        filElectrique.transform.parent = conteneurFilsElectriques.transform;
                        grilleCircuit.SetFil(point, (Vector2Int)pointPrecedent, filElectrique.GetComponent<FilElectrique>());
                    }

                    pointPrecedent = point;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pointPrecedent = null;
        }
    }

    /// <summary>
    /// Méthode appelée chaque frame lorsque l'outil Supprimer Fils est actif.
    /// </summary>
    private void UpdateSupprimerFils()
    {
        if (boutonGaucheEnfonce)
        {
            Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
            (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(positionMonde);

            if (grilleCircuit.EstAssezProcheArete(point1, point2, positionMonde, DISTANCE_DETECTION_ARETE))
            {
                FilElectrique filElectrique = grilleCircuit.GetFil(point1, point2);

                if (filElectrique != null)
                {
                    // Supprimer le fil électrique
                    grilleCircuit.RetirerFil(filElectrique);
                    Destroy(filElectrique.gameObject);
                }
            }
        }
    }
}
