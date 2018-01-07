using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGItem : MonoBehaviour {
	public int hp;
	public GameObject[] items;
	internal int beAttNum;
	public GameObject HoverHp;
	Player player;
	public bool isSpecial;
	public enum GameState{//talk state
		start=0,GrassCollected,DonutMade,FirstBaoDefeated,SeeingSecBao,SecBaoDefeated,SecBaoFailed,SeeingFirstMonster,FirstMonsterDefeated,
		FirstMonsterFailed,IceCreamMade,FirstCombat,FirstCombatWin,FirstCombatFailed,idle
	}
	public GameState gamestate;

	// Use this for initialization
	void Start () {
		//player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player = GameObject.FindObjectOfType<Player>();
		beAttNum = 0;

	}
	
	// Update is called once per frame
	void Update () {
		//掉落物品有哪些 靠近可以被攻擊 寫條
		if(beAttNum==hp){
			FallProps ();
		}

	}
	void OnMouseOver(){
		//hover hp
		HoverHp.SetActive(true);
	}
	void OnMouseExit(){
		HoverHp.SetActive(false);
	}

	void FallProps(){			//掉落 算機率
		for(int i = 0; i < items.Length; i++){
			
			Instantiate (items [i], transform.position+new Vector3(0,3,0), transform.rotation);
	
		}
		if (isSpecial) {
			Fairy fairy = GameObject.FindObjectOfType<Fairy> ();
			fairy.changeTalkState ((int)gamestate);
		}
		Destroy (this.gameObject);
	}
	void OnTriggerStay(Collider col){
		if (player.isAtt) {
			beAttNum++;
			player.isAtt = false;
		}
	}
}
