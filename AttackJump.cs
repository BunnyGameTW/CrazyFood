using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AttackJump : MonoBehaviour {
	Animator ani;
	Rigidbody body;
	public float jumpHeight;
	bool canAttAgain,isGround;
	Tween tweener;//
	float posY,jumpTime;
	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
        canAttAgain = true;
	}
	
	// Update is called once per frame
	void Update () {
		jump ();
	}
	void jump(){
		

		//Debug.Log ("can att? " + canAttAgain);

		if (ani.GetBool ("isAtt")) {
			if (canAttAgain) {
				Debug.Log ("att start");
				tweener = transform.DOJump (transform.position, jumpHeight, 1, 1.0f,true);
                jumpTime = 0;
				posY = transform.position.y;
				ani.SetFloat ("speedY", 2.0f);
				Debug.Log ("jump UP");

				canAttAgain = false;
			} 

			//Debug.Log (body.velocity.y);
		}
		if(!canAttAgain){     
            jumpTime += Time.deltaTime;
			//Debug.Log("jumpTime:"+jumpTime);

			if (jumpTime > 0.4f && jumpTime < 0.5f) { 
				ani.SetFloat ("speedY", -0.7f);
				Debug.Log ("fall"+transform.position.y);
				//bug!!!
			}

			tweener.OnComplete (jumpOver);
        }
	}
	void jumpOver(){
		ani.SetFloat ("speedY", 0.0f);
		Debug.Log ("floor");

	}

    public void attEvent()
    {
        canAttAgain = true;
		ani.SetFloat ("speedY",2.0f);
		Debug.Log ("attevent");
    }
 
}
