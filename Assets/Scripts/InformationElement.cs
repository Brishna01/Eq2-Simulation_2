using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationElement : MonoBehaviour
{

    public TextMeshProUGUI texteElement;// UI pour afficher le nom de l'élément

    public TextMeshProUGUI texteCourant;   // UI pour afficher le courant
    public TextMeshProUGUI textePuissance; // UI pour afficher la puissance

    public TextMeshProUGUI texteTension;   // UI pour afficher la tension
    public TextMeshProUGUI texteResistance; // UI pour afficher la resistance

    private ElementCircuit elementCircuit;
    
    // Start is called before the first frame update
    void Start()
    {
        texteElement = GameObject.Find("TexteElement").GetComponent<TextMeshProUGUI>();

        texteCourant = GameObject.Find("TexteCourant").GetComponent<TextMeshProUGUI>();
        textePuissance = GameObject.Find("TextePuissance").GetComponent<TextMeshProUGUI>();

        texteTension = GameObject.Find("TexteTension").GetComponent<TextMeshProUGUI>();
        texteResistance = GameObject.Find("TexteResistance").GetComponent <TextMeshProUGUI>();

        elementCircuit = GetComponent<ElementCircuit>();

        texteElement.enabled = false;
        texteCourant.enabled = false;
        textePuissance.enabled = false;
        texteTension.enabled = false;
        texteResistance.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        texteElement.text = gameObject.name ;
        texteElement.enabled = true; // S'assurer que le texte est visible

        // Afficher deux décimales : https://stackoverflow.com/a/164932
        texteCourant.text = "Courant : " + elementCircuit.intensite.ToString("0.##") + " A";
        texteCourant.enabled = true;
        textePuissance.text = "Puissance : " + elementCircuit.puissance.ToString("0.##") + " W";
        textePuissance.enabled = true;
        texteTension.text = "Tension : " + elementCircuit.tension.ToString("0.##") + " V";
        texteTension.enabled = true;
        texteResistance.text = "Resistance : " + elementCircuit.resistance.ToString("0.##") + " Ω";
        texteResistance.enabled = true;
    }

    public void OnMouseExit()
    {
        texteElement.enabled = false;
        texteCourant.enabled = false;
        textePuissance.enabled = false;
        texteTension.enabled = false;
        texteResistance.enabled = false;
    }
}
