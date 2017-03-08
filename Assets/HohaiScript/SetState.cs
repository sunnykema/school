using UnityEngine;
using System.Collections;

public class SetState : MonoBehaviour {
    //状态的转化标志，在LoginAndRegister类中有调用
    public static bool IsChange = false;
  //  public GameObject LittleMap;
    public GameObject EasyTouch;
    public GameObject userLogin;
    public GameObject MainChoice;
    public GameObject otherFunction;

    /// <summary>
    /// 点击登录按钮
    /// </summary>
    public  void OnClickLoginButton() {
        if (!IsChange) {
            return;
        }
        IsChange = false;
        userLogin.gameObject.SetActive(false);
        EasyTouch.SetActive(true);
        MainChoice.SetActive(true);
        otherFunction.SetActive(true);

    }

}
