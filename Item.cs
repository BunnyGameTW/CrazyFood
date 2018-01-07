using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item : MonoBehaviour {
	public float probability;//機率暫時都1
    public string species;
    public int num;
	Player player;
	Rigidbody body;
	bool isFloor;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		body.AddForce (Vector3.up*300);//加力
		player = GameObject.FindWithTag ("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void pickUp(){
		

	}
	void OnCollisionStay(Collision col){
		if (col.gameObject.name == "floor")
			isFloor = true;
	}
	void OnTriggerStay(Collider col){
		if (isFloor) {
			if (col.tag=="Player") {//剪取
                //音效
                //讀黨 檢查物品種類 數量
                SaveItem itemInfo = new SaveItem();
                itemInfo.num = num;
                itemInfo.species = species;
                SaveItem.getInstance().check();
                SaveItem.getInstance().save(itemInfo);

				//存檔 更新物品列表 
                //物品種類 數量
				Destroy (this.gameObject);


				
			}
		}
	}
}
