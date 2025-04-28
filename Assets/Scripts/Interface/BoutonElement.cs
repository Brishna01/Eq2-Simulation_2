using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoutonElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
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

    // Start is called before the first frame update
    void Start()
    {
        imageBouton = GetComponent<Image>();
        imageElement = transform.Find("Image").GetComponent<Image>();
        texteElement = transform.Find("MeshTexte").GetComponent<TextMeshProUGUI>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = imageBouton.color;

        AfficherElement();
    }

    private void AfficherElement()
    {
        if (elementCircuit != null)
        {
            SpriteRenderer afficheurSprite = elementCircuit.GetComponentInChildren<SpriteRenderer>();
            if (afficheurSprite != null)
            {
                imageElement.sprite = afficheurSprite.sprite;
                imageElement.color = afficheurSprite.color;
                imageElement.preserveAspect = true;
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
}
