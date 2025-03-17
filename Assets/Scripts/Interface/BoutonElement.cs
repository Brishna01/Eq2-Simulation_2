using System;
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
    private System.Random rand;

    [SerializeField]
    private Color couleurSelectionne;

    [SerializeField]
    private GameObject tour;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random(); // Initialize random number generator
        image = GetComponent<Image>();
        rectangle = GetComponent<RectTransform>();
        tailleInitiale = new Rect(rectangle.rect);
        couleurInitiale = image.color;
        bouton = GameObject.Find("Bouton 1").GetComponent<Button>();
        tour = GameObject.Find("Tower Mage");

        bouton.onClick.AddListener(AjouterObjet);
    }

    void AjouterObjet()
    {
        if (tour != null)
        {
            int x = rand.Next(-5, 5);
            int y = rand.Next(-5, 5);
            int z = rand.Next(-5, 5);

            Instantiate(tour, new Vector3(x, y, z), Quaternion.identity);
            Debug.Log("Nouveau tour ajouté à la place " + x+"," + y + "," + z);
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assigné !");
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
        image.color = couleurSelectionne;
        Debug.Log(gameObject.name + " sélectionné");
    }

    public void OnDeselectionner()
    {
        image.color = couleurInitiale;
        Debug.Log(gameObject.name + " déselectionné");
    }
}
