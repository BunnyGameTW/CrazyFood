using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTalk : MonoBehaviour {
	Fairy fairy;


	// Use this for initialization
	void Start () {
	//	change (gamestate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void change(int i){
		
		fairy = GameObject.FindObjectOfType<Fairy> ();
		fairy.changeTalkState (i);

	}
}
