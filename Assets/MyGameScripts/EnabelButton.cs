using UnityEngine;
using System.Collections;
using System.Threading;
using System.Timers;
/// <summary>
/// 更改Gameobject的属性。。是否隐藏。
/// </summary>
public class EnabelButton : MonoBehaviour {
    public GameObject myButton;
    public GameObject OtherButton;
    private bool state = true;
    private bool otherState = false;

    //toast的时间戳
    private float ToastTime = 5f;
    private const float maxToastTime = 2f;
    private bool IsToastTime = false;

    System.Timers.Timer t = new System.Timers.Timer(10000); 
    /// <summary>
    /// 显示GameObject
    /// </summary>
    public void EnabelButtonSub() {
        myButton.SetActive(true);
    }
    /// <summary>
    /// 隐藏Gameobject
    /// </summary>
    public void DisabelButton() {
        OtherButton.SetActive(false);
    }
    /// <summary>
    /// 改变GameOb的状态，
    /// </summary>
    public void ChangeState()
    {

        if (state)
        {
           // return;
            myButton.SetActive(false);
            state = false;
        }
        else
        {
            myButton.SetActive(true);
            state = true;
        }

    }

    void Update()
    {
        if (!IsToastTime)
        {
            return;
        }
        if (ToastTime > maxToastTime)
        {
            myButton.SetActive(false);
            IsToastTime = false;
        }
        else
        {
            ToastTime = ToastTime + Time.deltaTime;

        }
    }

    //打印Toast
    public void ToastState()
    {
        ToastTime = 0;
        IsToastTime = true;
        print("ToastState Has Been safjk");
        myButton.SetActive(true);
        UILabel contentLabel = GameObject.Find("ContentLabel").GetComponent<UILabel>();
        contentLabel.text = "aslkjkl";


    }
}
