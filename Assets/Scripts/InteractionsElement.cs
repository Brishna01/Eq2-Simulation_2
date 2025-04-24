using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionsElement : MonoBehaviour
{
    private ElementCircuit elementCircuit;
    private GrilleCircuit grilleCircuit;
    private ControleurOutils controleurOutils;
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        elementCircuit = GetComponent<ElementCircuit>();
        grilleCircuit = GameObject.Find("Circuit").GetComponent<GrilleCircuit>();
        controleurOutils = GameObject.Find("ControleurOutils").GetComponent<ControleurOutils>();
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        switch (controleurOutils.outilSelectionne)
        {
            case Outil.DeplacerElements:
                systemePlacement.CommencerPlacement(gameObject, false, true);
                break;
        }
    }

    public void OnMouseOver()
    {
        switch (controleurOutils.outilSelectionne)
        {
            case Outil.SupprimerElements:
                if (controleurOutils.boutonGaucheEnfonce && !systemePlacement.estActif)
                {
                    grilleCircuit.RetirerElement(elementCircuit);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
