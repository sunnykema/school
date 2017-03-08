using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.IO;

public class LoginAndRegister : MonoBehaviour {
    public GameObject login;
    public GameObject UserInfoCancel;
    
    //改变状态
    public GameObject EasyTouch;
    public GameObject userLogin;
    public GameObject MainChoice;
    public GameObject otherFunction;
    public GameObject userRegister;
    public GameObject userCancelReturn;

    string serverLacation = HostIp.serverLacation;
    string myLocationServer = HostIp.myLocationServer;
    private string url;
    //检查注册页面输入的用户名是否已存在
    private bool checkName = false;


    public GameObject ToastLabel;

    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 3f;
    private bool IsToastTime = false;

    //用户的配置信息
    UserConfig userConfig;
    string UserName;
    /*
    MyAndroidToast toast;
    void Start(){
        toast = new MyAndroidToast();
    }
*/
    void Start() {
        userConfig = new UserConfig();
    }
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
	

    //获得IP地址
    void GetMyIp()
    {
        serverLacation = HostIp.serverLacation;
        myLocationServer = HostIp.myLocationServer;
    }

    /// <summary>
    /// 登陆按钮
    /// </summary>
    public void OnLoginClick() {
       // print("dengsajkh");
        GetMyIp();
        url = "http://" + serverLacation + "/SchoolWander/servlet/UserLogin";
        print("登陆");
        //注册请求 POST		
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string userName = GameObject.Find("LoginAndRegister_userName").GetComponent<UIInput>().value;
        UserName = userName;
        string userPwd = GameObject.Find("LoginAndRegister_userPwd").GetComponent<UIInput>().value;
        if (userName == "" || userPwd == "") {
            print("用户名和密码不能为空!");
            ToastState("用户名和密码不能为空！！");
            return;
        }

        print("userName = "+userName);
        print("userPwd = "+userPwd);
        dic.Add("userName",userName);
        dic.Add("userPwd",userPwd);
        StartCoroutine(POST(url,dic));
    }
    /*
    /// <summary>
    /// 登陆界面的注册账号按钮
    /// </summary>
    public void OnRegisterClick() {
        login.SetActive(false);
        register.SetActive(true);
    }
    */


    /// <summary>
    /// 注册页面的注册按钮
    /// </summary>
    public void OnRegisterNewClick() {
        GetMyIp();
        string userName = GameObject.Find("My_UserRegister_userName").GetComponent<UIInput>().value;
        string pwd = GameObject.Find("My_UserRegister_userPwd").GetComponent<UIInput>().value;
        string check_pwd = GameObject.Find("My_UserRegister_userPwdAg").GetComponent<UIInput>().value;
        if (userName == "" || pwd == "" || check_pwd == "") {
            print("用户名密码不能为空！！");
            ToastState("用户名密码不能为空！！");
            return;
        }

        if (!pwd.Equals(check_pwd)) {
            print("两次输入的密码不同！！！");
            ToastState("两次输入的密码不同！！！");
            return;
        }

        url = "http://" + serverLacation + "/SchoolWander/servlet/UserRegister";
        Dictionary<string,string> dic = new Dictionary<string,string>();
        dic.Add("userName",userName);
        dic.Add("userPwd",pwd);
        StartCoroutine(PostRegister(url,dic));
    }
    /// <summary>
    /// userName的值变化时
    /// </summary>
    public void OnNameValueChange() {
        //标识符要置为false
       // print("OnNameValueChange");
        checkName = false;
    }

