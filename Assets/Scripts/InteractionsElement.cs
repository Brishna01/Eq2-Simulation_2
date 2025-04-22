using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionsElement : MonoBehaviour
{
    private ElementCircuit elementCircuit;
    private GrilleCircuit grilleCircuit;
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        elementCircuit = GetComponent<ElementCircuit>();
        grilleCircuit = GameObject.Find("Terrain").GetComponent<GrilleCircuit>();
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        switch (systemePlacement.outilSelectionne)
        {
            case Outil.DeplacerElements:
                systemePlacement.CommencerPlacement(gameObject, false, true);
                break;
            case Outil.Supprimer:
                grilleCircuit.RetirerElement(elementCircuit);
                Destroy(gameObject);
                break;
        }
    }
}
