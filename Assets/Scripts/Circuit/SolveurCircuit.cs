using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrôle la résolution de circuit.
/// </summary>
public class SolveurCircuit : MonoBehaviour
{
    private GrilleCircuit grilleCircuit;
    private SimulationCircuit simulationCircuit;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = GetComponent<GrilleCircuit>();

        grilleCircuit.onModifie += () => simulationCircuit = ResoudreCircuit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (simulationCircuit != null)
            {
                simulationCircuit.AfficherMatrices();
                simulationCircuit.AfficherNoeuds();
            }
        }
    }

    /// <summary>
    /// Crée une nouvelle simulation du circuit.
    /// </summary>
    /// <returns>la simulation de circuit</returns>
    public SimulationCircuit ResoudreCircuit()
    {
        SimulationCircuit simulationCircuit = new SimulationCircuit(grilleCircuit);
        simulationCircuit.Resoudre();
        simulationCircuit.Appliquer();

        return simulationCircuit;
    }
}
