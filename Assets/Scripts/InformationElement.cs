using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationElement : MonoBehaviour
{

    public TextMeshProUGUI infoElementText;// UI pour afficher la puissance et le courant 

    public TextMeshProUGUI courantText;   // UI pour afficher le courant
    public TextMeshProUGUI puissanceText; // UI pour afficher la puissance

    public TextMeshProUGUI tensionText;   // UI pour afficher la tension
    public TextMeshProUGUI resistanceText; // UI pour afficher la resistance

    private ElementCircuit elementCircuit;
    
    // Start is called before the first frame update
    void Start()
    {
        infoElementText = GameObject.Find("InfoElementText").GetComponent<TextMeshProUGUI>();

        courantText = GameObject.Find("TextCourant").GetComponent<TextMeshProUGUI>();
        puissanceText = GameObject.Find("TextPuissance").GetComponent<TextMeshProUGUI>();

        tensionText = GameObject.Find("TextTension").GetComponent<TextMeshProUGUI>();
        resistanceText = GameObject.Find("TextResistance").GetComponent <TextMeshProUGUI>();

        elementCircuit = GetComponent<ElementCircuit>();

        infoElementText.enabled = false;
        courantText.enabled = false;
        puissanceText.enabled = false;
        tensionText.enabled = false;
        resistanceText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        infoElementText.text = gameObject.name ;
        infoElementText.enabled = true; // S'assurer que le texte est visible

        // Afficher deux décimales : https://stackoverflow.com/a/164932
        courantText.text = "Courant : " + elementCircuit.intensite.ToString("0.##") + " A";
        courantText.enabled = true;
        puissanceText.text = "Puissance : " + elementCircuit.puissance.ToString("0.##") + " W";
        puissanceText.enabled = true;
        tensionText.text = "Tension : " + elementCircuit.tension.ToString("0.##") + " V";
        tensionText.enabled = true;
        resistanceText.text = "Resistance : " + elementCircuit.resistance.ToString("0.##") + " Ω";
        resistanceText.enabled = true;
    }

    public void OnMouseExit()
    {
        infoElementText.enabled = false;
        courantText.enabled = false;
        puissanceText.enabled = false;
        tensionText.enabled = false;
        resistanceText.enabled = false;
    }
}
