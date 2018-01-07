using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyPao_1 : MonoBehaviour
{
    Timer t1, t2;
    Rigidbody body;
    Animator animator;
    GameObject player;
    public float att, attCD, speed, hp, floorY, limitDis;
    internal float nowHp;
    ////韋傑加入
    private bool Skill4TrrifyFlag = false, StratToCountFlag = false;  //Skill4TrrifyFlag:技能4恐懼flag
    private float Skill4TrrifyCCTimeer = 0.0f;
    private Vector3 Unitvector;
    private const float RunawaySpeed = 0.1f, Skill4TrrifyCCTime = 2.0f;
    private const float AwakeTime = 2.0f; //被驚醒的時間
    private float AwakeTimmer ; //被驚醒的計時
    private bool IsAwakeFlag;
    public float AwakeRange;
    Player PlayerClass;

    private Transform Main; //抓子物件
    private Transform SpriteOBJ; //抓子物件
    private float ColorG, ColorB;
    private Animator SpriteEffectAni;
    //韋傑加入end
    NavMeshAgent agent;//ai

    // Use this for initialization
    void Start()
    {
        ////韋傑加入
        IsAwakeFlag = false;
        AwakeTimmer = 0.0f;
        Main = transform.GetChild(0); //
        SpriteOBJ = Main.transform.GetChild(1); //抓子物件 (圖) 用於改變顏色
        ColorG = ColorB = 255;
        SpriteEffectAni=SpriteOBJ.GetComponent<Animator>();
        //韋傑加入end
        nowHp = hp;
        t1 = gameObject.AddComponent<Timer>();
        t2 = gameObject.AddComponent<Timer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerClass = player.GetComponent<Player>();
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
        //Skill4TrrifyFlag = true;
    }

    private void Skill4ExitEvent(GameObject obj, string state, Collider collider)
    {
        //若不是Exit則立刻結束Method
        if (state.Equals("Exit") == false)
            return;
        Debug.Log("obj:" + obj.name + ", 離開了:" + collider.name);
        StratToCountFlag = true;
    }

    private void Skill4ExplodeEnterEvent(GameObject obj, string state, Collider collider)
    {
        if (state.Equals("Enter") == false)
            return;
        Debug.Log("爆炸發生obj:" + obj.name + ", 撞到了:" + collider.name); //name : name of object
        nowHp = nowHp - 9;//扣血 先隨便便扣
    }
    //韋傑加入end


    // Update is called once per frame
    void Update()
    {
        CountAwaketime();
        TerrifyState(Skill4TrrifyFlag);
        if (!Skill4TrrifyFlag) //wj改動 如果沒被恐懼則執行正常狀態
            state();
    }

    void state()
    {
        if (nowHp <= 0)
        {
            animator.SetBool("isAlive", false);
            CancelInvoke("AI");
            this.GetComponent<BoxCollider>().center = new Vector3(0, -10, 0);//觸發ontriggerExit
            Destroy(this.gameObject, 3.0f);
        }
        /*if (Vector3.Distance (player.transform.position, transform.position) > 10.0f)//player skill1
			rig.isKinematic =true;//skill*/
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

    void Direction(int i)
    {
        if (i == 0)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = false;
    }
    void AI()
    {
        if (IsAwakeFlag==false) //還在睡
        {
            animator.SetTrigger("idle");
        }
        else if (IsAwakeFlag == true)
        {
            target = FindClosestEnemy("summon");//標籤名要設定//distance??
            if (target == null)
            {
                target = player;
            }
            Vector3 diff = player.transform.position - transform.position;//優先鎖定玩家
            Vector3 diff2 = target.transform.position - transform.position;
            float playerDistance = diff.sqrMagnitude;
            float distance = diff2.sqrMagnitude;
            if (playerDistance <= distance)
            {
                target = player;
            }
            //limit Distance
            Vector3 offset = target.transform.position - transform.position;
            float sqrLen = offset.sqrMagnitude;
            if (sqrLen > limitDis * limitDis) target = null;

            if (!animator.GetBool("isAtt"))
            {
                if (target == null)
                {
                    animator.SetTrigger("idle");
                    return;
                }
                Vector3 targetPos = target.transform.position;
                targetPos.y = floorY;

                float step = speed * Time.deltaTime;
                agent.SetDestination(target.transform.position);
                //	transform.position = Vector3.MoveTowards(transform.position,targetPos,step);
                Vector3 dis = target.transform.position - transform.position;

                if (dis.x > 0) Direction(0);//change dir
                else Direction(1);
                animator.SetTrigger("move");

            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (IsAwakeFlag)
        {
            if (col.tag == "summon" || col.tag == "Player")
            {
                animator.SetBool("isAtt", true);
            }
            if (col.tag == "summon" && t1.timer(col.GetComponent<Summon>().attCD))
            {
                nowHp -= col.GetComponent<Summon>().att;//attack by summon
            }
            else if (col.tag == "Player" && t2.timer(col.GetComponent<Player>().attCD))
            {
                nowHp -= col.GetComponent<Player>().att;//attack by character

            }
        }
        /*if (col.tag == "ch") {
			if (player.GetComponent<me> ().skill1) {
				rig.isKinematic =false;//
				rig.AddForce((transform.position.x-player.transform.position.x),0,(transform.position.z-player.transform.position.z),ForceMode.Impulse);
			}
		}*/
    }
    void OnTriggerExit(Collider col)
    {
        if (IsAwakeFlag)
        {
            if (col.tag == "summon" || col.tag == "Player")
            {
                animator.SetBool("isAtt", false);
            }
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
            transform.position = transform.position + step * Time.deltaTime * RunawaySpeed;
            if (StratToCountFlag)
            {
                Skill4TrrifyCCTimeer = Skill4TrrifyCCTimeer + Time.deltaTime;
                if (Skill4TrrifyCCTimeer > Skill4TrrifyCCTime)
                {
                    Skill4TrrifyFlag = false;
                    Skill4TrrifyCCTimeer = 0.0f; //歸0
                    StratToCountFlag = false; //關
                }
            }
        }
        else
            return;
    }

    void CountAwaketime()  //計算是否睡醒
    {
        Vector3 offset = player.transform.position - transform.position; //算距離
        float sqrLen = offset.sqrMagnitude;
        effectAniState();

        if (sqrLen < AwakeRange* AwakeRange && PlayerClass.brushFlag==false) //在距離內 且不在草裡 會被驚醒
        {
            AwakeTimmer = Time.deltaTime+ AwakeTimmer;
            //改顏色
            if (AwakeTimmer> 0 && AwakeTimmer <= AwakeTime / 2)
            {
                ColorB = 256 - 256 * AwakeTimmer;  //因為是一秒
                //SpriteOBJ.transform.GetComponent<SpriteRenderer>().color = new Color(256/256, 256/256,125/256, 1);
                Debug.Log("BBB  "+ColorB);
                SpriteOBJ.transform.GetComponent<SpriteRenderer>().color = new Color(256 / 256, 256/256, ColorB / 256, 1);
            }

            else if (AwakeTimmer> AwakeTime/2 && AwakeTimmer <= AwakeTime )
            {
                ColorG = 255 - 255 * (AwakeTimmer-1);  //因為是一秒
                Debug.Log("GGG  " + ColorG);
                //SpriteOBJ.transform.GetComponent<SpriteRenderer>().color = new Color(256/256, 125/256, 0, 1);
                SpriteOBJ.transform.GetComponent<SpriteRenderer>().color = new Color(256/256, ColorG/256, ColorB/256, 1);
            }
            //
            else if (AwakeTime < AwakeTimmer)
            {
                IsAwakeFlag = true;
                
            }
        }
    }

    void effectAniState()
    {
        if (AwakeTimmer == 0.0f) SpriteEffectAni.SetBool("IsSleep",true);
        if (AwakeTimmer > 0.0f && AwakeTimmer <= AwakeTime / 2)
        {
            SpriteEffectAni.SetBool("IsSleep",false);
        }

        else if (AwakeTimmer > AwakeTime / 2 && AwakeTimmer <= AwakeTime)
        {
           
        }
        
        else if (AwakeTime < AwakeTimmer)
        {
            
        }

    }
    //end of wj
}
