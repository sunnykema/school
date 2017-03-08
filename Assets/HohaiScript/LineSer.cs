using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using System;
public class LineSer : MonoBehaviour {
    public string serverLacation = HostIp.serverLacation;
    public string myLocationServer = HostIp.myLocationServer;
    private string url = "";
    public static int cnt = 0;
    public static int clickCount = 0;
    public const int maxLen = 250;

    public UIInput uiInput;
 //   public UIPopupList controlFloor;
    public UIPopupList controlServer;
    
    public GameObject ToastLabel;
    
    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 3f;
    private bool IsToastTime = false;
    

    //服务的信息
    public static string[] Lno = new string[maxLen];
    public static string[] Xaxis = new string[maxLen];
    public static string[] Yaxis = new string[maxLen];
    public static string[] Zaxis = new string[maxLen];
    public static string[] tName = new string[maxLen];
    public static string[] remarks = new string[maxLen];
    public static string[] ImgURL = new string[maxLen];
    public static string[] typename = new string[maxLen];
    public static string[] storey = new string[maxLen];


    MyAndroidToast toast;
    void Start(){
        toast = new MyAndroidToast();
    }

	void Update () {
		//        print("IsToasfsf = "+IsToastTime);
		if (!IsToastTime)
		{
			return;
		}
		// print("asdfas");
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
    void GetIp()
    {
        serverLacation = HostIp.serverLacation;
        myLocationServer = HostIp.myLocationServer;
    }
    //改变listview当中的值
    public void OnValueChange()
    {
        GetIp();
        clickCount = 1;
        uiInput = GameObject.Find("InputField").GetComponent<UIInput>();
      //  controlFloor = GameObject.Find("Control_Floor").GetComponent<UIPopupList>();
        controlServer = GameObject.Find("Control_Server").GetComponent<UIPopupList>();
		url = "http://" + serverLacation + "/SchoolWander/servlet/ServerFind";
      //  string chooseStairs = controlFloor.value.ToString().Trim();
        //Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").getBytes(s));
      //  print("chooseStairs = " + chooseStairs);
        string chooseType = controlServer.value.ToString().Trim();
        print("chooseType =" + chooseType);
        string chooseInput = uiInput.value.ToString().Trim();
        print("chooseInput = " + chooseInput);
        //url += "chooseStairs=" + chooseStairs + "&chooseType=" + chooseType + "&chooseInput=" + chooseInput;
        //SetStart ();
        //GET请求
        //StartCoroutine(GET(url));

        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("chooseStairs", chooseStairs);
        dic.Add("chooseType", chooseType);
        dic.Add("chooseInput", chooseInput);
        dic.Add("clickCount", clickCount.ToString());
        StartCoroutine(POST(url, dic));

    }

    //POST请求 全局
    IEnumerator POST(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        yield return www;
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
            print("post method");
            ToastState("请求成功!!!");
            string str = www.text.ToString();
            //print ("str ="+str);

            string[] strs = new string[maxLen*2];

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
                	print("stri = "+str1);
                JsonData jd = JsonMapper.ToObject(str1);
                //	print(jd[0]["Lno"]);
                Lno[i] = jd[0]["Lno"].ToString();
                Xaxis[i] = jd[0]["Xaxis"].ToString();
                Yaxis[i] = jd[0]["Yaxis"].ToString();
                Zaxis[i] = jd[0]["Zaxis"].ToString();
                tName[i] = jd[0]["name"].ToString();
                remarks[i] = jd[0]["remarks"].ToString();
                ImgURL[i] = jd[0]["ImgURL"].ToString();
                typename[i] = jd[0]["typename"].ToString();
//                storey[i] = jd[0]["storey"].ToString();
            }
			AddItemIntable addItem = gameObject.GetComponent<AddItemIntable>();
            addItem.ChangeListView();

        }
    }
    

	// 添加更多。。
	public void GetMoreByPostMethod(){
		GetIp();
		clickCount++;
		uiInput = GameObject.Find ("InputField").GetComponent<UIInput>();
		//controlFloor = GameObject.Find ("Control_Floor").GetComponent<UIPopupList> ();
		controlServer = GameObject.Find ("Control_Server").GetComponent<UIPopupList> ();
		url = "http://"+serverLacation+"/SchoolWander/servlet/ServerFind";
	//	string chooseStairs = controlFloor.value.ToString().Trim();
		//Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").getBytes(s));
	//	print ("chooseStairs = "+chooseStairs);
		string chooseType = controlServer.value.ToString().Trim();
		print ("chooseType ="+chooseType);
		string chooseInput = uiInput.value.ToString().Trim();
		print("chooseInput = "+chooseInput);
		//url += "chooseStairs=" + chooseStairs + "&chooseType=" + chooseType + "&chooseInput=" + chooseInput;
		//SetStart ();
		//GET请求
		//StartCoroutine(GET(url));
		
		//注册请求 POST		
		Dictionary<string,string> dic = new Dictionary<string, string> ();	
	//	dic.Add("chooseStairs",chooseStairs);		
		dic.Add("chooseType",chooseType);		
		dic.Add("chooseInput",chooseInput); 
		dic.Add ("clickCount",clickCount.ToString());
		StartCoroutine(POST(url,dic));	
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
