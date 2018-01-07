using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public float ptime;
	void Start(){
		ptime = 0;
	}
	public bool timer(float CD)
	{
		if (Time.timeSinceLevelLoad - ptime >= CD) {
			ptime = Time.timeSinceLevelLoad;
			return true;
		}
		else return false;
	}
}
