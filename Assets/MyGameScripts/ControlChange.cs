/**
 *  * 
 * HTTP SERVER
 * 
 * 
 * */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using System;
public class ControlChange : MonoBehaviour {
	public  const int maxLen = 250;
	public UIInput uiInput ;
	public UIPopupList controlFloor;
	public UIPopupList controlServer;
    private GameObject people;
    private Transform tran;
    public string serverLacation = HostIp.serverLacation;
    public string myLocationServer = HostIp.myLocationServer;
	private string url  = "";
	public  static int cnt = 0;
	public static int clickCount = 0;
    public static string People_Position = "";       //用于别的脚本调用的定位位置变量

    public GameObject ToastLabel;
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
    //flight 的信息

    public static string[] Ano = new string[maxLen];
    public static string[] FromPlace = new string[maxLen];
    public static string[] ToPlace = new string[maxLen];
    public static string[] PlanttoArrive = new string[maxLen];
    public static string[] state = new string[maxLen];
    public static string[] luggage = new string[maxLen];

    public static string[] PlantoLaunch = new string[maxLen];
    public static string[] Gates = new string[maxLen];
    public static string[] checking = new string[maxLen];
    public static string[] counter = new string[maxLen];


    private float x, y, z;
    private float[] arr = {0.0f,0.0f,0.0f};
    private char ch;
    private string temp;
    private int peopleStairs;
    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 3f;
    private bool IsToastTime = false;
	void Awake(){

	}
    

