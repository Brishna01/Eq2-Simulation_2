using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class SolveurCircuit : MonoBehaviour
{
    [SerializeField]
    private bool modeDebug;

    private GrilleCircuit grilleCircuit;

    // Start is called before the first frame update
    void Start()
    {
        grilleCircuit = GetComponent<GrilleCircuit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResoudreCircuit();
        }
    }

    public SimulationCircuit ResoudreCircuit()
    {
        SimulationCircuit simulationCircuit = new SimulationCircuit(grilleCircuit);
        simulationCircuit.Resoudre();
        simulationCircuit.Appliquer();

        if (modeDebug)
        {
            simulationCircuit.AfficherMatrices();
            simulationCircuit.AfficherNoeuds();
        }

        return simulationCircuit;
    }
}
