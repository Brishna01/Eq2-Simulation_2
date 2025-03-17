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
    private System.Random rand;

    [SerializeField]
    private Color couleurSelectionne;

    [SerializeField]
    public GameObject elementCircuit;

    [SerializeField]
    private GameObject tour;

    // Start is called before the first frame update
    void Start()
    {
        bouton = GetComponent<Button>();
        imageBouton = GetComponent<Image>();
        imageElement = transform.Find("Image").GetComponent<Image>();
        texteElement = transform.Find("MeshTexte").GetComponent<TextMeshProUGUI>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = imageBouton.color;

        tour = GameObject.Find("Tower Mage");

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

    void AjouterObjet()
    {
        if (tour != null)
        {
            int x = rand.Next(-5, 5);
            int y = rand.Next(-5, 5);
            int z = rand.Next(-5, 5);

            Instantiate(tour, new Vector3(x, y, z), Quaternion.identity);
            Debug.Log("Nouveau tour ajout� � la place " + x+"," + y + "," + z);
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assign� !");
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

    public void OnSelectionner()
    {
        imageBouton.color = couleurSelectionne;
    }

    public void OnDeselectionner()
    {
        imageBouton.color = couleurInitiale;
    }
}