    void Update() {
        if (!IsToastTime) {
            return;
        }
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
    //获得IP地址
    void GetIp()
    {
        serverLacation = HostIp.serverLacation;
        myLocationServer = HostIp.myLocationServer;
    }
    //获得游戏体所在的楼层
    void GetPeopleStaris()
    {
        people = GameObject.Find("people");
        float stairs = people.transform.position.y;
        /*
        if (stairs < 50.0)
        {
            peopleStairs = 0;
        }
        else if (stairs < 85)
        {
            peopleStairs = 1;
        }
        else
        {
            peopleStairs = 2;
        }
         * */
        ChangePosition changePosition = new ChangePosition();
        peopleStairs = changePosition.GetStairs(stairs);

    }
	//改变listview当中的值
	public void OnValueChange(){
        GetIp();
		clickCount = 1;
		uiInput = GameObject.Find ("InputField").GetComponent<UIInput>();
		controlFloor = GameObject.Find ("Control_Floor").GetComponent<UIPopupList> ();
		controlServer = GameObject.Find ("Control_Server").GetComponent<UIPopupList> ();
		url = "http://"+serverLacation+"/IndoorNav/servlet/ServerFind";
		string chooseStairs = controlFloor.value.ToString().Trim();
		//Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").getBytes(s));
		print ("chooseStairs = "+chooseStairs);
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
		dic.Add("chooseStairs",chooseStairs);		
		dic.Add("chooseType",chooseType);		
		dic.Add("chooseInput",chooseInput); 
		dic.Add ("clickCount",clickCount.ToString());
	   	StartCoroutine(POST(url,dic));	

	}

    //查询航信息... 到达
    public void GetFlightIn()
    {
        GetIp();

        string Server = "ServerFlightIn";
        string Type = "In";

        url = "http://" + serverLacation + "/IndoorNav/servlet/" + Server + "?Type=" + Type;
        print("Url = " + url);
        //Get请求.
        StartCoroutine(GetForFlightIn(url));
    }

    

    //查询航信息... 出发
    public void GetFlightOut()
    {
        GetIp();

        string Server = "ServerFlightOut";
        string Type = "Out";

        url = "http://" + serverLacation + "/IndoorNav/servlet/" + Server + "?Type=" + Type;
        print("Url = " + url);
        //Get请求.
        StartCoroutine(GetForFlightOut(url));
    }

    //在航班信息中，点击航班生成相应线路
    public void CreateLineforFlightIn()
    {
        GetIp();
        string str = gameObject.name;
        str = "Luggage" + str;

        print("str = "+str);
        UILabel tmp;
        tmp = GameObject.Find(str).GetComponent<UILabel>();
        str = tmp.text;
        print("str = " + str);
        url = "http://" + serverLacation + "/IndoorNav/servlet/CreateLineforFlightIn?Luggage=" +str;
        print("Url = " + url);
        //Get请求.
        StartCoroutine(GetLineForFlightIn(url));
    }
    //在航班信息中，点击航班生成相应线路
    public void CreateLineforFlightOut() {
        GetIp();
        string str = gameObject.name;
        print("str = " + str);
        string cuss = "CUSSName" + str;
        string gate = "gateName" + str;
        UILabel CUSSName;
        CUSSName = GameObject.Find(cuss).GetComponent<UILabel>();

        UILabel gateName;
        gateName = GameObject.Find(gate).GetComponent<UILabel>();
        url = "http://" + serverLacation + "/IndoorNav/servlet/CreateLineforFlightOut?CUSSName=" + CUSSName.text + "&gateName="+gateName.text;
        //Get请求.
        StartCoroutine(GetLineForFlightOut(url));
    }

	//获取当前的地址。
	public void GetMyLocation(){
        GetIp();
		string myurl = "http://" + myLocationServer + "/loc";
		StartCoroutine (GET(myurl));
	}

    //nearly的请求
    public void SearchNearlyFood()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the food Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "餐饮");
        StartCoroutine(POSTforNear(url, dic));

    }
    public void SearchNearlyMarket()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the Market Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "商店");
        StartCoroutine(POSTforNear(url, dic));
    }
    public void SearchNearlyStair()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the Stair Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "扶梯");
        StartCoroutine(POSTforNear(url, dic));
    }
    public void SearchNearlyElevate()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the Elevate Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "电梯");
        StartCoroutine(POSTforNear(url, dic));
    }
    public void SearchNearlyToilet()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the Toilet Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "卫生间");
        StartCoroutine(POSTforNear(url, dic));
    }
    public void SearchNearlyWaterPlace()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the WaterPlace Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "饮水处");
        StartCoroutine(POSTforNear(url, dic));
    }

    public void SearchLocalProduct()
    {
        GetIp();
        GetPeopleStaris();
        print("you click the LocalProduct Button");
        url = "http://" + serverLacation + "/IndoorNav/servlet/ServerNearly";
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("chooseStairs", peopleStairs + "");
        dic.Add("chooseType", "特产");
        StartCoroutine(POSTforNear(url, dic));
    }

	// 添加更多。。
	public void GetMoreByPostMethod(){
        GetIp();
		clickCount++;
		uiInput = GameObject.Find ("InputField").GetComponent<UIInput>();
		controlFloor = GameObject.Find ("Control_Floor").GetComponent<UIPopupList> ();
		controlServer = GameObject.Find ("Control_Server").GetComponent<UIPopupList> ();
		url = "http://"+serverLacation+"/IndoorNav/servlet/ServerFind";
		string chooseStairs = controlFloor.value.ToString().Trim();
		//Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").getBytes(s));
		print ("chooseStairs = "+chooseStairs);
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
		dic.Add("chooseStairs",chooseStairs);		
		dic.Add("chooseType",chooseType);		
		dic.Add("chooseInput",chooseInput); 
		dic.Add ("clickCount",clickCount.ToString());
		StartCoroutine(POST(url,dic));	
	}
	/*
	void SetStart ()
	{
		//GET请求
		StartCoroutine(GET(url));
		
	}
	*/

    /// <summary>
    /// 上传行走的路线，进行数据挖掘
    /// </summary>
    public void UpLoadRoad(string myPosition, string TargetPosition, string myRode,string evaRoad)
    {
        GetIp();
        //注册请求 POST		
        url = "http://" + serverLacation + "/IndoorNav/servlet/UerRoad";
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("myPosition", myPosition);
        dic.Add("TargetPosition", TargetPosition);
        dic.Add("myRode", myRode);
        dic.Add("evaRoad", evaRoad);
        StartCoroutine(POSTForUpLoadRoad(url, dic));	
    }


    /// <summary>
    /// post for upload Road
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    IEnumerator POSTForUpLoadRoad(string url, Dictionary<string, string> post)
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
          //  ToastState("请求失败!!!");

        }
        else
        {
            //POST请求成功            
            Debug.Log("request ok : " + www.text);
            print("post method");
        //    ToastState("请求成功!!!");
       
        }
    }

	void OnGUI()
	{
		
	}

	//POST请求 附近
	IEnumerator POSTforNear(string url, Dictionary<string,string> post){  
		WWWForm form = new WWWForm ();    
		foreach (KeyValuePair<string,string> post_arg in post) {     
				form.AddField (post_arg.Key, post_arg.Value);    	
		} 		
		WWW www = new WWW (url, form);	
		yield return www;
        
		if (www.error != null) {      
				//POST请求失败			
				Debug.Log ("error is :" + www.error);
                ToastState("请求失败!!!");
	
		} else {			
				//POST请求成功            
				Debug.Log ("request ok : " + www.text);
                ToastState("请求成功!!!");
                string str = www.text.ToString();
                //print ("str ="+str);

                string[] strs = new string[maxLen];

                cnt = 0;
                strs[0] = "";
                for (var i = 0; i < str.Length; i++)
                {
                    if (str[i] != '&')
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
	//POST请求 全局
	IEnumerator POST(string url, Dictionary<string,string> post){  
		WWWForm form = new WWWForm();    
		foreach(KeyValuePair<string,string> post_arg in post){     
			form.AddField(post_arg.Key, post_arg.Value);    	
		} 		
		WWW www = new WWW(url, form);	
		yield return www; 		
		if (www.error != null)  {      
			//POST请求失败			
			Debug.Log("error is :"+ www.error);
            ToastState("请求失败!!!");

		} else{			
			//POST请求成功            
			Debug.Log("request ok : " + www.text);   
			print ("post method");
            ToastState("请求成功!!!");
			string str = www.text.ToString();
			//print ("str ="+str);
			
			string[] strs = new string[maxLen]; 
			
			cnt  = 0;
			strs[0] = "";
			for(var i = 0; i < str.Length;i++){
				if(str[i] != '&'){
					strs[cnt] += str[i];
				}else{
					cnt ++;
					strs[cnt] = "";
				}
			}
			print("cnt ="+cnt);
			/*
			JsonData jd = JsonMapper.ToObject(strs);
			for(int i = 0; i< jd.Count;i++){
				print ("Lno ="+jd[i]["Lno"]);
			}
			*/
			
			for(int i = 0; i < cnt ; i++){
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
				remarks[i] = jd[0]["remarks"].ToString();
                ImgURL[i] = jd[0]["ImgURL"].ToString();
                typename[i] = jd[0]["typename"].ToString();
                storey[i] = jd[0]["storey"].ToString();
			}
			AddItemByValueChange addItem = gameObject.GetComponent<AddItemByValueChange>();
			addItem.ChangeListView ();

		}    
	}

    /// <summary>
    /// 获得航班页面用户点击的到达飞机的行李盘的位置。
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator GetLineForFlightIn(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            //GET请求失败
            Debug.Log("error is :" + www.error);
          //  ToastState("请求失败!!!");
        }
        else
        {
            //GET请求成功
            Debug.Log("request ok : " + www.text);
           // ToastState("请求成功!!!");
            //print ("www");
            string str = www.text.ToString();
            JsonData jd = JsonMapper.ToObject(str);
            Lno[0] = jd[0]["Lno"].ToString();
            Xaxis[0] = jd[0]["Xaxis"].ToString();
            Yaxis[0] = jd[0]["Yaxis"].ToString();
            Zaxis[0] = jd[0]["Zaxis"].ToString();
            AddTarget addTarget = new AddTarget();
            addTarget.GetLineForFlightIn();
             /*
            float x, y, z;
            x = float.Parse(Xaxis[0]);
            y = float.Parse(Yaxis[0]);
            z = float.Parse(Zaxis[0]);
            ChangePosition changePosition = new ChangePosition();
            changePosition.ChangePeople(x, y, z);
            */

        }


    }


    /// <summary>
    ///  获得航班页面用户点击的起飞飞机的值机柜台和登机口的位置。
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator GetLineForFlightOut(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            //GET请求失败
            Debug.Log("error is :" + www.error);
         //   ToastState("请求失败!!!");
        }
        else
        {
            //GET请求成功
            Debug.Log("request ok : " + www.text);
         //   ToastState("请求成功!!!");
            //print ("www");

            string str = www.text.ToString();
            string[] strs = new string[5];

            cnt = 0;
            strs[0] = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != '&')
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
            }
            
            AddTarget addTarget = new AddTarget();
            addTarget.GetLineForFlightOut();
            /*
           float x, y, z;
           x = float.Parse(Xaxis[0]);
           y = float.Parse(Yaxis[0]);
           z = float.Parse(Zaxis[0]);
           ChangePosition changePosition = new ChangePosition();
           changePosition.ChangePeople(x, y, z);
           */

        }


    }

    //Get请求...航班信息....到达
    IEnumerator GetForFlightIn(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            //GET请求失败
            Debug.Log("error is :" + www.error);
        //    ToastState("请求失败!!!");
        }
        else
        {
            //GET请求成功
            Debug.Log("request ok : " + www.text);
          //  ToastState("请求成功!!!");
            //print ("www");
            string str = www.text.ToString();
            //print ("str ="+str);

            string[] strs = new string[maxLen];

            cnt = 0;
            strs[0] = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != '&')
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

            for (int i = 0; i < cnt; i++)
            {
                //print(strs[i]);
                string str1 = strs[i];
                //	print("stri = "+str1);
                JsonData jd = JsonMapper.ToObject(str1);
                //	print(jd[0]["Lno"]);
                Ano[i] = jd[0]["Ano"].ToString();
                FromPlace[i] = jd[0]["FromPlace"].ToString();
                ToPlace[i] = jd[0]["ToPlace"].ToString();
                PlanttoArrive[i] = jd[0]["PlanttoArrive"].ToString();
                state[i] = jd[0]["state"].ToString();
                luggage[i] = jd[0]["luggage"].ToString();

            }
            AddFlight addFlight = gameObject.GetComponent<AddFlight>();
            addFlight.ChangeFlightIn();

        }
    }



    //Get请求...航班信息....出发
    IEnumerator GetForFlightOut(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            //GET请求失败
            Debug.Log("error is :" + www.error);
     //       ToastState("请求失败!!!");
        }
        else
        {
            //GET请求成功
            Debug.Log("request ok : " + www.text);
       //     ToastState("请求成功!!!");
            //print ("www");
            string str = www.text.ToString();
            //print ("str ="+str);

            string[] strs = new string[maxLen];

            cnt = 0;
            strs[0] = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != '&')
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

            for (int i = 0; i < cnt; i++)
            {
                //print(strs[i]);
                string str1 = strs[i];
                //	print("stri = "+str1);
                JsonData jd = JsonMapper.ToObject(str1);
                //	print(jd[0]["Lno"]);
                Ano[i] = jd[0]["Ano"].ToString();
                FromPlace[i] = jd[0]["FromPlace"].ToString();
                ToPlace[i] = jd[0]["ToPlace"].ToString();
                PlantoLaunch[i] = jd[0]["PlantoLaunch"].ToString();
                state[i] = jd[0]["state"].ToString();
                Gates[i] = jd[0]["Gates"].ToString();
                checking[i] = jd[0]["checking"].ToString();
                counter[i] = jd[0]["counter"].ToString();

            }
            AddFlight addFlight = gameObject.GetComponent<AddFlight>();
            addFlight.ChangeFlightOut();
        }
    }

	//GET请求 定位...
	IEnumerator GET(string url){
		WWW www = new WWW (url);
		yield return www;

		if (www.error != null) {
				//GET请求失败
			Debug.Log ("error is :" + www.error);
            ToastState("定位失败!!!");
	
		} else {
				//GET请求成功
			Debug.Log ("request ok : " + www.text);
            ToastState("定位成功!!!");
            temp = www.text;
            int k = 0;
            int flag = 0;
            string tmp = "";
            for (int i = 0; i < www.text.Length; i++) {
                if (temp[i] == '[') {
                    continue;
                }
                if (temp[i] == ',' || temp[i] == ']') {
                    //print("tmp = "+tmp);
                    arr[k] = float.Parse(tmp);
                    tmp = "";
                    /*s
                    for (int j = i - 1; j >= 0; j--) {
                        if (temp[i] == '.')
                            break;
                        arr[k] += temp[i];
                        arr[k] /= 10;
                    }
                     * */
                    k++;
                }else
                 tmp += temp[i];
            }
            people = GameObject.Find("people");
           // print("People = "+people);
            tran = people.transform;
            Vector3 po;
            po = tran.position;
            ChangePosition changePosition = new ChangePosition();
            changePosition.ChangeLocation(arr[0], arr[1], arr[2]);
            po.x = changePosition.GetX();
            po.y = changePosition.GetY();
            po.z = changePosition.GetZ();
            people.transform.position = po;
            People_Position = po+"";
            print("people_position"+People_Position);
		}
	}



	/*
	//GET请求
	IEnumerator GET(string url)
	{
		
		WWW www = new WWW (url);
		yield return www;
		
		if (www.error != null)
		{
			//GET请求失败
			Debug.Log("error is :"+ www.error);
			
		} else
		{
			//GET请求成功
			Debug.Log("request ok : " + www.text);

			string str = www.text.ToString();
			//print ("str ="+str);

			string[] strs = new string[maxLen]; 

			cnt  = 0;
			strs[0] = "";
			for(var i = 0; i < str.Length;i++){
				if(str[i] != '&'){
					strs[cnt] += str[i];
				}else{
					cnt ++;
					strs[cnt] = "";
				}
			}
			print("cnt ="+cnt);
			/*
			JsonData jd = JsonMapper.ToObject(strs);
			for(int i = 0; i< jd.Count;i++){
				print ("Lno ="+jd[i]["Lno"]);
			}


			for(int i = 0; i < cnt ; i++){
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
				remarks[i] = jd[0]["remarks"].ToString();

			}
			
		

		}
	}

*/
}
