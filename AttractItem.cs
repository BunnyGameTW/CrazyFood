using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractItem : MonoBehaviour {
    float timer;
    public float lifeTime;
	// Use this for initialization
	void Awake () {
		
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;   
        delete();

    }
    void attract() {

    }
    void delete() {
        if (timer+2.0f > lifeTime) {
            this.GetComponentInChildren<SphereCollider>().center += new Vector3(0, -50, 0);//觸發ontriggerExit

            Destroy(this.gameObject,2.0f); }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("123");
    }
}
