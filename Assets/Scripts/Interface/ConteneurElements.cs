using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConteneurElements : MonoBehaviour
{
    private BoutonElement boutonSelectionne;
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();

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
            DeselectionnerBoutonElement();
        }
    }

    private void SelectionnerBoutonElement(BoutonElement boutonElement)
    {
        if (boutonSelectionne)
        {
            DeselectionnerBoutonElement();
        }

        boutonSelectionne = boutonElement;
        boutonSelectionne.OnSelectionner();
        systemePlacement.CommencerPlacement(boutonSelectionne.elementCircuit);
    }

    private void DeselectionnerBoutonElement()
    {
        if (boutonSelectionne != null)
        {
            boutonSelectionne.OnDeselectionner();
            boutonSelectionne = null;
        }

        systemePlacement.ArreterPlacement();
    }
}
