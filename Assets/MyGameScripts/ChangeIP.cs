using UnityEngine;
using System.Collections;

using System.IO;

public class ChangeIP : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIpInTabel()
    {

        HostIp hostIp = new HostIp();

        hostIp.clickTheButton();
        UILabel ChangeLocationIp = GameObject.Find("Label_LoactionIpDress").GetComponent<UILabel>();
        UILabel ChangeMainIp = GameObject.Find("Label_MainIpDress").GetComponent<UILabel>();

        ChangeLocationIp.text = HostIp.myLocationServer;
        ChangeMainIp.text = HostIp.serverLacation;

        print(HostIp.pathth);
    }

    public void ChangeConnectIpDress()
    {

        string ppath = Application.persistentDataPath + "//" + "IP.txt";

        StreamReader srlogin = new StreamReader(ppath);

        string msgs = srlogin.ReadToEnd();

        string[] info = File.ReadAllLines(ppath);

        HostIp hostIp = new HostIp();

        hostIp.clickTheButton();
        UILabel ChangeLocationIp = GameObject.Find("Label_LoactionIpDress").GetComponent<UILabel>();
        UILabel ChangeMainIp = GameObject.Find("Label_MainIpDress").GetComponent<UILabel>();
        HostIp.myLocationServer = ChangeLocationIp.text;
        HostIp.serverLacation = ChangeMainIp.text;

        info[1] = info[1].Replace(info[1], ChangeLocationIp.text);

        info[0] = info[0].Replace(info[0], ChangeMainIp.text);

        srlogin.Close();

        File.WriteAllLines(ppath, info);

        // File.WriteAllText(ppath, info);

        print(HostIp.pathth);
    }




}
