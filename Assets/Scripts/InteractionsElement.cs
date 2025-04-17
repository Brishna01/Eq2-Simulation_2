using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionsElement : MonoBehaviour
{
    private GrilleCircuit grilleCircuit;
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = GameObject.Find("Terrain").GetComponent<GrilleCircuit>();
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (systemePlacement.enlever)
        {
            grilleCircuit.SetElement(transform.position, null);
            grilleCircuit.SetValeur(transform.position, 0);

            Destroy(gameObject);
        }
        else
        {
            systemePlacement.CommencerPlacement(gameObject, false, true);
        }
    }
}
