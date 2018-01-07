using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveItem  {
    static SaveItem saveItem;
    public string species;
    public int num; 
    public List<SaveItem> _listData;


    public static SaveItem getInstance()
    {
        if (saveItem == null)
        {
            saveItem = new SaveItem();

        }
        return saveItem;

    }
    
    public void check() {
        if (_listData == null)
        {
            _listData = new List<SaveItem>();
        }
    
    }
    public void load() {
    }
	void Upate(){
		foreach (SaveItem _savedItem in _listData) {//print list data
			//Debug.Log("species: " + _savedItem.species + " \\ num: " + _savedItem.num);

		}
	}
    public  void save(SaveItem ItemInfo) {
        bool isInList=false;
        foreach (SaveItem _savedItem in _listData) {
            if (ItemInfo.species == _savedItem.species) {//in list
                _savedItem.num++;
                isInList = true;
                break;
            }
        }
        if(!isInList)  _listData.Add(ItemInfo);

        foreach (SaveItem _savedItem in _listData) {//print list data
            Debug.Log("species: " + _savedItem.species + " \\ num: " + _savedItem.num);
        }
        //Debug.Log(_listData.IndexOf(ItemInfo));
        //Debug.Log(_listData[_listData.IndexOf(ItemInfo)].species);
        //Debug.Log(_listData[_listData.IndexOf(ItemInfo)].num);
       
    }
	public bool checkDelete(SaveItem ItemInfo){
		foreach (SaveItem _savedItem in _listData) {
			if (ItemInfo.species == _savedItem.species) {//in list
				if (_savedItem.num - ItemInfo.num >= 0){
					return true;
				} 
				else {//unenough
					Debug.Log (_savedItem.species + " is not enough");
					return false;
				}
			} 
				
		}
		return false;
	
	}
	public void delete(SaveItem ItemInfo){
		//bool isInList = false;
		foreach (SaveItem _savedItem in _listData) {
			if (ItemInfo.species == _savedItem.species) {//in list
				if (_savedItem.num - ItemInfo.num > 0) {
					_savedItem.num -= ItemInfo.num;//&& num is enough
				} else if (_savedItem.num - ItemInfo.num == 0) {
					_listData.Remove (_savedItem);// ==0 remove it
				} else {//unenough
					Debug.Log (_savedItem.species + " is not enough");
				}
				break;
				Debug.Log("species: " + _savedItem.species + " \\ num: " + _savedItem.num);

			} 

		}
		//if(!isInList)  _listData.Add(ItemInfo);

		foreach (SaveItem _savedItem in _listData) {//print list data
		}

	}
}
