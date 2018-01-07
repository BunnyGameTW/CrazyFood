using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePan : MonoBehaviour {
    Renderer ren;
    Color col;
    Vector3 pos, rot;
    // Use this for initialization
    void Start () {
        ren = GetComponent<Renderer>();
        col = ren.material.GetColor("_Color");
        col.a = 0.3f;
        ren.material.SetColor("_Color", col); //設定Shader   字串對接
       
    }

}
