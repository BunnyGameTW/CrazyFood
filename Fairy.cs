using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour {

	public GameObject talkUI;
	internal bool isTalk;
	string nowString;
	int talkState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        followPlayer();

    }
	public void changeTalkState(int i){
		talkState = i;
		talk (talkState);
	}
	void OnMouseOver(){
		//Debug.Log ("+++");

	}
	void OnMouseDown(){
		//Debug.Log ("---");

		//talkUI.SetActive (true);
		talk(talkState);
	}
	void talk(int i)
	{

		Talk talky = talkUI.GetComponentInChildren<Talk>();
		int storySize = CSV.getInstance().arrayData[i].Length;

		talky.SetStorySize(storySize - 1);
		string[] talkStory = talky.story;
		for (int j = 1; j < storySize; j++)
		{//讀入第N段文字
			talkStory[j - 1] = CSV.getInstance().arrayData[i][j];
			Debug.Log(talkStory[j - 1]);
		}
		talkUI.SetActive(true);
		isTalk = true;
		nowString = "";

	}
    void followPlayer() {
        Vector3 target = FindObjectOfType<Player>().transform.position;
        target.y = transform.position.y;
        transform.position=Vector3.MoveTowards(transform.position,target,1);
    }
}
