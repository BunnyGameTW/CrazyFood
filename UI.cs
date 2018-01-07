using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour {
	public Text expUI,levelUI;
	public Image hpImg,mpImg;
	Image []summonsImg;

	public GameObject player;
	float hp,maxHp;
	float maxPower;

	float power,level,exp,money;
	// Use this for initialization
	void Start () {
		
		maxHp = player.GetComponent<Player> ().hp;
		maxPower = player.GetComponent<Player> ().maxPower;
		/*for (int i = 0; i < player.GetComponent<Player> ().summons.Length; i++) {//set team image to ui summon image
			summonsImg [i].sprite = player.GetComponent<Player> ().summons [i].GetComponent<Summon>().summonIcon;
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		HpBar ();
		Power ();
	//	Exp ();
	}
	//UI-food

	//UI-hp bar
	void HpBar(){
		float fillAmount=hpImg.fillAmount;//

		hp = player.GetComponent<Player> ().nowHp;
		float ratio = hp / maxHp;
		if (ratio < 0.0f) ratio = 0.0f;
		//hpImg.rectTransform.localScale = new Vector3 (ratio, 1, 1);

		hpImg.fillAmount = Mathf.Lerp (fillAmount,ratio,0.5f);
	}
	void Power(){
		float fillAmount=mpImg.fillAmount;//

		power = player.GetComponent<Player> ().nowPower;
		float ratio = power / maxPower;
		if (ratio < 0.0f) ratio = 0.0f;
		mpImg.fillAmount = Mathf.Lerp (fillAmount,ratio,0.1f);
	}
	void Summon(){
		// summon cd change sprite color?
	}
	void Exp(){
		exp = (int)player.GetComponent<Player> ().nowExp;
		expUI.text = "exp:" + exp;
		level = (int)player.GetComponent<Player> ().level;
		levelUI.text = "level:" + level;
	}


}
