using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConteneurElements : MonoBehaviour
{
    [SerializeField]
    private BoutonElement boutonSelectionne;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BoutonElement boutonElement in GetComponentsInChildren<BoutonElement>())
        {
            boutonElement.GetComponent<Button>().onClick.AddListener(() => OnBoutonClick(boutonElement));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBoutonClick(BoutonElement boutonElement)
    {
        if (boutonElement != boutonSelectionne) 
        {
            SelectionnerBoutonElement(boutonElement);
        } 
        else
        {
            boutonElement.OnDeselectionner();
            boutonSelectionne = null;
        }
    }

    private void SelectionnerBoutonElement(BoutonElement boutonElement)
    {
        if (boutonSelectionne)
        {
            boutonSelectionne.OnDeselectionner();
        }

        boutonSelectionne = boutonElement;
        boutonSelectionne.OnSelectionner();
    }
}
