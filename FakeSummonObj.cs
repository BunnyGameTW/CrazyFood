using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSummonObj : MonoBehaviour {
	Color spriteColor; 
	void Start(){
		canSummon = true;
		spriteColor=GetComponent<SpriteRenderer> ().color;
	}
	void Update(){
		followMouse();
		check ();
	}

	internal GameObject Obj;
	private Vector3 target = new Vector3();
	bool canSummon;
	void followMouse() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{

			target = new Vector3(hit.point.x, 0.5f, hit.point.z);//Y need to be set
		}
		transform.position =target;

	}
	void check() {
		if (Input.GetMouseButtonDown(0) && canSummon)
		{ //yes			
			Instantiate(Obj, transform.position, transform.rotation);
			//cost power
			Player player=FindObjectOfType<Player>();
			player.animator.SetTrigger("summon");//設定玩家召喚動畫
			player.SummonFX.SetActive(true);
			player.cost (Obj.GetComponent<Summon> ().summonCost);
			player.isSkill = false;
			this.gameObject.SetActive(false);
		}
		else if (Input.GetMouseButtonDown(1))
		{//no
			Player player=FindObjectOfType<Player>();
			player.isSkill = false;
			this.gameObject.SetActive(false);
		}
	}
	void OnTriggerStay(Collider col){
		if (col.tag == "summon") {
			canSummon = false;
			//change color
			//spriteColor.a=128;
			//GetComponent<SpriteRenderer> ().color=spriteColor;
			Debug.Log ("there's something!!");
		}
		if (col.tag == "canSummonArea") {
			canSummon = true;
			spriteColor.a=255;

			GetComponent<SpriteRenderer> ().color=spriteColor;

			//change color
		}
	}
	void OnTriggerExit(Collider col){
		if (col.tag == "summon") {
			canSummon = true;
			//spriteColor = Color.white;

			Debug.Log ("exit ");
		}
		if (col.tag == "canSummonArea") {
			canSummon = false;
			spriteColor.a=255;

			GetComponent<SpriteRenderer> ().color=spriteColor;

			//change color
		}
	}

}

