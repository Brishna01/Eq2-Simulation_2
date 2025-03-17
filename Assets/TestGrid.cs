using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestGrid : MonoBehaviour 
{
    // Start is called before the first frame update
   
    private int colonnes = 5;
    private GridParalelle grid;

    private void Start()
    {
         grid = new GridParalelle(colonnes, 4, 3f);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }
    }

    // Debug.Log(width + " "+ height);


        
}
