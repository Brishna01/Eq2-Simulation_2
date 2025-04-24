using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class SolveurCircuit : MonoBehaviour
{
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
        simulationCircuit.Afficher();

        return simulationCircuit;
    }

    private void ResoudreCircuitTest()
    {
        Matrix<double> G = Matrix<double>.Build.Dense(4, 4);
        Vector<double> i = Vector<double>.Build.Dense(4);

        G[0, 0] = 1 / 5.0;
        G[0, 1] = -1 / 5.0;
        G[0, 2] = 0.0;
        G[0, 3] = 1.0;
        G[1, 0] = -1 / 5.0;
        G[1, 1] = 1 / 5.0 + 1 / 10.0 + 1 / 7.0;
        G[1, 2] = -1 / 7.0;
        G[1, 3] = 0.0;
        G[2, 0] = 0.0;
        G[2, 1] = -1 / 7.0;
        G[2, 2] = 1 / 7.0;
        G[2, 3] = 0.0;
        G[3, 0] = 1.0;
        G[3, 1] = 0.0;
        G[3, 2] = 0.0;
        G[3, 3] = 0.0;

        i[0] = 0.0;
        i[1] = 0.0;
        i[2] = 0.0;
        i[3] = 1.0;

        Vector<double> v = G.Solve(i);
    }
}
