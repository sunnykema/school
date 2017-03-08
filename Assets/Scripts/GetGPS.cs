using UnityEngine;
using System.Collections;
using LitJson;
public class GetGPS : MonoBehaviour
{

    public string gps_info = "";
    public int flash_num = 1;

    //ConverToBaiduMap converToBaiduMap;
    GameObject Label_Longitude;
    GameObject Label_Latitude;
    string Longitude;
    string Latitude;
    private float N;
    private float E;
    private float M;
    private float E1;
    private float N1;
    Vector3 po;
    private Transform people;
    // Use this for initialization

    public GameObject ToastLabel;
    private bool flag;
    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 3f;
    private bool IsToastTime = false;
    private float f_curtime;
    private float f_curtime1;
    /*
    MyAndroidToast toast;
    void Start(){
        toast = new MyAndroidToast();
    }
*/
    void Update()
    {
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

    void Start()
    {
        //converToBaiduMap = new ConverToBaiduMap();
        Label_Latitude = GameObject.Find("Label_Latitude");
        Label_Longitude = GameObject.Find("Label_Longitude");
        M = 6.2f;
        E1 = 0f;
        N1 = 0f;
        f_curtime = 0.0f;
        f_curtime1 = 0.0f;
        people = GameObject.Find("people").transform;
        flag = true;
        //StartCoroutine(StartGPS());
    }
    bool Isinschool()
    {
        if (E >= 119.983407f && E <= 119.991042f)
        {
            if (N >= 31.822602f && N <= 31.825862f)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 获取当前位置
    /// </summary>
    public void GetMyLocation() {
        // converToBaiduMap.ConverToBaidu(Input.location.lastData.longitude.ToString(), Input.location.lastData.latitude.ToString());
        // converToBaiduMap.ConverToBaidu("119.9753", "31.82049");
        // Longitude = converToBaiduMap.GetLongitude();
        //  Latitude = converToBaiduMap.GetLatitude();
        //  Debug.Log("Longitude = " + Longitude);
        //  Debug.Log("Latitude = " + Latitude);
    
        string url = "http://api.map.baidu.com/geoconv/v1/?";
        url = url + "coords=" + Input.location.lastData.longitude.ToString() + "," + Input.location.lastData.latitude.ToString() + "&from=1&to=5&ak=kxBEdjniN2c3yZLIesNzTIRS";
        StartCoroutine(GetBaiduCoords(url));
        this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
        //this.gps_info = "N:" + Latitude + " E:" + Longitude;
        this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
        this.flash_num += 1;
        E1 = E - 119.983524f;
        N1 = N - 31.822732f;
        po.z = (float)(101.3479 + 16059.781718 * E1);
        po.x = (float)(278.0573 - 14633.887595 * N1);
        if (E - 119.987166f >= 0.0f)
        {
            po.x -= 5.0f;
            // po.z += 6.0f;
          //  ToastState("进行了偏移！！");
        }

        po.y = M;
        people.position = po;

    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 28;
       // GUI.Label(new Rect(20, 20, 600, 48), this.gps_info);
       // GUI.Label(new Rect(20, 50, 600, 48), this.flash_num.ToString());
        //Label_Longitude.GetComponent<UILabel>().text = Longitude;
       // Label_Latitude.GetComponent<UILabel>().text = Latitude;
        if (Longitude != "" && Latitude != null)
        {
            E = float.Parse(Longitude);
        }
        if (Latitude != "" && Latitude != null)
        {
            N = float.Parse(Latitude);
        }

        GUI.skin.button.fontSize = 50;
        /*if (GUI.Button(new Rect(Screen.width / 2 - 110, 200, 220, 85), "GPS定位"))
        {
            // 这里需要启动一个协同程序
            StartCoroutine(StartGPS());
        }*/
        /*if (GUI.Button(new Rect(Screen.width / 2 - 110, 800, 220, 85), "切换GPS状态"))
        {
            if (flag)
                flag = false;
            else
                flag = true;
        }*/
        /*if (GUI.Button(new Rect(Screen.width / 2 - 110, 500, 220, 85), "刷新GPS"))
        {
            // converToBaiduMap.ConverToBaidu(Input.location.lastData.longitude.ToString(), Input.location.lastData.latitude.ToString());
            // converToBaiduMap.ConverToBaidu("119.9753", "31.82049");
            // Longitude = converToBaiduMap.GetLongitude();
            //  Latitude = converToBaiduMap.GetLatitude();
            //  Debug.Log("Longitude = " + Longitude);
            //  Debug.Log("Latitude = " + Latitude);
            if (flag == false)
            {
                string url = "http://api.map.baidu.com/geoconv/v1/?";
                url = url + "coords=" + Input.location.lastData.longitude.ToString() + "," + Input.location.lastData.latitude.ToString() + "&from=1&to=5&ak=kxBEdjniN2c3yZLIesNzTIRS";
                StartCoroutine(GetBaiduCoords(url));
                this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
                //this.gps_info = "N:" + Latitude + " E:" + Longitude;
                this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
                this.flash_num += 1;
                E1 = E - 119.983524f;
                N1 = N - 31.822732f;
                po.z = (float)(101.3479 + 16059.781718 * E1);
                po.x = (float)(278.0573 - 14633.887595 * N1);
                if (E - 119.987166f >= 0.0f)
                {
                    po.x -= 5.0f;
                    // po.z += 6.0f;
                    ToastState("进行了偏移！！");
                }

                po.y = M;
                people.position = po;
            }
        }*/


        /*if (flag)
        {*/
       /* f_curtime1 += Time.deltaTime;
        if (f_curtime1 >= 5.0f)
        {
            f_curtime += Time.deltaTime;
            if (f_curtime >= 5.0f)
            {
                string url = "http://api.map.baidu.com/geoconv/v1/?";
                url = url + "coords=" + Input.location.lastData.longitude.ToString() + "," + Input.location.lastData.latitude.ToString() + "&from=1&to=5&ak=kxBEdjniN2c3yZLIesNzTIRS";
                StartCoroutine(GetBaiduCoords(url));
                this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
                //this.gps_info = "N:" + Latitude + " E:" + Longitude;
                this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
                this.flash_num += 1;
                E1 = E - 119.983524f;
                N1 = N - 31.822732f;
                po.z = (float)(101.3479 + 16059.781718 * E1);
                po.x = (float)(278.0573 - 14633.887595 * N1);
                /*if (E - 119.987166f >= 0.0f)
                {
                    po.x -= 5.0f;
                    //po.z += 6.0f;
                    //ToastState("进行了偏移！！");
                }
                po.y = M;
                people.position = po;
                f_curtime = 0.0f;
            }
        }*/
            
        //}
    }

    // Input.location = LocationService
    // LocationService.lastData = LocationInfo 

    void StopGPS()
    {
        Input.location.Stop();
    }

    IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用
        if (!Input.location.isEnabledByUser)
        {
            this.gps_info = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用
        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            this.gps_info = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            this.gps_info = "Unable to determine device location";
            yield return false;
        }
        else
        {
            //Debug.Log();
            //  converToBaiduMap.ConverToBaidu(Input.location.lastData.longitude.ToString(), Input.location.lastData.latitude.ToString());
            //    converToBaiduMap.ConverToBaidu("119.9753", "31.82049");
            //  Longitude = converToBaiduMap.GetLongitude();
            //  Latitude = converToBaiduMap.GetLatitude();
            string url = "http://api.map.baidu.com/geoconv/v1/?";
            url = url + "coords=" + Input.location.lastData.longitude.ToString() + "," + Input.location.lastData.latitude.ToString() + "&from=1&to=5&ak=kxBEdjniN2c3yZLIesNzTIRS";
            StartCoroutine(GetBaiduCoords(url));
            // Debug.Log("Longitude = " + Longitude);
            //  Debug.Log("Latitude = " + Latitude);
            this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            // this.gps_info = "N:" + Latitude + " E:" + Longitude;
            this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
            yield return new WaitForSeconds(100);
        }
    }

    IEnumerator GetBaiduCoords(string url)
    {
        print("GetBaiduCoords");
        WWW www = new WWW(url);
        print("After WWW");
        yield return www;
        print("After Yield");
        if (www.error != null)
        {
            print("error");
            //GET请求失败
            Debug.Log("error is :" + www.error);
            //   ToastState("请求失败!!!");
        }
        else
        {
            print("Ok");
            //GET请求成功
            Debug.Log("request ok : " + www.text);
            // label = GameObject.Find("Label_text");

            string str = "";
            //截取JSON数据
            int i = 0;
            for (i = 0; i < www.text.Length; i++)
            {
                if (www.text[i] == '[')
                {
                    break;
                }
            }
            for (; i < www.text.Length - 1; i++)
            {
                str += www.text[i];
            }

            // label.GetComponent<UILabel>().text = str;
            print("str = " + str);
            Debug.Log("str = " + str);
            JsonData jd = JsonMapper.ToObject(str);
            print("x:=" + jd[0]["x"]);
            print("y = " + jd[0]["y"]);
            Longitude = jd[0]["x"].ToString();
            Latitude = jd[0]["y"].ToString();
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
