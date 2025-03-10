    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoutonElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button bouton;
    private Image image;
    private RectTransform rectangle;
    private Rect tailleInitiale;
    private Color couleurInitiale;
     


    [SerializeField]
    private Color couleurSelectionne;

    [SerializeField]
    private GameObject tour;

    [SerializeField]
    private Button bouton1;


    // Start is called before the first frame update
    void Start()
    {
        bouton = GetComponent<Button>();
        image = GetComponent<Image>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = image.color;

        bouton1 = GameObject.Find("Bouton 1").GetComponent<Button>();
        tour = GameObject.Find("Tower Mage");
        bouton1.onClick.AddListener(AjouterObjet);
    }

    void AjouterObjet()
    {
        if (tour != null)
        {
            // Instancier l'objet dans la scène à la position (0, 0, 0)
            Instantiate(tour, Vector3.zero, Quaternion.identity);
            Debug.Log(tour.name + " ajouté à la scène");
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assigné !");
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
        image.color = couleurSelectionne;
        Debug.Log(gameObject.name + " sélectionné");
    }

    public void OnDeselectionner()
    {
        image.color = couleurInitiale;
        Debug.Log(gameObject.name + " déselectionné");
    }
}
