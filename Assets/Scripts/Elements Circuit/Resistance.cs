using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistance : ElementCircuit
{
    [field: SerializeField]
    public override double resistance { get; set; }
    public override double puissance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
