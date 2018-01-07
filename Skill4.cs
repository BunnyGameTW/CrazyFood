using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4 : MonoBehaviour {

    private const float Skill4CC_CD=2, Skill4Explode_CD=10, Skill4Explode_AniTime=2.0f;  //Skill4CC_CD 恐懼CD   Skill4Explode_CD 過熱後長CD
    private const int Skill4overheatSpeed = 5, Skill4ColdSpeed=8;
    private bool Skill4CC_flag=true, Skill4Explode_flag=false, Skill4Explode_AniFlag=false, Skill4CC_firstFrameFlag=true;
    private float Skill4CDTimmer1=0, Skill4Explode_AniTimeer=0.0f; //init
    private float Skill4overheat = 0;
    GameObject Skill4Range1, Skill4Range2, Playerr;
    //Transform Child;

    void Start () {
        Skill4Range1=GameObject.Find("skill4Range1");
        Playerr= GameObject.Find("Player");
        Skill4Range1.SetActive(false);
        Skill4Range2 = GameObject.Find("skill4Range2");
        Skill4Range2.SetActive(false);
        //Child = GetComponentInChildren<Transform>(); 
        //Child.gameObject.SetActive(false);
    }

    void Skill4CCCD_counter()//計時 Skill4CC_CD 恐懼CD 
    {
        if (Skill4CC_flag == false)
        {
            Skill4CDTimmer1 = Skill4CDTimmer1 + Time.deltaTime;
            Debug.Log("Short sec" + Skill4CDTimmer1);
            if (Skill4CDTimmer1 > Skill4CC_CD)
            {
                Skill4CDTimmer1 = 0.0f;
                Skill4CC_flag = true;
                Skill4CC_firstFrameFlag = true; //開啟first frame
            }
        }
        else if (Skill4Explode_flag == true) { //計時爆炸
            Skill4CDTimmer1 = Skill4CDTimmer1 + Time.deltaTime;
            Debug.Log("Long sec" + Skill4CDTimmer1);
            if (Skill4CDTimmer1 > Skill4Explode_CD)
            {
                Skill4CDTimmer1 = 0.0f;
                Skill4Explode_flag = false;
                Skill4overheat = 0.0f;//歸零過熱值
            }
        }
    }


    void Skill4Explode_Ani(bool flag) //爆炸動畫 (非腳色)
    {
        if (flag) //角色施放報炸動話 要再加!
        {
            if (Skill4Explode_AniTimeer == 0.0f) //第一個frame
            {
                transform.position = Playerr.transform.position;
                Skill4Range2.SetActive(true);
                Skill4Explode_AniTimeer = Skill4Explode_AniTimeer + Time.deltaTime;//切報炸動畫
                                                                                   //開報炸特效 位置不能每frame更新
            }
            else if (Skill4Explode_AniTimeer < Skill4Explode_AniTime)
            {
                Skill4Explode_AniTimeer = Skill4Explode_AniTimeer + Time.deltaTime;
            }
            else
            {
                Skill4Explode_AniTimeer = 0.0f;
                Skill4Range2.SetActive(false);
                
                Skill4Explode_AniFlag = false;//關報炸動畫

            }
        }
        else
            return;
    }


    void Update ()
    {
        if (Input.GetKey(KeyCode.R) && Skill4CC_flag && !Skill4Explode_flag)// GetKey  按住按鍵：按著按鍵時會傳回True
        {
            if (Skill4overheat < 100) ////未過熱
            {

                if (Skill4CC_firstFrameFlag)  //first frame
                {
                    //切恐懼動畫
                    transform.position = Playerr.transform.position;//特效範圍拉回玩家位置(腳下) 位置要每frame更新
                    Skill4Range1.SetActive(true);
                    Skill4overheat = Skill4overheat + 30; //起始frame加一值
                    Skill4CC_firstFrameFlag = false;
                    Debug.Log("heat" + Skill4overheat);
                }

                else 
                {
                    Skill4overheat = Skill4overheat + Time.deltaTime * Skill4overheatSpeed; //後續frame慢慢累加
                    transform.position = Playerr.transform.position;//特效範圍拉回玩家位置(腳下) 位置要每frame更新
                    Debug.Log("heat" + Skill4overheat);
                }

            }
            else ////過熱
            {
                //切報炸動畫
                //開報炸特效 位置不能每frame更新
                Skill4Range1.SetActive(false); //關恐懼範為
                Skill4Explode_flag = true;
                Skill4Explode_AniFlag = true;
            }
 
            ////Child.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.R) && Skill4overheat < 100)//觸發離開 開始計算未過熱CD
        {
            Skill4CC_flag = false;//計算未過熱CD
                                  //切回原動畫 idle隻類的
            Skill4Range1.SetActive(false);
            Debug.Log("111111111111111111111111111111111111111111111");

        }
        else if (Skill4CC_flag && !Skill4Explode_flag &&Skill4overheat > 0.0f) //降溫
        {
            Skill4overheat = Skill4overheat - Time.deltaTime * Skill4ColdSpeed; //
            Debug.Log("heat" + Skill4overheat);
            if (Skill4overheat < 0.1f) {
                Skill4overheat = 0.0f;
                //降到0停止
            }
        }

        Skill4CCCD_counter();
        Skill4Explode_Ani(Skill4Explode_AniFlag);
    }

}
/*
ObjectA
--ObjectB
--ObjectC
----ObjectD

假設Object A, B, C 以及D的類型都是GameObject，如果想要藉由Object A來讀到其子物件，
網路大多數的教法是使用GameObject.Find來做，但Find有2個缺點，1.無差別的模式搜詢，2.必須知道物件的名稱。
所以如果程式使用複製物件時，則會搜詢出好多相同名稱的物件，除此有改名稱，這麼一來問題就來了，如果名稱是用自動且隨機的方式產生，
我們將無法明確的找出子物仔有哪些，所以Find並不是一個抓取物件的好方法。

然後在Unity3d的腳本文件，有一系列的GetComponent的function可以使用來完成，
正常的用法其實都還不錯用，可是在抓取GameObject怎麼嘗試都會出錯，最主要的問題在於unity3d的文件中，
明確的定義Component包含gameObject，所以很直觀的會餵GameObject來進行抓取，但不管怎麼下達總是會失敗，
事實上要抓GameObject類型物件，只需需改成Transform類型來承接即可，如下列範例就可以抓到Object A的3個子物件。
*/
