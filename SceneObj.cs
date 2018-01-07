using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObj : MonoBehaviour {

    public bool ISOpacity;
    private Renderer ren;
    void Start () {
        ISOpacity = false;
        ren = GetComponent<Renderer>();
    }
    public void SetOpacity(bool flag)
    {
        if (flag)
        {
            
            for (int i = 0; i < ren.materials.Length; i++)
            {
                Color col;
                col = ren.materials[i].GetColor("_Color");
                col.a = 0.5f;
                ren.materials[i].SetColor("_Color", col);
            }
        }
        else
        {
            for (int i = 0; i < ren.materials.Length; i++)
            {
                Color col;
                col = ren.materials[i].GetColor("_Color");
                col.a =1.0f;
                ren.materials[i].SetColor("_Color", col);
            }
        }
    }
}
//Renderer.material
//This function automatically instantiates the materials and makes them unique to this renderer. 
//It is your responsibility to destroy the materials when the game object is being destroyed. 
//Resources.UnloadUnusedAssets also destroys the materials but it is usually only called when loading a new level.
//也就是說要特別刪!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
//現在並未實作刪除  因為尚未切換場景