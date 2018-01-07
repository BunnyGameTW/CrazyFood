using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunset : MonoBehaviour {
	float timer;
	//public float sunsetTime,sunriseTime;
	public GameObject light,playerLight;
    private Light lightInstance; //改變顏色 強度需要
    private float lightIntensity;
    public float SunSetTime,SunRiseTime, DayTime,NightTime;
    public Color SunSetColor;
    private Color DayColor;
    const float SunsetDrgree = 20; //日落最後的角度
    const float SunRiseDrgree = 130; //日出最後的角度 日出160->130
    public int State;
    public enum LightState {Day,SunSet,Night,SunRise};
    private bool once = true;
    bool isSunSet=true, IsisSunRise;
	// Use this for initialization
	void Start () {
        //RenderSettings.ambientIntensity = 8;  調整 windows->light之中的參數Ambient Intensity (1-8 float)
        
        lightInstance = light.GetComponent<Light>();
        DayColor = lightInstance.color; //存好白天的顏色
        SunSetTime = SunRiseTime = 15;
        DayTime = NightTime = 5.0f;
        State = (int)LightState.Day;
        Debug.Log((int)LightState.Day);
    }
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
        //Debug.Log(light.transform.eulerAngles.x);
        if (State == (int)LightState.Day) {
            if (once)
            {
                light.transform.rotation = Quaternion.Euler(130.0f, -30.0f, 0.0f);  //轉長特定度 
                lightInstance.intensity = 1;
                RenderSettings.ambientIntensity = 1;
                once = false;
                timer = 0.0f;
            }
            light.transform.Rotate(new Vector3(-(130 - 50) / DayTime, 0f, 0f) * Time.deltaTime);
            if (timer> DayTime)
            {
                timer = 0.0f;
                State = (int)LightState.SunSet;
                Debug.Log("日落");
                once = true;
            }
        }
        else if (State == (int)LightState.SunSet)
        {
            if (once)
            {
                light.transform.rotation = Quaternion.Euler(50.0f, -30.0f, 0.0f);  //轉長特定度 
                lightInstance.intensity = 1;
                RenderSettings.ambientIntensity = 1;
                once = false;
                timer = 0.0f;
            }
            light.transform.Rotate(new Vector3(-(50 - SunsetDrgree) / SunSetTime, 0f, 0f) * Time.deltaTime);
            //傾到40度的時候光強度下降
            float x;
            if ((50 - light.transform.rotation.eulerAngles.x) < 10)
                x = 0;
            else
                x = (40 - light.transform.rotation.eulerAngles.x) / 20;
            lightIntensity = Mathf.Lerp(1, 0, x);  //算光強度   ((變化量/總變化量
            lightInstance.intensity = lightIntensity;
            // ambientIntensity 影響一切
            RenderSettings.ambientIntensity = lightIntensity;
            lightInstance.color = Color.Lerp(DayColor, SunSetColor, (50 - light.transform.rotation.eulerAngles.x) / (50 - SunsetDrgree));
            if (light.transform.rotation.eulerAngles.x < SunsetDrgree + 3.0f)
            {
                playerLight.SetActive(true);
            }
            if (timer> SunSetTime)
            {
                once = true;
                timer = 0.0f;
                State = (int)LightState.Night;
                Debug.Log("晚上");
            }
        }
        else if (State == (int)LightState.Night) {
            if (once)
            {
                light.transform.rotation = Quaternion.Euler(0.0f, -30.0f, 0.0f);  //轉長特定度 
                lightInstance.intensity = 0;
                RenderSettings.ambientIntensity = 0;
                once = false;
                timer = 0.0f;
            }
            if (timer > NightTime)
            {
                timer = 0.0f;
                State = (int)LightState.SunRise;
                Debug.Log("日出");
                once = true;
            }
        }
        else if (State == (int)LightState.SunRise) {
            if (once)
            {
                light.transform.rotation = Quaternion.Euler(160.0f, -30.0f, 0.0f);  //轉長特定度 準備開始日出
                lightInstance.intensity = 0;
                RenderSettings.ambientIntensity = 0;
                once = false;
                timer = 0.0f;
            }

            light.transform.Rotate(new Vector3(-(160 - SunRiseDrgree) / SunRiseTime, 0f, 0f) * Time.deltaTime);
            //160到150度的時候光強度上升
            //light.transform.rotation.eulerAngles.x的回傳直很詭異 從160轉到130 竟然傳20-50   ????
            float x;
            if ((light.transform.rotation.eulerAngles.x) < 30)
                x = (light.transform.rotation.eulerAngles.x - 20) / 10;
            else
                x = 1;
            lightIntensity = Mathf.Lerp(0, 1, x);  //算光強度   ((變化量/總變化量
            Debug.Log(light.transform.rotation.eulerAngles.x+"---------" +x);
            lightInstance.intensity = lightIntensity;
            RenderSettings.ambientIntensity = lightIntensity;
            lightInstance.color = Color.Lerp(SunSetColor, DayColor, (160 - light.transform.rotation.eulerAngles.x) / (160 - SunRiseDrgree));
            if (light.transform.rotation.eulerAngles.x > 20) {
                playerLight.SetActive(false);
            }
            if (light.transform.rotation.eulerAngles.x > 50) {
                timer = 0;
                State = (int)LightState.Day;
                Debug.Log("DAY");
                once = true;
            }
        }

        else
            Debug.Log("光壞了");
		//日出長得很詭異 腳色會被光
	}
}
