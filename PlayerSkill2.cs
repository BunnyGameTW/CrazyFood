using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill2 : MonoBehaviour {
	bool checkRot;
	Sprite skill2sprite;
	public GameObject potCover;
	Transform childTran;
	// Use this for initialization
	void OnEnable(){
		childTran = transform.GetChild (0);// GetComponentInChildren<Transform> ();
		childTran.localScale=new Vector3(1.0f,1.0f,1.0f);
		checkRot = false;
		followPlayerPos ();
	//	Debug.Log(childTran.localScale);
		timer = 0.0f;


	}
	// Update is called once per frame
	void Update () {
		
		if(!checkRot)followMouseRot ();
		check ();
	}

	Vector3 target=new Vector3();
	void followMouseRot(){
		Quaternion q=new Quaternion();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			target = new Vector3(hit.point.x,0.0f, hit.point.z);
		}

		target -= transform.position;
		target.y = 0.0f;
		q.SetLookRotation (target, Vector3.up);
		transform.rotation = q;
	}
	void followPlayerPos(){
		Vector3 playerPos = FindObjectOfType<Player> ().transform.position;
        

        transform.position = playerPos;
        
		//Debug.Log ("skill2Pos: " + transform.position);
	}
	float timer=0.0f;
	float potCoverStrength=0.0f;
	internal Vector3 potCoverDir;
	public float theta=0.0f;
	public float strengthRatio;
	void check(){
			//力道
			float scaleRatio=4;
			timer += Time.deltaTime;

			Vector3 scale = new Vector3 (childTran.localScale.x, scaleRatio*Mathf.Abs (Mathf.Sin (timer))+1, childTran.localScale.z);//變形怪怪的
			childTran.localScale = scale;
			potCoverStrength = scale.y;

		//}

		if (Input.GetMouseButtonDown (1)) {//no
			checkRot = true;

			Player player = FindObjectOfType<Player> ();
			player.isSkill = false;
			player.animator.SetBool ("isSkill2", false);
			this.gameObject.SetActive (false);
		}
		if (Input.GetMouseButtonUp (0)) {
			checkRot = true;

			//animation
			//y=1 rt=1 r x,y,z (x,ycosT+zsinT,ysinT+zcosT)
			//animation end射出鍋蓋
			//x y z pos=pos.x+cos(rot.y)*localscake.y pos.z+sin(rot.y),
			//strengthRatio=150.0f;
			target.y=Mathf.Abs( target.x*0.6f);
			potCoverDir = target.normalized * potCoverStrength * strengthRatio;
			Debug.Log (potCoverDir);
			float theta2=Mathf.Deg2Rad*theta;
			potCoverDir=new Vector3(potCoverDir.x, potCoverDir.y*Mathf.Cos(theta2)+potCoverDir.z*Mathf.Sin(theta2), potCoverDir.z*Mathf.Cos(theta2)+potCoverDir.y*Mathf.Sin(theta2));
			potCover.GetComponent<PanT>().potDir=potCoverDir;//set potCoverDir
			Vector3 pos=transform.position;
			pos.y = 4.05f;
			Instantiate (potCover, pos, transform.rotation);
			//set summon distance
			//Summon []summons=FindObjectsOfType<Summon>();
			//cost power
			Player player = FindObjectOfType<Player> ();
			player.animator.SetTrigger ("skill2");
			player.animator.SetBool ("isSkill2",false);

			player.isSkill = false;
			this.gameObject.SetActive (false);
		}
	}


}
