using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {
    public Transform SpriteOBJ1; //抓子物件
    GameObject Player1;
    Player PlayerClass;
    // Use this for initialization
    void Start ()
    {
        SpriteOBJ1 = transform.GetChild(0);
        Player1=GameObject.FindGameObjectWithTag("Player");
        PlayerClass = Player1.GetComponent<Player>();
        SpriteOBJ1.transform.GetComponent<SpriteRenderer>().color = new Color(256, 256, 256, 0.98f);
    }
    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject.CompareTag("PlayerColliderPoint2")))
        {
             PlayerClass.SetbrushFlag(true); //設定在草裡
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if ((collider.gameObject.CompareTag("PlayerColliderPoint2")))
        {
            SpriteOBJ1.transform.GetComponent<SpriteRenderer>().color = new Color(256, 256, 256, 0.5f);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if ((collider.gameObject.CompareTag("PlayerColliderPoint2")))
        {
            PlayerClass.SetbrushFlag(false); //設定在草裡
            SpriteOBJ1.transform.GetComponent<SpriteRenderer>().color = new Color(256, 256, 256, 0.98f);
        }
    }
}

