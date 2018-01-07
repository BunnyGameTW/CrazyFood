using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	//public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.  //我們應該不需要高度吧 節省效能囉~ 再說了Public一堆可以調看了我好煩XD
	public float zMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 1f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 1f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public float zSmooth = 1f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector3 maxXYZ;		// The maximum x and y coordinates the camera can have.
	public Vector3 minXYZ;		// The minimum x and y coordinates the camera can have.
	public GameObject target;

	private Transform player;		// Reference to the player's transform.

    /**  scroll var   **/
    public float fScrollSpeed;
    private float fHeight;
    private bool IsScroll;
    private float fLerpHeight;//還需要內差的Y值
    private float fWasLerpHeight;//以經內差後的Y值
    /**  scroll var  **/

    void Awake ()
	{
		// Setting up the reference.
		player = target.transform;

        // scroll var init
        fScrollSpeed = 2.0f;
        IsScroll = true;
        fLerpHeight = 0.0f;
        fWasLerpHeight = 0.0f;
        // scroll var init
    }


    bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckZMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.z - player.position.z) > zMargin;
	}

	bool CheckYMargin()
	{
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        //return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
        return (true);
	}

	void FixedUpdate ()
	{
		TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;
		float targetZ = transform.position.z;

		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckZMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetZ = Mathf.Lerp(transform.position.z, player.position.z-20.0f, zSmooth * Time.deltaTime);
        /*取消一切關於Y軸得Lerp()*/
        ////if(CheckYMargin())
        //// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        ////targetY = Mathf.Lerp(transform.position.y, player.position.y, xSmooth * Time.deltaTime);
        ////targetZ = transform.position.z - player.position.z;
        //// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        /*取消一切關於Y軸得Lerp()end*/

        //
        if (MouseSroll() != 0)
            fLerpHeight = MouseSroll();
        targetY = Mathf.Lerp(transform.position.y, transform.position.y+fLerpHeight, ySmooth * Time.deltaTime); //每偵進行內差
      //  targetZ = Mathf.Lerp(transform.position.z, transform.position.z - fLerpHeight, zSmooth * Time.deltaTime); //每偵進行內差

        fWasLerpHeight = Mathf.Abs(targetY - transform.position.y);//計算出該偵內差了多少 NAN零除以零的結果
        if(fWasLerpHeight!=0.0f)
            fLerpHeight = (Mathf.Abs(fLerpHeight) - fWasLerpHeight)*(fLerpHeight/ Mathf.Abs(fLerpHeight)); //勝下該Lerp的高度 保留其正負號

        targetX = Mathf.Clamp(targetX, minXYZ.x, maxXYZ.x);
		targetY = Mathf.Clamp(targetY, minXYZ.y, maxXYZ.y); 
		targetZ = Mathf.Clamp(targetZ, minXYZ.z, maxXYZ.z);

        
       
        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, targetZ);
	}

    float MouseSroll()
    {
        if (IsScroll)
        {
            if (transform.position.y < maxXYZ.y && Input.GetAxis("Mouse ScrollWheel") < 0.0f)  //放
                //transform.position = new Vector3(transform.position.x, transform.position.y + fScrollSpeed, transform.position.z);
                return fScrollSpeed;
            else if (transform.position.y > minXYZ.y && Input.GetAxis("Mouse ScrollWheel") > 0.0f) //縮
                //transform.position = new Vector3(transform.position.x, transform.position.y - fScrollSpeed, transform.position.z);
                return -fScrollSpeed;
            else
                return 0.0f;
        }
        else
            return 0.0f;
    }
}


