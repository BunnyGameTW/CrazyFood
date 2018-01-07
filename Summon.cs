using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Summon : MonoBehaviour {

	Timer t1;
	public float hp,speed, att,attCD,floorY,limitDis;
	public float summonCD;
	public int summonCost;
	public Sprite summonIcon;
	internal float nowHp;
	Animator animator;
	Rigidbody body;
	NavMeshAgent agent;
	public float PlayerDis;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		nowHp = hp;
		t1 = gameObject.AddComponent<Timer>();
		animator = GetComponent<Animator>();
		InvokeRepeating("AI", 1.0f, 0.2f);//1秒開始 .1秒叫一次
		body = GetComponent<Rigidbody> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();//ai
    }

    // Update is called once per frame
    void Update () {

		state ();

	}
	void state(){
		if (nowHp <= 0) {
			animator.SetBool ("isAlive", false);
			CancelInvoke ("AI");
			this.GetComponent<BoxCollider>().center = new Vector3(0, -10, 0);//觸發ontriggerExit
			//閃動效果?
			Destroy (this.gameObject,2.0f);

		}
	}
	void Direction(int i){
		if (i == 0)
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		else
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
	}
	void followPlayer(){
		float dis = Vector3.Distance(transform.position ,Player.transform.position);
		if (dis > PlayerDis) {
		//follow player
			agent.SetDestination(Player.transform.position);
			//若在戰鬥中則先完成這次動作再跟隨
			//if()
			//跟隨目標 敵人 空 或玩家
		}
	}
	protected GameObject target;
	//protected bool isAtt;
	// Use this for initialization

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
	void AI(){
		target = FindClosestEnemy("enemy");//標籤名要設定//加上距離限制?
		/*if (target == null) {
			animator.SetTrigger ("idle");
			return;
		} */
		if(target!=null){
			Vector3 offset =target.transform.position-transform.position;
			float sqrLen = offset.sqrMagnitude;
			if (sqrLen > limitDis * limitDis) target = null;

		}
        
       if (isAttract && thisTurnEnd || isAttract && !attStart)//聚集到召集物
       {
			target = FindClosestEnemy("playerSkill1");//標籤名要設定

			if (once) {
				agent.SetDestination (target.transform.position);
				once = false;
			}
           
		}
		if (!animator.GetBool ("isAtt")) {//not in att
			if (target == null) {
				animator.SetTrigger ("idle");
				return;
			}
			Vector3 targetPos= target.transform.position ;
			targetPos.y = floorY;

			float step = speed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position,targetPos,step);
			agent.SetDestination (target.transform.position);

			Vector3 dis= target.transform.position - transform.position ;
            //左右動畫沒調

			if (dis.x > 0)	Direction (0);
			else	Direction (1);
			animator.SetTrigger ("move");
		}
      
		//Debug.Log("Target:"+target);

    }
    bool thisTurnEnd;
    public void aniAttEvent()
    {
        if(isAttract)
            thisTurnEnd = true;
        animator.SetBool("isAtt", false);
        attStart = false;
        Debug.Log("thisTurn" + thisTurnEnd);
        //Debug.Log("isAttrect" + isAttract);
        Debug.Log ("aniAttEvent" );
    }
	bool once=true;
    bool isAttract=false,attStart;
    //Vector3 attractPos;
	Timer t2;
	public GameObject potCover;
	bool isPotCover,isStop;
	public float potCoverDis;
	Tween tweener;
	void OnTriggerEnter(Collider col){
		
	}
    void OnTriggerStay(Collider col) {
		if (col.tag == "enemy")
		{
            if (!thisTurnEnd)
            {
                animator.SetBool("isAtt", true);
                attStart = true;
            }

        }
		if (col.tag == "enemy" && t1.timer(col.GetComponent<Enemy>().attCD))
		{   
			nowHp-=col.GetComponent<Enemy>().att;//扣寫
		}
        if (col.tag == "playerSkill1") {          
            isAttract = true;       
        }
		if (col.tag == "potCover") {//bug
			Vector3 _offset = transform.position - col.transform.position;
			Vector3 _UnitOffset=_offset/Mathf.Sqrt((Mathf.Pow(_offset.x,2)+Mathf.Pow(_offset.y,2)+Mathf.Pow(_offset.z,2)));
			Debug.Log("offset in summon: "+ _offset);
			_UnitOffset.y = 0;
			transform.position += new Vector3 (_UnitOffset.x, _UnitOffset.y, _UnitOffset.z);
			agent.SetDestination (transform.position);
			isPotCover =true;
			Debug.Log("potCoverPos: "+ col.transform.position);

		}
    }
	void OnTriggerExit(Collider col)
	{
		if (col.tag == "enemy") {//enemy die or exit
			//animator.SetBool ("isAtt", false);
		}
        if (col.tag == "playerSkill1")
        {
            isAttract = false;
            Debug.Log("exit");

            once = true;
            thisTurnEnd = false;

        }
    }
}
