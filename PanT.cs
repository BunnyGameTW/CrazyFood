using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanT : MonoBehaviour {

    // Use this for initialization
    Vector3 OriginV3,force;
    Rigidbody rb;
    public float RX, RY, RZ;
    bool FlyingFlag,GroundFlag;
    private float PanRotateTimer, PanRotateTime ;
    Renderer ren;
    Color col;
    public Color TrunColor;
    private Animator PanAni;
    Renderer FPan;   //HAS A FakePan
    public GameObject Pan_fake;
    private Transform ParticleEffect_1;
	//
	public Vector3 potDir;
    void Start ()
    {
        //
        //Pan_fake.SetActive (true);
        transform.rotation = Quaternion.Euler(90f, 0, 180.0f);
        ParticleEffect_1 = transform.GetChild(0);
        ParticleEffect_1.gameObject.SetActive(false);
        OriginV3 =transform.position;
        rb = this.gameObject.GetComponent<Rigidbody>();
        PanAni = GetComponent<Animator>();
        ren = GetComponent<Renderer>();  //self
        col = ren.material.GetColor("_Color");  //self
        Pan_fake = GameObject.Find("pan_Fake");
		//
		Pan_fake.GetComponent<MeshRenderer>().enabled=true;

        FPan = Pan_fake.GetComponent<Renderer>();
        FPan.enabled=false;
        //rb.isKinematic=true;
      //  force = new Vector3(500, 500, 0);
        FlyingFlag = false;
        GroundFlag = false;
        PanRotateTime = 1.0f;
        rb.isKinematic = true; //關掉物理 
        rb.useGravity = false; //關掉重力
        PanAni.SetBool("IsGround", false);
        //
		rb.isKinematic = false; //打開物理 
		rb.useGravity = true; //打開重力
		// rb.AddForce(force);
		rb.AddForce (potDir);
		FlyingFlag = true; //設為飛行狀態
		PanRotateTimer = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
       /* if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = OriginV3;
            FlyingFlag = false;
            rb.useGravity = false; //關掉重力
            PanAni.SetBool("IsGround", false);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            rb.isKinematic = false; //打開物理 
            rb.useGravity = true; //打開重力
           // rb.AddForce(force);
			rb.AddForce (potDir,ForceMode.Impulse);
            FlyingFlag = true; //設為飛行狀態
            PanRotateTimer = 0.0f;
        }*/

        Flying(FlyingFlag);
        if (transform.localScale.x > 12)   //關掉提示
        {
            FPan.enabled = false;
        }
    }
       
    private void OnCollisionEnter(Collision collision)
    {
        if (FlyingFlag)
        {
            if (collision.gameObject.tag == "floor" && PanRotateTimer > PanRotateTime)  //判斷是否著地
            {
                FlyingFlag = false;
                rb.isKinematic = true; //關掉物理 
                ParticleEffect_1.gameObject.SetActive(true);
             //   Debug.Log("touch G");
                PanAni.SetBool("IsGround", true);
                FPan.enabled = true;
                Pan_fake.transform.position = this.transform.position;
                Pan_fake.transform.rotation = this.transform.rotation;
                GroundFlag = true;
                
            }
        }
    }

    void Flying(bool flag)
    {
        if (flag == false) return;
        else
        {
            PanAni.SetBool("IsGround", false);
            PanRotateTimer = PanRotateTimer + Time.deltaTime;
            //Debug.Log(PanRotateTimer);
            transform.Rotate(RX * Time.deltaTime, RY * Time.deltaTime, RZ * Time.deltaTime);
            ren.material.SetColor("_Color", Color.Lerp(col, TrunColor, PanRotateTimer));
            //Debug.Log(ren.material.GetColor("_Color"));
        }
    }

    void OnGround()
    {
		Pan_fake.GetComponent<MeshRenderer>().enabled=false;
		Destroy (this.gameObject);
		//Debug.Log("Pot Cover Delete");

    }
}
