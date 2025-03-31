using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoutonElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Button bouton;
    private Image imageBouton;
    private Image imageElement;
    private TextMeshProUGUI texteElement;
    private RectTransform rectangle;
    private Rect tailleInitiale;
    private Color couleurInitiale;
    public Action<BoutonElement> onPointerDown;

    [SerializeField]
    private Color couleurSelectionne;
    [SerializeField]
    public GameObject elementCircuit;

    [SerializeField]
    private GameObject tour;
    private System.Random rand;

    public TextMeshProUGUI courantText;   // UI pour afficher le courant
    public TextMeshProUGUI puissanceText; // UI pour afficher la puissance

    public double courant;   // Valeur pour le courant
    public double puissance; // Valeur pour la puissance

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Btn Tower")) {
            bouton = GameObject.Find("Btn Tower").GetComponent<Button>();
            courant = 10.0;
            puissance = 50.0;
            bouton.onClick.AddListener(UpdateUI);
        }

        imageBouton = GetComponent<Image>();
        imageElement = transform.Find("Image").GetComponent<Image>();
        //texteElement = transform.Find("MeshTexte").GetComponent<TextMeshProUGUI>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = imageBouton.color;
        tour = GameObject.Find("Tower Mage");

        rand = new System.Random();

        AfficherElement();

        courantText = GameObject.Find("TextCourant").GetComponent<TextMeshProUGUI>();
        puissanceText = GameObject.Find("TextPuissance").GetComponent<TextMeshProUGUI>();
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

    public void OnPointerDown(PointerEventData data)
    {
        onPointerDown(this);
    }

    public void OnSelectionner()
    {
        imageBouton.color = couleurSelectionne;

    }

    public void OnDeselectionner()
    {
        imageBouton.color = couleurInitiale;
    }
    
    private void UpdateUI()
    {
        // Mettre à jour l'affichage du courant et de la puissance
        courantText.text = "Courant: " + courant.ToString("F1") + " A"; // Affiche le courant avec 1 chiffre après la virgule
        puissanceText.text = "Puissance: " + puissance.ToString("F1") + " W"; // Affiche la puissance avec 1 chiffre après la virgule
    }


}
