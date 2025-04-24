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

    private GameObject objetAPlacer;
    private Vector3? positionInitiale;
    private bool modeDragEtDrop;
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
        Vector3 positionMondeArete = grilleCircuit.GetPositionMondeArete(point1, point2);
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

        if (onObjetPlace != null)
        {
            onObjetPlace(objetPlace, modeDragEtDrop);
        }
    }

    public void ArreterPlacement()
    {
        if (objetAPlacer != null && objetAPlacer.transform.parent == transform)
        {
            Destroy(objetAPlacer);

            if (onPlacementArrete != null)
            {
                onPlacementArrete(objetAPlacer, modeDragEtDrop);
            }
            
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
        Vector3 positionMondeArete = grilleCircuit.GetPositionMondeArete(point1, point2);

        if (grilleCircuit.EstAssezProcheArete(point1, point2, positionMonde, 0.3f))
        {
            return positionMondeArete;
        }

        return positionMonde;
    }
}
