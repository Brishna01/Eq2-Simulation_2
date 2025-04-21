using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Xml.Linq;
using CodeMonkey.Utils;

public class SystemePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject terrain;
    private GrilleCircuit grilleCircuit;

    [SerializeField]
    private GameObject conteneurObjets;

    [SerializeField]
    private Vector2 decalageCurseur;

    private GameObject objetAPlacer;
    private Vector3? positionInitiale;
    private bool modeDragEtDrop;

    public Action<GameObject, bool> onObjetPlace;
    public Action<GameObject, bool> onPlacementArrete;

    private new Camera camera;
    private EventSystem systemeEvenements;

    public bool enlever { get; private set; }

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            enlever = !enlever;
            ArreterPlacement();
        }

        if (objetAPlacer != null)
        {
            objetAPlacer.transform.position = CalculerPositionCibleMonde();
        }

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

        Vector3 positionMondeArete = GetPositionMondeArete(objetAPlacer.transform.position);
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
        enlever = false;

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
        Vector3 positionEcran = Input.mousePosition + new Vector3(decalageCurseur.x, decalageCurseur.y, 0);
        Vector3 positionMonde = camera.ScreenToWorldPoint(positionEcran);
        positionMonde.z = 0;

        (Vector2 point1, Vector2 point2) = grilleCircuit.GetArete(positionMonde);
        Vector3 positionMondeArete = GetPositionMondeArete(positionMonde);

        if (grilleCircuit.EstDedans(point1) && grilleCircuit.EstDedans(point2) && (positionMonde - positionMondeArete).magnitude < 0.3)
        {
            return positionMondeArete;
        }

        return positionMonde;
    }

    private Vector3 GetPositionMondeArete(Vector3 positionMonde)
    {
        (Vector2 point1, Vector2 point2) = grilleCircuit.GetArete(positionMonde);

        return grilleCircuit.GetPositionMonde((point1.x + point2.x) / 2, (point1.y + point2.y) / 2);
    }
}
