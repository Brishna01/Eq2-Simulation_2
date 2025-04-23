using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class SystemePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject circuit;
    private GrilleCircuit grilleCircuit;
    [SerializeField]
    private GameObject conteneurObjets;
    [SerializeField]
    private GameObject prefabFilElectrique;

    private GameObject objetAPlacer;
    private Vector3? positionInitiale;
    private bool modeDragEtDrop;

    public Outil outilSelectionne { get; private set; } = Outil.TracerFils;
    private bool boutonGaucheEnfonce;
    private Vector2Int? pointPrecedent;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            outilSelectionne = Outil.TracerFils;
            ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            outilSelectionne = Outil.DeplacerElements;
            ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            outilSelectionne = Outil.SupprimerFils;
            ArreterPlacement();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            outilSelectionne = Outil.SupprimerElements;
            ArreterPlacement();
        }

        if (Input.GetMouseButtonDown(0))
        {
            boutonGaucheEnfonce = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            boutonGaucheEnfonce = false;
        }

        if (objetAPlacer != null)
        {
            UpdatePlacement();
        }
        else if (outilSelectionne == Outil.TracerFils)
        {
            UpdateTracerFils();
        }
        else if (outilSelectionne == Outil.SupprimerFils)
        {
            UpdateSupprimerFils();
        }
        else if (outilSelectionne == Outil.SupprimerElements)
        {
            UpdateSupprimerElements();
        }
    }

    private void UpdatePlacement()
    {
        objetAPlacer.transform.position = CalculerPositionCibleMonde();

        if (modeDragEtDrop && Input.GetMouseButtonUp(0))
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
        else if (!modeDragEtDrop && Input.GetMouseButtonDown(0) && !systemeEvenements.IsPointerOverGameObject())
        {
            PlacerObjet();
        }
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

                        filElectrique.transform.parent = conteneurObjets.transform;
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

            if (EstAssezProcheArete(point1, point2, positionMonde, 0.1f))
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

    private void UpdateSupprimerElements()
    {
        if (boutonGaucheEnfonce)
        {
            Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
            positionMonde.z = 0;

            (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(positionMonde);

            if (EstAssezProcheArete(point1, point2, positionMonde, 0.1f))
            {
                ElementCircuit elementCircuit = grilleCircuit.GetElement(point1, point2);
                if (elementCircuit != null)
                {
                    grilleCircuit.RetirerElement(elementCircuit);
                    Destroy(elementCircuit.gameObject);
                }
            }
        }
    }

    public void CommencerPlacement(GameObject objet, bool cloner)
    {
        CommencerPlacement(objet, cloner, false);
    }

    public void CommencerPlacement(GameObject objet, bool cloner, bool _modeDragEtDrop)
    {
        if (objet == null)
        {
            objetAPlacer = null;
            return;
        }

        modeDragEtDrop = _modeDragEtDrop;
        outilSelectionne = Outil.DeplacerElements;

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
    }

    private void PlacerObjet()
    {
        if (objetAPlacer == null)
        {
            return;
        }

        ElementCircuit elementCircuit = objetAPlacer.GetComponent<ElementCircuit>();

        if (conteneurObjets != null)
        {
            objetAPlacer.transform.parent = conteneurObjets.transform;
        }

        (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(objetAPlacer.transform.position);
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);
        if ((objetAPlacer.transform.position - positionMondeArete).magnitude < 0.1)
        {
            ElementCircuit elementExistant = grilleCircuit.GetElement(objetAPlacer.transform.position);

            if (elementExistant && elementExistant != elementCircuit)
            {
                if (positionInitiale != null)
                {
                    elementExistant.gameObject.transform.position = (Vector3)positionInitiale;
                    grilleCircuit.SetElement((Vector3)positionInitiale, elementExistant);
                }
                else
                {
                    grilleCircuit.RetirerElement(elementExistant);
                    Destroy(elementExistant.gameObject);
                }
            }

            grilleCircuit.SetElement(objetAPlacer.transform.position, elementCircuit);
        }
        else
        {
            grilleCircuit.RetirerElement(elementCircuit);
        }

        GameObject objetPlace = objetAPlacer;
        objetAPlacer = null;

        onObjetPlace(objetPlace, modeDragEtDrop);
    }

    public void ArreterPlacement()
    {
        if (objetAPlacer != null && objetAPlacer.transform.parent == transform)
        {
            Destroy(objetAPlacer);
            onPlacementArrete(objetAPlacer, modeDragEtDrop);
            objetAPlacer = null;
            positionInitiale = null;
        }
    }

    public void ArreterPlacement(GameObject objet)
    {
        if (objetAPlacer != null && objet == objetAPlacer)
        {
            ArreterPlacement();
        }
    }

    private Vector3 CalculerPositionCibleMonde()
    {
        Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
        positionMonde.z = 0;

        (Vector2Int point1, Vector2Int point2) = grilleCircuit.GetArete(positionMonde);
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);

        if (EstAssezProcheArete(point1, point2, positionMonde, 0.3f))
        {
            return positionMondeArete;
        }

        return positionMonde;
    }

    private Vector3 GetPositionMondeArete(Vector2Int point1, Vector2Int point2)
    {
        return grilleCircuit.GetPositionMonde((float)(point1.x + point2.x) / 2, (float)(point1.y + point2.y) / 2);
    }

    private bool EstAssezProcheArete(Vector2Int point1, Vector2Int point2, Vector3 positionMonde, float distanceMaximale)
    {
        Vector3 positionGrille = grilleCircuit.GetPositionGrille(positionMonde);
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);

        float distance;

        if (point1.x == point2.x)
        {
            distance = Math.Abs(positionMonde.x - positionMondeArete.x);
        }
        else if (point1.y == point2.y)
        {
            distance = Math.Abs(positionMonde.y - positionMondeArete.y);
        }
        else
        {
            distance = (positionMonde - positionMondeArete).magnitude;
        }

        return distance <= distanceMaximale
            && (point1.x == point2.x 
                && positionGrille.y >= Math.Min(point1.y, point2.y) && positionGrille.y <= Math.Max(point1.y, point2.y)
            || point1.y == point2.y 
                && positionGrille.x >= Math.Min(point1.x, point2.x) && positionGrille.x <= Math.Max(point1.x, point2.x));
    }
}
