using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlleurDrag : MonoBehaviour
{
    private SystemePlacement systemePlacement;

    // Start is called before the first frame update
    void Start()
    {
        systemePlacement = GameObject.Find("SystemePlacement").GetComponent<SystemePlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        systemePlacement.CommencerPlacement(gameObject, false, true);
    }
}
