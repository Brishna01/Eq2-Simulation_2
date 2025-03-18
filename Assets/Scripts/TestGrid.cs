using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestGrid : MonoBehaviour 
{
    // Start is called before the first frame update
   
    private int colonnes = 5;
    private int lignes = 4;
    private GridParalelle grid;
    public GridParalelle gridInput;
    public int chiffre= 6;

    private void Start()
    {
         grid = new GridParalelle(colonnes, lignes, 4f, 1.75f);
    }

    private void Update()
    {
         if (Input.GetMouseButtonDown(0))
         {
             grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
         }

    }

    // Debug.Log(width + " "+ height);


        
}
