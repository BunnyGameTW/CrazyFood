using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CSV.getInstance ().loadFile (Application.dataPath + "/Resources", "testTalk.csv");
		//for (int i = 0; i < CSV.getInstance ().arrayData.Count; i++) {
		//	Debug.Log (CSV.getInstance ().getString(0,1));
		//	Debug.Log (CSV.getInstance ().getString(1,0));

	//	}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
