using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {
    public GameObject HoverObj;
    // Use this for initialization
    void Start () {
        HoverObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseEnter(){

        HoverObj.SetActive(true);


    }
    /*void OnMouseOver()
    {
        
       HoverObj.SetActive(true);

    }*/
    void OnMouseExit()
    {
        Debug.Log("888");
        HoverObj.SetActive(false);

    }
}
