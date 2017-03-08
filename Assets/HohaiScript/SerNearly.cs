using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using System;
/// <summary>
/// 附近服务
/// </summary>
public class SerNearly : MonoBehaviour {
     string serverLacation = HostIp.serverLacation;
     string myLocationServer = HostIp.myLocationServer;
     public const int maxLen = 250;
     public static int cnt = 0;

     //服务的信息
     public static string[] Lno = new string[maxLen];
     public static string[] Xaxis = new string[maxLen];
     public static string[] Yaxis = new string[maxLen];
     public static string[] Zaxis = new string[maxLen];
     public static string[] tName = new string[maxLen];

     string url = "";
   
     public GameObject ToastLabel;
     
     //toast的时间戳
     private float ToastTime = 5f;
     private const float maxToastTime = 3f;
     private bool IsToastTime = false;
	/*
    MyAndroidToast toast;
    void Start(){
        toast = new MyAndroidToast();
    }
*/
	void Update () {
		//print("IsToasfsf = "+IsToastTime);
		if (!IsToastTime)
		{
			return;
		}
		//print("asdfas");
		if (ToastTime > maxToastTime)
		{
			ToastLabel.SetActive(false);
			IsToastTime = false;
		}
		else
		{
			ToastTime = ToastTime + Time.deltaTime;
			
		}
	}

      //获得IP地址
    void GetMyIp()
    {
        serverLacation = HostIp.serverLacation;
        myLocationServer = HostIp.myLocationServer;
    }

    public void SearchNearLyParking() {
        GetMyIp();
        print("you click the Parking Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "停车场");
        StartCoroutine(POSTforNear(url, dic));
    
    }

    public void SearchNearlyScenery() { 
        GetMyIp();
        print("you click the Scenery Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "景观");
        StartCoroutine(POSTforNear(url, dic));
    }

    public void SearchNearlyLively() {
        GetMyIp();
        print("you click the Lively Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "生活");
        StartCoroutine(POSTforNear(url, dic));
    }

    public void SearchNearlyDormitory()
    {
        GetMyIp();
        print("you click the dormitory Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "宿舍");
        StartCoroutine(POSTforNear(url, dic));
    }
    public void SearchNearlyCanteen()
    {
        GetMyIp();
        print("you click the Canteen Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "食堂");
        StartCoroutine(POSTforNear(url, dic));
    }

    public void SearchNearlyClassRoom()
    {
        GetMyIp();
        print("you click the ClassRoom Button");
        url = "http://" + serverLacation + "/SchoolWander/servlet/SerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseName", "教学楼");
        StartCoroutine(POSTforNear(url, dic));
    }
    

    //POST请求 附近
    IEnumerator POSTforNear(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        yield return www;
        //ToastLabel.SetActive(true);
        if (www.error != null)
        {
            //POST请求失败			
            Debug.Log("error is :" + www.error);
           ToastState("请求失败!!!");

        }
        else
        {
            //POST请求成功            
            Debug.Log("request ok : " + www.text);
            ToastState("请求成功!!!");
            string str = www.text.ToString();
            //print ("str ="+str);

            string[] strs = new string[maxLen];

            cnt = 0;
            strs[0] = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != '@')
                {
                    strs[cnt] += str[i];
                }
                else
                {
                    cnt++;
                    strs[cnt] = "";
                }
            }
            print("cnt =" + cnt);
            /*
            JsonData jd = JsonMapper.ToObject(strs);
            for(int i = 0; i< jd.Count;i++){
                print ("Lno ="+jd[i]["Lno"]);
            }
            */

            for (int i = 0; i < cnt; i++)
            {
                //print(strs[i]);
                string str1 = strs[i];
                //	print("stri = "+str1);
                JsonData jd = JsonMapper.ToObject(str1);
                //	print(jd[0]["Lno"]);
                Lno[i] = jd[0]["Lno"].ToString();
                Xaxis[i] = jd[0]["Xaxis"].ToString();
                Yaxis[i] = jd[0]["Yaxis"].ToString();
                Zaxis[i] = jd[0]["Zaxis"].ToString();
                tName[i] = jd[0]["name"].ToString();
                //      remarks[i] = jd[0]["remarks"].ToString();

            }

            //  KGFMapSystem show = new KGFMapSystem();
            // show.GetKGFAwake();
            //show.ShowPoints();
            KGFMapSystem Kgf = GameObject.Find("KGFMapSystem").GetComponent<KGFMapSystem>();
            Kgf.ShowPoints();
        }
    }

    
    //打印Toast
    public void ToastState(string str)
    {
        ToastTime = 0;
        IsToastTime = true;
        print("ToastState Has Been safjk");
        ToastLabel.SetActive(true);
        UILabel contentLabel = GameObject.Find("ContentLabel").GetComponent<UILabel>();
        contentLabel.text = str;
    }
     
}
