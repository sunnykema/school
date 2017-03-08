using UnityEngine;
using System.Collections;

public class NotificationCenter : MonoBehaviour
{
    public static NotificationCenter Instance;

    public GameObject notificationPrefab;
    void Awake()
    {
        Instance = this;
    }

    public void Add(AndroidToast.NotificationStyle notificationStyle, string guiTextContent)
    {
        print("!!!!!!");
        GameObject go = Instantiate(notificationPrefab) as GameObject;
       // GameObject myTarget = (GameObject)Instantiate(Resources.Load("Door"));
        print("*****");
        go.GetComponent<AndroidToast>().currentNotificationStyle = notificationStyle;
        go.guiText.text = guiTextContent;
    }
}