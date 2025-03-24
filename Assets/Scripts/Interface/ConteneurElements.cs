using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConteneurElements : MonoBehaviour
{
    [SerializeField]
    private bool modeDragEtDrop;

    private BoutonElement boutonSelectionne;
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();

        foreach (BoutonElement boutonElement in GetComponentsInChildren<BoutonElement>())
        {
            boutonElement.onPointerDown += OnBoutonDown;
        }

        systemePlacement.onObjectPlace += (element, _modeDragEtDrop) =>
        {
            if (boutonSelectionne != null)
            {
                if (modeDragEtDrop)
                {
                    DeselectionnerBoutonElement();
                }
                else 
                {
                    systemePlacement.CommencerPlacement(boutonSelectionne.elementCircuit, true, modeDragEtDrop);
                }
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            DeselectionnerBoutonElement();
        }
    }

    private void OnBoutonDown(BoutonElement boutonElement)
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
        systemePlacement.CommencerPlacement(boutonSelectionne.elementCircuit, true, modeDragEtDrop);
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
