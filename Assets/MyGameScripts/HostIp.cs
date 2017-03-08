using UnityEngine;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
/// <summary>
/// 服务器的IP
/// </summary>
public class HostIp : MonoBehaviour
{
    //public static string serverLacation = "10.11.12.123:8080";
    //public static string myLocationServer = "10.11.12.123:8001";
    public static string serverLacation;
    public static string myLocationServer;
    // print("serverLacation = "serverLacation);
    // print("");
    public static string pathth;
     void Start()
    {

        string ppath = Application.persistentDataPath + "//" + "IP.txt";
        if (!File.Exists(ppath))
        {
            FileStream fs1 = new FileStream(ppath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine("10.11.121.123:8080");//开始写入值
            sw.Write("10.11.121.123:8001");
            sw.Close();
            fs1.Close();
        }
        StreamReader srlogin = new StreamReader(ppath);
        string msgs = srlogin.ReadToEnd();
        string[] info = File.ReadAllLines(ppath);
        srlogin.Close();
        serverLacation = info[0];
        myLocationServer = info[1];
        print("serverLacation = " + serverLacation);
        print("myLocationServer = " + myLocationServer);
    }

    public void clickTheButton()
    {

        string ppath = Application.persistentDataPath + "//" + "IP.txt";
        if (!File.Exists(ppath))
        {
            FileStream fs1 = new FileStream(ppath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine("10.11.121.123:8080");//开始写入值
            sw.Write("10.11.121.123:8001");
            sw.Close();
            fs1.Close();
        }
        StreamReader srlogin = new StreamReader(ppath);
        string msgs = srlogin.ReadToEnd();
        string[] info = File.ReadAllLines(ppath);
        srlogin.Close();
        serverLacation = info[0];
        myLocationServer = info[1];
        print("serverLacation = " + serverLacation);
        print("myLocationServer = " + myLocationServer);
    }


}
