using UnityEngine;
using System.Collections;

public class MyAndroidToast : MonoBehaviour {
    public  GameObject  ToastLabel;
    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 3f;
    private static bool IsToastTime = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
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

    //打印Toast
    public void ToastState(string str)
    {
        ToastLabel = GameObject.Find("Toast"); 
        ToastTime = 0;
        IsToastTime = true;
        print("isToastTime = "+IsToastTime);
        print("ToastState Has Been safjk");
        ToastLabel.SetActive(true);
        UILabel contentLabel = GameObject.Find("ContentLabel").GetComponent<UILabel>();
        contentLabel.text = str;
    }
}
