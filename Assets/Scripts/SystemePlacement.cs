using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SystemePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject terrain;
    private Grid grille;
    private GridParalelle grilleCircuit;
    private GameObject grillage;
    [SerializeField]
    private bool grillagePersistant;

    [SerializeField]
    private GameObject conteneurObjets;

    [SerializeField]
    private Vector2 decalageCurseur;

    private GameObject objetAPlacer;
    private bool modeDragEtDrop;

    public Action<GameObject, bool> onObjetPlace;
    public Action<GameObject, bool> onPlacementArrete;

    private new Camera camera;
    private EventSystem systemeEvenements;

    // Start is called before the first frame update
    void Start()
    {
        grille = terrain.GetComponent<Grid>();
        grilleCircuit = terrain.GetComponent<GridParalelle>();
        grillage = terrain.transform.Find("Grillage").gameObject;

        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        systemeEvenements = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        if (grillage != null)
        {
            grillage.transform.localScale = new Vector3(grilleCircuit.colonnes - 0.9f, grilleCircuit.lignes - 0.9f, 0);
            grillage.SetActive(grillagePersistant);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objetAPlacer != null)
        {
            objetAPlacer.transform.position = CalculerPositionCibleMonde();
        }

        if (modeDragEtDrop && Input.GetMouseButtonUp(0))
        {
            if (!systemeEvenements.IsPointerOverGameObject())
            {
                PlacerObject();
            }
            else
            {
                ArreterPlacement();
            }
        }
        else if (!modeDragEtDrop && Input.GetMouseButtonDown(0) && !systemeEvenements.IsPointerOverGameObject())
        {
            PlacerObject();
        }
    }

    private void PlacerObject()
    {
        if (objetAPlacer == null)
        {
            return;
        }

        SpriteRenderer afficheurSprite = objetAPlacer.GetComponent<SpriteRenderer>();
        if (afficheurSprite != null)
        {
            Color couleur = afficheurSprite.color;
            afficheurSprite.color = new Color(couleur.r, couleur.g, couleur.b);
        }

        if (conteneurObjets != null)
        {
            objetAPlacer.transform.parent = conteneurObjets.transform;
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

        if (cloner)
        {
            objetAPlacer = Instantiate(objet, transform);
            objetAPlacer.name = objet.name;
        }
        else
        {
            objetAPlacer = objet;
        }

        objetAPlacer.transform.position = CalculerPositionCibleMonde();

        SpriteRenderer afficheurSprite = objetAPlacer.GetComponent<SpriteRenderer>();
        if (afficheurSprite != null)
        {
            Color couleur = afficheurSprite.color;
            afficheurSprite.color = new Color(couleur.r, couleur.g, couleur.b, 0.8f);
        }

        if (grillage != null)
        {
            grillage.SetActive(true);
        }
    }

    public void ArreterPlacement()
    {
        if (objetAPlacer != null && objetAPlacer.transform.parent == transform)
        {
            Destroy(objetAPlacer);
            onPlacementArrete(objetAPlacer, modeDragEtDrop);
            objetAPlacer = null;
        }

        if (grillage != null && !grillagePersistant)
        {
            grillage.SetActive(false);
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

        if (grille != null)
        {
            positionMonde = grille.WorldToCell(positionMonde + new Vector3(0.5f, 0.5f, 0));
        }

        return new Vector3(
            Math.Clamp(positionMonde.x, -grilleCircuit.colonnes / 2, grilleCircuit.colonnes / 2), 
            Math.Clamp(positionMonde.y, -grilleCircuit.lignes / 2, grilleCircuit.lignes / 2), 
            0
        );
    }
}
