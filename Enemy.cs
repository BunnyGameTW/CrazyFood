using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	Timer t1,t2;
	Rigidbody body;
	Animator animator;
	GameObject player;
	public float att,attCD,speed,hp,floorY,limitDis;
	internal float nowHp;
	public int exp;
    ////韋傑加入
    private bool Skill4TrrifyFlag = false;
    private float Skill4TrrifyCCTimeer = 0.0f;
    private Vector3 Unitvector;
    private const float RunawaySpeed= 0.1f ,Skill4TrrifyCCTime=2.0f;
    //韋傑加入end
	NavMeshAgent agent;//ai
    
    // Use this for initialization
    void Start () {
		nowHp = hp;
		t1 = gameObject.AddComponent<Timer> ();
		t2 = gameObject.AddComponent<Timer> ();
		animator = GetComponent<Animator>();
		body=GetComponent<Rigidbody>();
		player = GameObject.Find("Player");

		InvokeRepeating("AI", 1.0f, 0.1f);//1秒開始 .1秒叫一次
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();//ai

        //韋傑加入
        //委派 訂閱subscribe 技能4的事件 傳說中的+= Comble 66
        Skill4CCRange.sendSkill4Range1Event += Skill4EnterEvent;
        Skill4CCRange.sendSkill4Range1Event += Skill4StayEvent;
        Skill4CCRange.sendSkill4Range1Event += Skill4ExitEvent;

        Skill4ExplodeRange.sendSkill4Range2Event += Skill4ExplodeEnterEvent;
        //Skill4ExplodeRange.sendSkill4Range2Event +=;
        //Skill4ExplodeRange.sendSkill4Range2Event +=;
        //韋傑加入end

    }


    ////韋傑加入
    private void Skill4EnterEvent(GameObject obj, string state, Collider collider)
    {
        //若不是Enter則立刻結束Method
        if (state.Equals("Enter") == false)
            return;
        Debug.Log("obj:" + obj.name + ", 撞到了:" + collider.name); //name : name of object
        Skill4TrrifyFlag = true;
    }

    private void Skill4StayEvent(GameObject obj, string state, Collider collider)
    {
        //若不是Stay則立刻結束Method
        if (state.Equals("Stay") == false)
            return;
        Debug.Log("obj:" + obj.name + ", 停留在:" + collider.name);
    }

    private void Skill4ExitEvent(GameObject obj, string state, Collider collider)
    {
        //若不是Exit則立刻結束Method
        if (state.Equals("Exit") == false)
            return;
        Debug.Log("obj:" + obj.name + ", 離開了:" + collider.name);
    }

    private void Skill4ExplodeEnterEvent(GameObject obj, string state, Collider collider)
    {
        if (state.Equals("Enter") == false)
            return;
        Debug.Log("爆炸發生obj:" + obj.name + ", 撞到了:" + collider.name); //name : name of object
        nowHp = nowHp - 9;//扣血
    }
    //韋傑加入end


    // Update is called once per frame
    void Update () {
        TerrifyState(Skill4TrrifyFlag);
        if(!Skill4TrrifyFlag) //wj改動
            state ();

    }
	bool once=true;
	void state(){
		if (nowHp <= 0) {
			animator.SetBool ("isAlive", false);
			CancelInvoke ("AI");
			this.GetComponent<BoxCollider>().center = new Vector3(0, -10, 0);//觸發ontriggerExit
			Destroy (this.gameObject,3.0f);
			if (once) {
				once = false;
				player.GetComponent<Player> ().nowExp += exp;
				agent.enabled = false;
			}
		}

	}

	protected GameObject target;
	public GameObject FindClosestEnemy(string enemyTag)
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(enemyTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos)
		{
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
	void Direction(int i){
		if (i == 0)
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		else
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
	}
		void AI(){
			target = FindClosestEnemy ("summon");//標籤名要設定//distance??
			if (target == null) {
				target = player;
			} 
			Vector3 diff =player.transform.position-transform.position;//優先鎖定玩家
			Vector3 diff2 =target.transform.position-transform.position;
			float playerDistance = diff.sqrMagnitude;
			float distance = diff2.sqrMagnitude;
			if (playerDistance <= distance){
				target = player;
					}
			//limit Distance
			Vector3 offset =target.transform.position-transform.position;
			float sqrLen = offset.sqrMagnitude;
			if (sqrLen > limitDis * limitDis) target = null;

			if (!animator.GetBool ("isAtt")) {
				if (target == null) {
					animator.SetTrigger ("idle");
					return;
				}
				Vector3 targetPos= target.transform.position ;
				targetPos.y = floorY;
				
				float step = speed * Time.deltaTime;
			agent.SetDestination (target.transform.position);
           // Debug.Log(target);
			//	transform.position = Vector3.MoveTowards(transform.position,targetPos,step);
				Vector3 dis= target.transform.position - transform.position ;

				if (dis.x > 0)	Direction (0);//change dir
				else	Direction (1);
				animator.SetTrigger ("move");

			}
		}
	void OnTriggerStay(Collider col) {
      //  col.
		if (col.tag == "summon" || col.tag == "Player") {   
			animator.SetBool("isAtt",true);
		}
		if (col.tag == "summon" && t1.timer (col.GetComponent<Summon> ().attCD)) {   
			nowHp -= col.GetComponent<Summon> ().att;//attack by summon
		} 
		else if (col.tag == "Player" && t2.timer (col.GetComponentInParent<Player> ().attCD)) {
			nowHp -= col.GetComponentInParent<Player> ().att;//attack by character

		}
		if (col.tag == "potCover") {
		
			Vector3 _offset = transform.position - col.transform.position;
			Vector3 _UnitOffset=_offset/Mathf.Sqrt((Mathf.Pow(_offset.x,2)+Mathf.Pow(_offset.y,2)+Mathf.Pow(_offset.z,2)));
			Debug.Log("offset in Enemy: "+ _offset);
			_UnitOffset.y = 0;
			transform.position += new Vector3 (_UnitOffset.x, _UnitOffset.y, _UnitOffset.z);
			//Debug.Log("potCoverPos: "+ col.transform.position);
		}

	}
	void OnTriggerExit(Collider col)
	{
		if (col.tag == "summon"|| col.tag=="Player")
		{   
			animator.SetBool("isAtt", false);
		}

	}

    //wj
    void TerrifyState(bool flag)
    {
        if (flag)
        {
            Unitvector = transform.position - player.transform.position;
            Unitvector.x = Unitvector.x / Vector3.Distance(transform.position, player.transform.position);
            Unitvector.z = Unitvector.z / Vector3.Distance(transform.position, player.transform.position);//計算單位向亮
            Vector3 step;
            step.x = transform.position.x * Unitvector.x;//V3沒多載*   極度差評爛引擎
            step.y = 0.0f;
            step.z = transform.position.z * Unitvector.z;
            transform.position = transform.position+step*Time.deltaTime *RunawaySpeed;
            Skill4TrrifyCCTimeer = Skill4TrrifyCCTimeer + Time.deltaTime;
            if (Skill4TrrifyCCTimeer>Skill4TrrifyCCTime)
            {
                Skill4TrrifyFlag = false;
                Skill4TrrifyCCTimeer = 0.0f; //歸0
            }

        }
        else
            return;
    }
        

    //end of wj

}
