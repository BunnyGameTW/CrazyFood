using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour {
	void Update(){
		followMouse();
		check ();
	}

	public GameObject Obj;
	private Vector3 target = new Vector3();
	void followMouse() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{

			target = new Vector3(hit.point.x, 0.0f, hit.point.z);
		}
		transform.position =target;

	}
	void check() {
		if (Input.GetMouseButtonDown(0))
		{ //yes
			//
			Instantiate(Obj, transform.position, transform.rotation);
			//animation

			Player player=FindObjectOfType<Player>();
			player.animator.SetTrigger("skill1");//設定玩家召喚動畫；

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
}
