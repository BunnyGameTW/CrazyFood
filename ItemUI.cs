using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour {
	//struct items{
	int Num;
		RawImage [] itemImg;
		Text [] itemNum;
	public Texture[] items;
	//}
	//public items [] itemsUI;
	// Use this for initialization
	void Start () {
		SaveItem.getInstance().check();
		itemImg = GetComponentsInChildren<RawImage> ();
		itemNum = GetComponentsInChildren<Text> ();
		for (int i = 0; i < itemImg.Length; i++) {
			itemImg [i].texture = items[14];
			itemNum [i].text = "";
		}

	}
	
	// Update is called once per frame
	void Update () {
		updateItem ();
	}
	void updateItem(){
		List<SaveItem> itemList = SaveItem.getInstance()._listData;
		itemImg = GetComponentsInChildren<RawImage> ();
		itemNum = GetComponentsInChildren<Text> ();
		//Debug.Log (itemImg);
		//itemImg=new Image[itemList.Count];//Debug.Log("species: " + _savedItem.species + " \\ num: " + _savedItem.num);
	/*	for(int i=0;i<itemImg.Length;i++){
			itemImg[i].sprite=

		}*/
		Num = 0;
		foreach (SaveItem _savedItem in itemList) {//print list data
			for(int i=0; i<items.Length;i++){
				if (_savedItem.species == items [i].name) {
					itemImg [Num].texture = items [i];
					itemNum [Num].text = _savedItem.num.ToString();

					break;
				}

			}

			//itemImg [Num].texture = items [0].name; _savedItem.species;
			Num++;
		}
		//if (Num == 0) {
			for (int i = Num; i < itemImg.Length; i++) {
				itemImg [i].texture = items[14];
				itemNum [i].text = "";
			}
		//}
		//SaveItem.getInstance().check();

	}
}
