using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Player : MonoBehaviour {
	internal Animator animator;
	Rigidbody body;
	Timer t1,t2,t3,t4;
	public float att,attCD,speed,hp,nowPower;
	public int level,nowExp,money;
	int exp;
    SpriteRenderer r;//skill3 animation color
    public int powerTime,maxPower;
	public GameObject []summons;
	internal bool isPot;//是否在鍋子旁
	internal float nowHp;
	public GameObject PlayerSkill1Obj,fakeSummonObj,PlayerSkill2Obj;
    public bool brushFlag; //韋傑加入 是否在草裡 在草裡為真
    public void SetbrushFlag(bool flag) { brushFlag = flag; }
	public GameObject SummonFX;//summon particle
    // Use this for initialization
	public LayerMask Mask;
	internal bool isAtt;
	private bool skill3_isOnce;
	NavMeshAgent agent;
	public enum GameState{//talk state
		start=0,GrassCollected,DonutMade,FirstBaoDefeated,SeeingSecBao,SecBaoDefeated,SecBaoFailed,SeeingFirstMonster,FirstMonsterDefeated,
		FirstMonsterFailed,IceCreamMade,FirstCombat,FirstCombatWin,FirstCombatFailed,idle
	}
	GameState gamestate;
	Fairy fairy;
	public float skill1CD;
	Timer tSkill1;
	public GameObject Bag;
	bool isOpen;

	public GameObject summonUI;
    void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		clickMove ();
		skill ();
		createPower (); 
		skillNo3 ();
		updateExp ();
    }
	void init(){//initialize
		t1 =gameObject.AddComponent<Timer>();
		t2= gameObject.AddComponent<Timer> ();
		t3= gameObject.AddComponent<Timer> ();
		t4= gameObject.AddComponent<Timer> ();
		animator = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
		nowHp = hp;
        PlayerSkill1Obj.SetActive(false);
		fakeSummonObj.SetActive (false);
		PlayerSkill2Obj.SetActive(false);

        r = gameObject.GetComponent<SpriteRenderer>();   //绑定到SpriteRenderer上 //skill3
		//exp&level
		level=1;
		setExperience ();
		isAtt = false;
		agent = GetComponent<NavMeshAgent> ();
		skill3_isOnce = true;
		gamestate = GameState.start;
		fairy = GameObject.FindObjectOfType<Fairy> ();
		fairy.changeTalkState ((int)gamestate);
		tSkill1 = gameObject.AddComponent<Timer> ();
		isOpen = false;
    }
	//click & move
	private bool moveState = false;
	private Vector3 target = new Vector3();
	void clickMove()
	{
		if (!isSkill) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity,Mask)) {
				if (Input.GetMouseButtonDown (1)) {
					moveState = true;
					target = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
					
				}
			}
			float step = speed * Time.deltaTime;
			Vector3 targetDir = target - transform.position;
			if (Vector3.Distance (transform.position, target) < 0.1f) { 
				moveState = false;
			}

            if (skillNo3MovingFlag==true) //技能3啟動關閉moveState
            {
                moveState = false;
            }
            if (moveState) {			
				agent.SetDestination (target);

				//transform.position = Vector3.MoveTowards (transform.position, target, step);
				//Debug.Log (targetDir);
				animator.SetFloat ("speedX", targetDir.x);
				animator.SetFloat ("speedY", targetDir.z);

				animator.SetTrigger ("move");//
			} else {
				animator.SetTrigger ("idle");//
			}
		} else {
            //is skill set ani
            if (moveState)
            {
                //行走時使用技能TARGET設成原地 動畫射程idle或使用技能
                target = transform.position;
                agent.SetDestination(target);
                animator.SetTrigger("idle");//
            }

        }
	}
	//animation Event
	public void attEvent(){
		animator.SetBool ("isAtt", false);
	}

	//cost power
	public void cost(int i){		
			nowPower -= i;	
	}
	//check power
	bool checkPower(int i){
		if (nowPower - i >= 0) {			
			return true;
		} else
			return false;
	}

	//create power
	void createPower(){
		if (t3.timer(powerTime)) {
			nowPower = (int)((nowPower * 1.1f) + 3);//need balance
			if (nowPower >= maxPower)
				nowPower = maxPower;
		}
	}
	bool checkSummon(int i){
		if (summons [i]) {
			return true;
		} else {
			Debug.Log ("summon No. " + i + " doesn't exist!");
			return false;
		}
	}
	//set exp
	void setExperience(){
		exp = level * level;//need to br improved

	}
	void updateExp(){
		
		if (nowExp >= exp) {
			level++;
			nowExp -= exp;
			setExperience ();
		}
	}
	public bool isSkill;//是否正在使用技能
	public bool isSkill3;
	void skill()//skill and summon
	{
		foreach (char c in Input.inputString)
		{
			switch (c)
			{//技能
				case' '://普攻
					animator.SetTrigger("attack");//攻擊動畫
					isAtt=true;
				break;     
				case 'Q':
				case 'q':             
					if (!isSkill && !isSkill3) {//如果沒再用其他技能的時候
						if (tSkill1.timer (skill1CD)) {
							//cd
							PlayerSkill1Obj.SetActive (true);
							isSkill = true;
						}
					}
                    break;
				case 'W':
				case 'w':
					if (!isSkill && !isSkill3) {
						isSkill = true;
						PlayerSkill2Obj.SetActive (true);
						animator.SetBool ("isSkill2",true);
					}
                    break;
				/*case 'E':
				case 'e':
					if (!isSkill) {
					//check power??
					//check cd
						//isSkill = true;
					 	//skillNo3 ();
						Debug.Log ("skill 3");
					}
                    break;*/
                //召喚獸
                case 'A'://NO0
				case'a':
					if (checkSummon (0)) {
						float sumCD= summons [0].GetComponent<Summon> ().summonCD;
						if (t1.timer (sumCD)){
							int sumCost= summons [0].GetComponent<Summon> ().summonCost;				
							if(checkPower (sumCost)){
							fakeSummonObj.GetComponent<SpriteRenderer>().sprite = summons [0].GetComponent<Summon>().summonIcon;//set sprite
							fakeSummonObj.GetComponent<FakeSummonObj>().Obj=summons[0];
							fakeSummonObj.SetActive(true);
							isSkill = true;

							}
						}					
					}
				break;

				// playerskill fakeSummon
			case 'B'://NO1
			case 'b':
				isOpen = !isOpen;
				Bag.SetActive (isOpen);
				break;/*
				case 'D'://NO2
				case 'd':
					if(checkSummon(2)){
						float sumCD= summons [2].GetComponent<Summon> ().summonCD;
						if (t3.timer (sumCD) && isPot){
							int sumCost= summons [2].GetComponent<Summon> ().summonCost;
							if(cost (sumCost)){
								Instantiate (summons[2], transform.position, transform.rotation);
							}
						}
					}      
                break;*/
			case '3':
				//get 1 image name
				//check cd

				string summonName3 =summonUI.GetComponentsInChildren<RawImage> () [2].texture.name;
				checkEatItem (summonName3);

				break;

			case '4':
				//get 1 image name
				//check cd

				string summonName4 =summonUI.GetComponentsInChildren<RawImage> () [3].texture.name;
				checkEatItem (summonName4);

				break;

			}

		}
	}
	void checkEatItem(string summonName){
		if (summonName == "Donut") {
			//check cd
			//check material
			MakeItem make = new MakeItem();
			SaveItem itemInfo = make.material (summonName);
			SaveItem.getInstance().check();
			bool isEnough=SaveItem.getInstance().checkDelete(itemInfo);
			if(isEnough){
				SaveItem.getInstance ().delete (itemInfo);
			}
			//set cd
			//set fairy
			//set talk
		} 
		else if (summonName == "baoIcon") {
			//summon
			//delete
			//cd

		}
		else if (summonName == "IceCream") {
		}
		else if (summonName == "HamEgg") {
			MakeItem make = new MakeItem();
			SaveItem [] itemInfo = make.material (summonName,1);
			SaveItem.getInstance().check();
			bool[] isEnough = new bool[itemInfo.Length];
			for (int i = 0; i < itemInfo.Length; i++) {
				isEnough[i] = SaveItem.getInstance ().checkDelete (itemInfo[i]);
			}
			if (isEnough [0] && isEnough [1]) {
				for (int i = 0; i < itemInfo.Length; i++) {
					SaveItem.getInstance ().delete (itemInfo [i]);
				}
			}
		}
		else if (summonName == "summon2Icon") {
		}
	}
	void OnTriggerStay(Collider col){
		if (col.tag == "pot") {
			isPot = true;
		}
		if (col.tag == "enemy") { 	

			if (t4.timer (col.GetComponent<Enemy> ().attCD)) {
				nowHp -= col.GetComponent<Enemy> ().att;//扣寫
				if (nowHp <= 0)
					nowHp = 0.0f;
			} 

		}
		if (col.tag == "enemyPao_1") { 	

			if (t4.timer (col.GetComponent<enemyPao_1> ().attCD)) {
				nowHp -= col.GetComponent<enemyPao_1> ().att;//扣寫
				if (nowHp <= 0)
					nowHp = 0.0f;
			} 

		}
	}
	void OnTriggerExit(Collider col){
		if (col.tag == "pot") {
			isPot = false;
		}
	}
	public void SummonAniEvent(){
		SummonFX.SetActive (false);
	}

	//skill3
	const float skillNo3MovingSpeed=5, skillNo3Range=400, skillNo3CDTime=10.0f, skillNo3ShieldTime = 3.0f,skillNo3BUFFTime=2.0f; //韋傑加入
	private float skillNo3CDTimmer=0.0f, skillNo3ShieldTimmer = 0.0f, skillNo3BUFFTimmer;//韋傑加入
	private bool skillNo3CDFlag=true, skillNo3MovingFlag=false , skillNo3ShieldFlag=false, skillNo3BUFFFlag=false; //韋傑加入
	private Vector3 skillNo3target = new Vector3();

    void skillNo3()//尚未考慮魔耗問題 本技能目前事無消耗的意思
    {
		//if (!isSkill3) {
		//	isSkill3 = true;

			if (skillNo3CDFlag) { // 加入flag cd之類的
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit,Mathf.Infinity,Mask)) {
					if (Input.GetKey (KeyCode.E) && hit.transform.gameObject.tag == "summon" && !skillNo3ShieldFlag && !skillNo3BUFFFlag) {//按下按鍵 且鼠標於友軍上 
						if ((transform.position.x - hit.transform.position.x) * (transform.position.x - hit.transform.position.x) + (transform.position.z - hit.transform.position.z) * (transform.position.z - hit.transform.position.z) < skillNo3Range) {//且於施放範圍內
							skillNo3MovingFlag = true;
							skillNo3target = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
						}
					}
				}
				float step = skillNo3MovingSpeed * Time.deltaTime;
				Vector3 targetDir = skillNo3target - transform.position;
				if (Vector3.Distance (transform.position, skillNo3target) < 0.1f) {//已移到目標身上
					skillNo3MovingFlag = false;//跳到有方身上的flag
					skill3_isOnce = true;
					if (skillNo3BUFFFlag == false) {
						skillNo3ShieldFlag = true;//開始計算舉盾cd 可不可以舉盾的flag
					}
				}
				if (skillNo3MovingFlag) {
                
					transform.position = Vector3.MoveTowards (transform.position, skillNo3target, step);

					if (skill3_isOnce) {
						skill3_isOnce = false;
						agent.SetDestination (skillNo3target);//

					}
					animator.SetBool ("isJump", true);
					isSkill3 = true;
					//切動畫援護動畫******************
				} 
				else {
					//保留 暫時用不到 看有沒有其它動畫
					animator.SetBool ("isJump", false);
					isSkill3 = false;

				}
				if (skillNo3ShieldFlag) { //可以舉盾
					skillNo3ShieldTimmer = skillNo3ShieldTimmer + Time.deltaTime;
					if (Input.GetKey (KeyCode.T)) {//3秒內重起按鍵舉盾
						skillNo3MovingFlag = false;
						skillNo3ShieldFlag = false;
						skillNo3ShieldTimmer = 0.0f;
						//切舉盾動畫之類的
						skillNo3BUFFFlag = true;
						Debug.Log ("舉盾");
						r.color = new Color (255f, 255f, 0f, 0f);

					} else if (skillNo3ShieldTimmer > skillNo3ShieldTime) {//3秒內沒舉盾開始計算cd
						skillNo3CDFlag = false;
						skillNo3ShieldFlag = false;
						skillNo3ShieldTimmer = 0.0f;
					}
				}
				if (skillNo3BUFFFlag) {//計時舉盾增防時間
					skillNo3BUFFTimmer = skillNo3BUFFTimmer + Time.deltaTime;
					if (skillNo3BUFFTimmer > skillNo3BUFFTime) {
						skillNo3BUFFTimmer = 0.0f;
						skillNo3BUFFFlag = false;
						skillNo3CDFlag = false;//開始算CD
						r.color = new Color (255f, 255f, 255f, 0f);
						//**********************切回原動畫
					}
				}
			} else { //計算CD
				skillNo3CDTimmer = skillNo3CDTimmer + Time.deltaTime;
				if (skillNo3CDTimmer > skillNo3CDTime) {
					skillNo3CDFlag = true;
					skillNo3CDTimmer = 0.0f;
				}
			}
	//	}
    }
	public void potCoverAniEvent(){
	}
}
