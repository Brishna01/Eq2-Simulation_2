using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class SystemePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject terrain;
    private GrilleCircuit grilleCircuit;

    [SerializeField]
    private GameObject conteneurObjets;
    [SerializeField]
    private GameObject prefabFilElectrique;

    private GameObject objetAPlacer;
    private Vector3? positionInitiale;
    private bool modeDragEtDrop;
    public Outil outilSelectionne { get; private set; } = Outil.DeplacerElements;

    private GameObject filElectrique;
    private Vector2? pointPrecedent;
    private bool boutonGaucheEnfonce;

    public Action<GameObject, bool> onObjetPlace;
    public Action<GameObject, bool> onPlacementArrete;

    private new Camera camera;
    private EventSystem systemeEvenements;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = terrain.GetComponent<GrilleCircuit>();

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
            outilSelectionne = Outil.Supprimer;
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
        else if (outilSelectionne == Outil.TracerFils)
        {
            if (boutonGaucheEnfonce)
            {
                Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
                positionMonde.z = 0;

                Vector2 point = grilleCircuit.GetPoint(positionMonde);
                Vector3 positionMondePoint = grilleCircuit.GetPositionMonde(point.x, point.y);

                if ((positionMonde - positionMondePoint).magnitude < 0.2)
                {
                    if (pointPrecedent == null && Input.GetMouseButtonDown(0))
                    {
                        pointPrecedent = point;
                    }
                    else if (pointPrecedent != null && point != pointPrecedent 
                        && grilleCircuit.SontAdjacents(point, (Vector2)pointPrecedent)
                        && !grilleCircuit.GetFil(point, (Vector2)pointPrecedent))
                    {
                        GameObject filElectrique = Instantiate(prefabFilElectrique);
                        LineRenderer afficheurLigne = filElectrique.GetComponent<LineRenderer>();

                        Vector3[] positions = {
                            grilleCircuit.GetPositionMonde(((Vector2)pointPrecedent).x, ((Vector2)pointPrecedent).y),
                            grilleCircuit.GetPositionMonde(point.x, point.y)
                        };
                        afficheurLigne.SetPositions(positions);

                        filElectrique.transform.parent = conteneurObjets.transform;
                        grilleCircuit.SetFil(point, (Vector2)pointPrecedent, filElectrique.GetComponent<FilElectrique>());
                    }

                    if (grilleCircuit.SontAdjacents(point, (Vector2)pointPrecedent))
                    {
                        pointPrecedent = point;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                pointPrecedent = null;
            }
        }
        else if (outilSelectionne == Outil.Supprimer)
        {
            if (boutonGaucheEnfonce)
            {
                Vector3 positionMonde = camera.ScreenToWorldPoint(Input.mousePosition);
                positionMonde.z = 0;

                (Vector2 point1, Vector2 point2) = grilleCircuit.GetArete(positionMonde);

                if (GetDistanceArete(point1, point2, positionMonde) < 0.1)
                {
                    FilElectrique filElectrique = grilleCircuit.GetFil(point1, point2);

                    if (filElectrique)
                    {
                        grilleCircuit.RetirerFil(filElectrique);
                        Destroy(filElectrique.gameObject);
                    }
                }
            }
        }
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

        (Vector2 point1, Vector2 point2) = grilleCircuit.GetArete(objetAPlacer.transform.position);
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

        (Vector2 point1, Vector2 point2) = grilleCircuit.GetArete(positionMonde);
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);

        if (grilleCircuit.EstDedans(positionMonde) && GetDistanceArete(point1, point2, positionMonde) < 0.3)
        {
            return positionMondeArete;
        }

        return positionMonde;
    }

    private Vector3 GetPositionMondeArete(Vector2 point1, Vector2 point2)
    {
        return grilleCircuit.GetPositionMonde((point1.x + point2.x) / 2, (point1.y + point2.y) / 2);
    }

    private float GetDistanceArete(Vector2 point1, Vector2 point2, Vector3 positionMonde)
    {
        Vector3 positionMondeArete = GetPositionMondeArete(point1, point2);

        if (point1.x == point2.x)
        {
            return Math.Abs(positionMonde.x - positionMondeArete.x);
        } else if (point1.y == point2.y)
        {
            return Math.Abs(positionMonde.y - positionMondeArete.y);
        }
        
        return (positionMonde - positionMondeArete).magnitude;
    }
}
