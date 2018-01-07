using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
  平滑的放大有2种方法：
       1.float distance-=Input.GetAxis("Mouse ScrollWheel")*0.1f*Mathf.Abs(distance);（我自己原来写过，这个也不错）
       2.float distance *= (1-0.1f*Input.GetAxis("Mouse ScrollWheel"));（官方用这个列子）
 */
public class MouseScroll : MonoBehaviour {

    public float fMax_Height;
    public float fMin_Height;
    public float fScrollSpeed;
    private float fHeight;

    // Update is called once per frame
    void Start() {
        fMax_Height = 45.0f;
        fMin_Height = 12.0f;
        fScrollSpeed = 1.0f;
    }

    void FixedUpdate() {
        MouseSroll();
    }

    void MouseSroll() {
            if (true)
            {
                if (transform.position.y < fMax_Height && Input.GetAxis("Mouse ScrollWheel") < 0.0f)  //放
                    transform.position = new Vector3(transform.position.x, transform.position.y + fScrollSpeed, transform.position.z);

                else if (transform.position.y > fMin_Height && Input.GetAxis("Mouse ScrollWheel") > 0.0f) //縮
                    transform.position = new Vector3(transform.position.x, transform.position.y - fScrollSpeed, transform.position.z);
            }
        }
    
}
