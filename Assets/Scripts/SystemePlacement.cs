using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SystemePlacement : MonoBehaviour
{
    [SerializeField]
    private Vector2 decalageCurseur;
    private GameObject objetModele;
    private GameObject objetAPlacer;

    [SerializeField]
    private Grid grille;
    [SerializeField]
    private GameObject grilleVisuelle;
    [SerializeField]
    private GameObject conteneur;

    private new Camera camera;
    private EventSystem systemeEvenements;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        systemeEvenements = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        if (grilleVisuelle != null)
        {
            grilleVisuelle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objetAPlacer != null)
        {
            objetAPlacer.transform.position = CalculerPositionCibleMonde();
        }

        if (Input.GetMouseButtonDown(0) && !systemeEvenements.IsPointerOverGameObject())
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

        if (conteneur != null)
        {
            objetAPlacer.transform.parent = conteneur.transform;
        }

        objetAPlacer = null;
        CommencerPlacement(objetModele);
    }

    public void CommencerPlacement(GameObject _objetModele)
    {
        objetModele = _objetModele;

        if (objetModele == null)
        {
            objetAPlacer = null;
            return;
        }

        objetAPlacer = Instantiate(objetModele, transform);
        objetAPlacer.name = objetModele.name;
        objetAPlacer.transform.position = CalculerPositionCibleMonde();

        SpriteRenderer afficheurSprite = objetAPlacer.GetComponent<SpriteRenderer>();
        if (afficheurSprite != null)
        {
            Color couleur = afficheurSprite.color;
            afficheurSprite.color = new Color(couleur.r, couleur.g, couleur.b, 0.8f);
        }

        if (grilleVisuelle != null)
        {
            grilleVisuelle.SetActive(true);
        }
    }

    public void ArreterPlacement()
    {
        if (objetAPlacer != null)
        {
            Destroy(objetAPlacer);
            objetAPlacer = null;
        }

        if (grilleVisuelle != null)
        {
            grilleVisuelle.SetActive(false);
        } 

        objetModele = null;
    }

    private Vector3 CalculerPositionCibleMonde()
    {
        Vector3 positionEcran = Input.mousePosition + new Vector3(decalageCurseur.x, decalageCurseur.y, 0);
        Vector3 positionMonde = camera.ScreenToWorldPoint(positionEcran);

        if (grille != null)
        {
            positionMonde = grille.WorldToCell(positionMonde) + new Vector3(0.5f, 0.5f, 0);
        }

        return new Vector3(positionMonde.x, positionMonde.y, 0);
    }
}
