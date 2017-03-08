using UnityEngine;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

public class UserConfig : MonoBehaviour {
    public static string userName;
    void Start()
    {

        string ppath = Application.persistentDataPath + "//" + "UserInfo.txt";
        if (!File.Exists(ppath))
        {
            FileStream fs1 = new FileStream(ppath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
          //  sw.WriteLine("10.11.121.123:8080");//开始写入值
            sw.Write("null");
            sw.Close();
            fs1.Close();
        }
        StreamReader srlogin = new StreamReader(ppath);
        string msgs = srlogin.ReadToEnd();
        string[] info = File.ReadAllLines(ppath);
        srlogin.Close();
        userName = info[0];
       // myLocationServer = info[1];
        print("userName in UserConfig = " + userName);
      //  print("myLocationServer = " + myLocationServer);
    }

    public string clickTheButton()
    {

        string ppath = Application.persistentDataPath + "//" + "UserInfo.txt";
        if (!File.Exists(ppath))
        {
            FileStream fs1 = new FileStream(ppath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            //  sw.WriteLine("10.11.121.123:8080");//开始写入值
            sw.Write("null");
            sw.Close();
            fs1.Close();
        }
        StreamReader srlogin = new StreamReader(ppath);
        string msgs = srlogin.ReadToEnd();
        string[] info = File.ReadAllLines(ppath);
        srlogin.Close();
        userName = info[0];
        // myLocationServer = info[1];
        print("userName in UserConfig = " + userName);
        //  print("myLocationServer = " + myLocationServer);
        return userName;
    }
}
