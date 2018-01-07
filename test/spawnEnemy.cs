using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class spawnEnemy : MonoBehaviour {
	public GameObject enemy;
	public bool isDead;
	public int total;
	public int now;
	int i;
	public Text enemyTxt;
	Text txt;
	// Use this for initialization
	void Start () {
		isDead = false;
		txt = enemyTxt.GetComponent<Text>();
		now = 0;
		i = 0;
		total=0;
		InvokeRepeating("counter", 1.0f, 5.0f);//1秒開始 .1秒叫一次
		txt.text = "000";
	}
	void create(int n){//create n enemy
		for (int i = 0; i < n; i++) {
			Instantiate (enemy, new Vector3 (transform.position.x+Random.Range(-3,3),
				transform.position.y, transform.position.z+Random.Range(-3,3)), transform.rotation);    
		}
	}
	/*void create(int n,){//create n enemy
		for (int i = 0; i < n; i++) {
			Instantiate (enemy, new Vector3 (transform.position.x, transform.position.y, Random.Range (-3,6)), transform.rotation);    
		}
	}*/
	void counter(){//create n times
		
		if (i<3) {
			create (now);//2+4
			now+=2;
		}
		i++;
	}
	// Update is called once per frame
	void Update(){
		total=GameObject.FindGameObjectsWithTag ("right").Length;
		txt.text =  total.ToString();//文字顯示
	}
	void OnTriggerEnter(Collider col){
		if (col.tag == "ch")
			create (Random.Range (1, 3));
	}
}
