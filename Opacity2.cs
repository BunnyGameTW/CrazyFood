using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opacity2 : MonoBehaviour {

    private GameObject Player,MainCam;
    SceneObj SceneObjScript;
    void Start () {
        Player = GameObject.Find("Player");
        MainCam = GameObject.Find("Main Camera");
        
    }
	void Update () {
       this.transform.position= MainCam.transform.position;
        this.transform.LookAt(Player.transform, Vector3.up);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Scene")
        {
            Vector3 V1, V2;
            V1 = other.transform.position - MainCam.transform.position;
            V2 = Player.transform.position - MainCam.transform.position;
            if (Vector3.Distance(other.transform.position, MainCam.transform.position)< Vector3.Distance(Player.transform.position, MainCam.transform.position)) {
                SceneObjScript = other.transform.gameObject.GetComponent<SceneObj>();
                SceneObjScript.ISOpacity = true;
                SceneObjScript.SetOpacity(SceneObjScript.ISOpacity);
       //         Debug.Log(other + "in");
            }
        }
       // else
         //   Debug.Log(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Scene")
        {
            SceneObjScript = other.transform.gameObject.GetComponent<SceneObj>();
            SceneObjScript.ISOpacity = false;
            SceneObjScript.SetOpacity(SceneObjScript.ISOpacity);
    //        Debug.Log(other + "out");
        }
    }
}
