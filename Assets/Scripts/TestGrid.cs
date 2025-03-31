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
    //public GridParalelle gridInput;
    public int chiffre= 6;

    private GameObject objet = null;

    private void Start()
    {
         grid = new GridParalelle(colonnes, lignes, 1f, 1f);
    }

    private void Update()
    {
         if (Input.GetMouseButtonDown(0))
         {
             grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56, objet);
         }

    }

    // Debug.Log(width + " "+ height);


        
}
