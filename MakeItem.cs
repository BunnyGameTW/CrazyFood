using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItem  {
	// Use this for initialization
	SaveItem [] items;
	public SaveItem material(string name){
		if (name == "Donut") {
			SaveItem item = new SaveItem ();
			item.species = "Carrot";
			item.num = 1;
			return item;
		}
		return null;
	}
	public SaveItem[] material(string name,int i){
		

		if (name == "baoIcon") {
			SaveItem item1=new SaveItem();
			SaveItem item2=new SaveItem();
			item1.species = "Meat";
			item1.num = 1;
			item2.species = "Mushroom";
			item2.num = 1;
			items = new SaveItem[2];
			items [0] = item1;
			items [1] = item2;

			return items;
		}
		else if(name == "summon2Icon") {
			SaveItem item1=new SaveItem();
			SaveItem item2=new SaveItem();
			item1.species = "Meat";
			item1.num = 1;
			item2.species = "Onion";
			item2.num = 1;
			items = new SaveItem[2];
			items [0] = item1;
			items [1] = item2;


			return items;
		}
		else if(name == "IceCream") {
			SaveItem item1=new SaveItem();
			SaveItem item2=new SaveItem();


			item1.species = "Cheese";
			item1.num = 1;
			item2.species = "Milk";
			item2.num = 1;
			items = new SaveItem[2];
			items [0] = item1;
			items [1] = item2;

		
			return items;
		}
		else if(name == "HamEgg") {
			
		
			SaveItem item1=new SaveItem();
			SaveItem item2=new SaveItem();


			item1.species = "Bread";
			item1.num = 1;
			item2.species = "Tomato";
			item2.num = 1;
			items = new SaveItem[2];
			items [0] = item1;
			items [1] = item2;
			return items;
		}
		return null;
	}
}
