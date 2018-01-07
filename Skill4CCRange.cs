using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4CCRange : MonoBehaviour {

    public delegate void sendkill4Range1Message(GameObject obj, string state, Collider collider);//宣告委派，該委派無返回值、有三個傳入的參數
    public static event sendkill4Range1Message sendSkill4Range1Event;//宣告事件用於委派，該事件必須為public static

    private void Message(GameObject obj, string state, Collider collider) // send message 
    {
        sendSkill4Range1Event(obj, state, collider);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("enemy"))
        {
            Message(gameObject, "Enter", collider);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("enemy"))
        {
            Message(gameObject, "Stay", collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("enemy"))
        {
            Message(gameObject, "Exit", collider);
        }
    }
}
