using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotCover : MonoBehaviour {
	Rigidbody body;
	public Vector3 potDir;
	bool once;
	float timer;
	public float lifeTime;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		once=true;
	}
	
	// Update is called once per frame
	void Update () {
		if (once) {
			body.AddForce (potDir,ForceMode.Impulse);
		//	body.AddTorque(new Vector3(0,potDir.y*2.0f,0),ForceMode.Impulse);//mesh rotate

			once = false;
			Debug.Log("potDir "+potDir);
		}
		//if(!startTimer)
		timer += Time.deltaTime;   
		delete();

	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "floor") {
			Debug.Log ("ground");

		}
	}
	void  delete(){
		if (timer+2.0f > lifeTime) {
			//this.GetComponentInChildren<SphereCollider>().center += new Vector3(0, -50, 0);//觸發ontriggerExit

			Destroy(this.gameObject,2.0f); }
	
	
	} 
	
}
