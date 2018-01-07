using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemHp : MonoBehaviour {
	int hp,beAttnum;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateHp ();
	}
	void updateHp(){//0 1  0 1 2
		
		BGItem item=GetComponentInParent<BGItem>();
		hp = item.hp;
		beAttnum = item.beAttNum;
		Color tColor = new Color (0, 0, 0, 255);

		if (beAttnum > 0) {
			for(int i=0;i<beAttnum;i++)
				transform.GetChild (hp - beAttnum).GetComponent<Image> ().color = tColor;
		}

	}
}
