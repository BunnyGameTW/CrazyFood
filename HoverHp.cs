using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HoverHp : MonoBehaviour {
	float nowHp,hp;
	public bool Player,Summon,Enemy;
	public GameObject HPobj;

	public Image hpImg;
	// Use this for initialization
	void Start () {
		HPobj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void getHp(){

		if (Player) {
			hp = GetComponent<Player> ().hp;
			nowHp = GetComponent<Player> ().nowHp;
			//	Debug.Log ("enter");
		}
		if (Summon) {
			hp = GetComponent<Summon> ().hp;
			nowHp = GetComponent<Summon> ().nowHp;
			//	Debug.Log ("enter");
		}
		if (Enemy) {
			hp = GetComponent<Enemy> ().hp;
			nowHp = GetComponent<Enemy> ().nowHp;
			//Debug.Log ("enter");
		}
        float fillAmount = hpImg.fillAmount;

        float ratio = nowHp / hp;
		if (ratio < 0.0f) ratio = 0.0f;
        hpImg.fillAmount = Mathf.Lerp(fillAmount, ratio, 0.1f); 
          /*
           float fillAmount=mpImg.fillAmount;//

                  power = player.GetComponent<Player> ().nowPower;
                  float ratio = power / maxPower;
                  if (ratio < 0.0f) ratio = 0.0f;
                  mpImg.fillAmount = Mathf.Lerp (fillAmount,ratio,0.1f);*/
        //  hpImg.rectTransform.localScale = new Vector3 (ratio, 1, 1);

	}
	/*void OnMouseEnter(){
		
		getHp ();

	}*/
	void OnMouseOver(){
	//	Debug.Log ("hover");

		getHp ();
		HPobj.SetActive (true);

	}
	void OnMouseExit(){
		HPobj.SetActive (false);

	//	Debug.Log ("exit");
	}
}
