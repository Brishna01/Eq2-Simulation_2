using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoutonElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button bouton;
    private Image imageBouton;
    private Image imageElement;
    private TextMeshProUGUI texteElement;
    private RectTransform rectangle;
    private Rect tailleInitiale;
    private Color couleurInitiale;

    [SerializeField]
    private Color couleurSelectionne;
    [SerializeField]
    public GameObject elementCircuit;

    [SerializeField]
    private GameObject tour;
    private System.Random rand;
    public LayerMask solMask; // Layer du sol pour que l'objet suive la scène

    private GameObject towerActuelle;
    private bool enPlacement = false;
    public Camera cam;

    public TextMeshProUGUI courantText;   // UI pour afficher le courant
    public TextMeshProUGUI puissanceText; // UI pour afficher la puissance

    public float courant;   // Valeur pour le courant
    public float puissance; // Valeur pour la puissance

    // Start is called before the first frame update
    void Start()
    {
        bouton = GameObject.Find("Btn Tower").GetComponent<Button>();
        bouton.onClick.AddListener(GenererTower);


        imageBouton = GetComponent<Image>();
        imageElement = transform.Find("Image").GetComponent<Image>();
        //texteElement = transform.Find("MeshTexte").GetComponent<TextMeshProUGUI>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = imageBouton.color;
        tour = GameObject.Find("Tower Mage");

        rand = new System.Random();

        AfficherElement();
    }

    private void AfficherElement()
    {
        if (elementCircuit != null)
        {
            SpriteRenderer afficheurSprite = elementCircuit.GetComponent<SpriteRenderer>();
            if (afficheurSprite != null)
            {
                imageElement.color = afficheurSprite.color;
                texteElement.enabled = false;
            }
            else
            {
                texteElement.text = elementCircuit.name;
                imageElement.enabled = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (enPlacement && towerActuelle != null)
        {
            Vector3 sourisPosition = Input.mousePosition;
            sourisPosition = cam.ScreenToWorldPoint(sourisPosition);
            sourisPosition.z = 0;

            RaycastHit hit;
            if (Physics.Raycast(sourisPosition, Vector3.forward, out hit, Mathf.Infinity, solMask))
            {
                towerActuelle.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                enPlacement = false; // Finalise le placement de la tour
            }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tailleInitiale.width * 1.05f);
        rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tailleInitiale.height * 1.05f);
    }

    public void OnPointerExit(PointerEventData data)
    {
        rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tailleInitiale.width);
        rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tailleInitiale.height);
    }

    public void OnSelectionner()
    {
        imageBouton.color = couleurSelectionne;

    }

    public void OnDeselectionner()
    {
        imageBouton.color = couleurInitiale;
    }
    private void GenererTower()
    {
        if (tour != null)
        {
            towerActuelle = Instantiate(tour); // Crée une copie de "Tower Mage"
            enPlacement = true;

            courant = 10.0f;
            puissance = 50.0f;

        }
        else
        {
            Debug.LogError("Tower Mage n'est pas assigné dans l'Inspector !");
        }
    }
    private void UpdateUI()
    {
        // Mettre à jour l'affichage du courant et de la puissance
        courantText.text = "Courant: " + courant.ToString("F1") + " A"; // Affiche le courant avec 1 chiffre après la virgule
        puissanceText.text = "Puissance: " + puissance.ToString("F1") + " W"; // Affiche la puissance avec 1 chiffre après la virgule
    }


}