    /// <summary>
    /// 验证是否存在该用户名
    /// </summary>
    public void OnchangeName() {
        GetMyIp();
        //如果之前已经检测过，则返回
        if (checkName) {
            return;
        }
        print("onchangename");
        string userName = GameObject.Find("My_UserRegister_userName").GetComponent<UIInput>().value;
        Dictionary<string,string> dic = new Dictionary<string,string>();
        url = "http://" + serverLacation + "/SchoolWander/servlet/CheckuserName";
        dic.Add("userName",userName);
        StartCoroutine(PostCheckuserName(url,dic));
    }
    /// <summary>
    /// 检测是否存在相同的用户名
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    IEnumerator PostCheckuserName(string url, Dictionary<string, string> post) {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("error is :" + www.error);
        }
        else
        {
            Debug.Log("request ok : " + www.text);
            if (www.text.Trim().Equals("yes"))
            {
                print("可以注册该用户名！");
               
                //标识为已检测
                checkName = true;
            }
            else {
                print("已存在该用户名!");
                ToastState("已存在该用户名!!");
            }
        }
    
    }



    /// <summary>
    /// post 请求,,登陆
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <returns></returns>
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
            Debug.Log("error is :" + www.error);
        }
        else
        {
            Debug.Log("request ok : " + www.text);
            if (www.text.Trim().Equals("yes"))
            {
                print("登陆成功!");
                ToastState("登录成功!!");
                OnClickLoginButtonForChangeState();// 返回主界面
                ChangeUserInfo(); //更改用户
            }
            else {
                print("登陆失败!");
                ToastState("用户名或密码错误!!");
            }
        }

    }

    /// <summary>
    /// 注册帐户Post
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    IEnumerator PostRegister(string url, Dictionary<string, string> post) {
        WWWForm form = new WWWForm();
        foreach(KeyValuePair<string ,string> post_arg in post){
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("error is :" + www.error);
        }
        else
        {
            Debug.Log("request ok : " + www.text);
            if (www.text.Trim().Equals("yes"))
            {
                print("注册成功!!!");
                ToastState("注册成功!!");
                userRegister.transform.GetComponent<UIPlayTween>().Play(true);
                OnClickRegisterButtonForChangeState();
            }
            else {
                print("注册失败！");
                ToastState("注册失败!!");
            }
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

    /// <summary>
    /// 点击登录按钮
    /// </summary>
    void OnClickLoginButtonForChangeState()
    {
        userLogin.SetActive(false);
        EasyTouch.SetActive(true);
        MainChoice.SetActive(true);
        otherFunction.SetActive(true);

    }
    /// <summary>
    /// 点击注册按钮
    /// </summary>
    void OnClickRegisterButtonForChangeState() {
        userRegister.SetActive(false);
        userLogin.SetActive(true);
        print("213123");
    }

    /// <summary>
    /// 更改用户帐户
    /// </summary>
    void ChangeUserInfo()
    {

        string ppath = Application.persistentDataPath + "//" + "UserInfo.txt";

        StreamReader srlogin = new StreamReader(ppath);

        string msgs = srlogin.ReadToEnd();

        string[] info = File.ReadAllLines(ppath);

        HostIp hostIp = new HostIp();

        userConfig.clickTheButton();

        //string userName = GameObject.Find("My_UserRegister_userName").GetComponent<UIInput>().value;
        //info[1] = info[1].Replace(info[1], ChangeLocationIp.text);

        info[0] = info[0].Replace(info[0], UserName);

        srlogin.Close();

        File.WriteAllLines(ppath, info);

        // File.WriteAllText(ppath, info);

        //print(HostIp.pathth);
    }

    /// <summary>
    /// 注销用户
    /// </summary>
    public void CancelUser() {
        //print("asfdkjash");
        string ppath = Application.persistentDataPath + "//" + "UserInfo.txt";

        StreamReader srlogin = new StreamReader(ppath);

        string msgs = srlogin.ReadToEnd();

        string[] info = File.ReadAllLines(ppath);

        HostIp hostIp = new HostIp();

        userConfig.clickTheButton();

        //string userName = GameObject.Find("My_UserRegister_userName").GetComponent<UIInput>().value;
        //info[1] = info[1].Replace(info[1], ChangeLocationIp.text);

        info[0] = info[0].Replace(info[0], "null");

        srlogin.Close();

        File.WriteAllLines(ppath, info);

        // File.WriteAllText(ppath, info);

    //    print(HostIp.pathth);
      
    }
    /// <summary>
    /// 注销用户后，返回主界面
    /// </summary>
    public void CancelUserReturn() {
        UIPlayTween[] uiPlayTween;
        uiPlayTween = userCancelReturn.transform.GetComponents<UIPlayTween>();
        int count = 0;
        foreach (UIPlayTween playTween in uiPlayTween)
        {
            playTween.Play(true);
            count++;
        }
        print("count = " + count);

    }
    /// <summary>
    /// 用户登录的信息，是否已经登录.
    /// </summary>
    public void OnclickuserInfo() {
        string myUsername = userConfig.clickTheButton();
        print("myUserName = " + myUsername);
        UIPlayTween[] uiPlayTween;
        if (myUsername.Equals("null"))
        {
            print("name is null");
            uiPlayTween =  login.transform.GetComponents<UIPlayTween>();
            int count = 0;
            foreach (UIPlayTween playTween in uiPlayTween) {
                playTween.Play(true);
                count++;
            }
            print("count = "+count);
        }
        else { 
            print("name is not null");
            uiPlayTween = UserInfoCancel.transform.GetComponents<UIPlayTween>();
            int count = 0;
            foreach (UIPlayTween playTween in uiPlayTween)
            {
                playTween.Play(true);
                count++;
            }
            EasyTouch.SetActive(false);
            print("count ="+count);
            UILabel myLabel = GameObject.Find("Label_UserInfoForCancel_userName").transform.GetComponent<UILabel>();
            myLabel.text = myUsername;
            print("myLabel = "+myLabel.text);
        }
    }

    /// <summary>
    /// 解决小地图不能消失的BUG
    /// </summary>
    public void CloseEasyTouch() {
        EasyTouch.SetActive(false);
    }
}
