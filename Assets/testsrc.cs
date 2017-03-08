using UnityEngine;
using System.Collections;

public class testsrc : MonoBehaviour {

    public string value = "http://blog.csdn.net/dingxiaowei2013";
    void Start()
    {
        value = data.val;
    }

	void OnGUI()
    {
        GUI.Label(new Rect(20, 50, 700, 20), value);
    }
}
