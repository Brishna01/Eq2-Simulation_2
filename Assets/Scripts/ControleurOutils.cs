using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControleurOutils : MonoBehaviour
{
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

    public CanvasGroup OptionPanel;
    public void BtnQ()
    {
        outilSelectionne = Outil.TracerFils;
        systemePlacement.ArreterPlacement();
    }

    public void BtnW()
    {
        outilSelectionne = Outil.DeplacerElements;
        systemePlacement.ArreterPlacement();
    }

    public void BtnE()
    {
        outilSelectionne = Outil.SupprimerFils;
        systemePlacement.ArreterPlacement();
    }

    public void BtnR()
    {
        outilSelectionne = Outil.SupprimerElements;
        systemePlacement.ArreterPlacement();
    }

    private void UpdateTracerFils()
    {
        if (boutonGaucheEnfonce)
        {
            Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
            positionMonde.z = 0;

            Vector2Int point = grilleCircuit.GetPoint(positionMonde);
            Vector3 positionMondePoint = grilleCircuit.GetPositionMonde(point.x, point.y);

            if ((positionMonde - positionMondePoint).magnitude < 0.5)
            {
                if (pointPrecedent == null && Input.GetMouseButtonDown(0))
                {
                    pointPrecedent = point;
                }
                else if (pointPrecedent != null && point != pointPrecedent
                    && grilleCircuit.SontAdjacents(point, (Vector2Int)pointPrecedent))
                {
                    if (!grilleCircuit.GetFil(point, (Vector2Int)pointPrecedent))
                    {
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

    private void UpdateSupprimerFils()
    {
        if (boutonGaucheEnfonce)
        {
            Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
            positionMonde.z = 0;

            (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(positionMonde);

            if (grilleCircuit.EstAssezProcheArete(point1, point2, positionMonde, 0.1f))
            {
                FilElectrique filElectrique = grilleCircuit.GetFil(point1, point2);

                if (filElectrique != null)
                {
                    grilleCircuit.RetirerFil(filElectrique);
                    Destroy(filElectrique.gameObject);
                }
            }
        }
    }
}
