using UnityEngine;
using System.Collections;

public class CallTwoDemen : MonoBehaviour {

	public void CallTwoDimension(){
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("Show");
		Debug.Log ("show succeed");
	}

	//这个是Android调用的方法  
	void GetString(string str)
	{
		data.val = str;
		//Application.LoadLevel(2);
		Debug.Log ("str = "+str);

		UILabel showCode = GameObject.Find ("ShowCode").GetComponent<UILabel> ();
		showCode.text = str;
		Debug.Log ("showCode.text = "+showCode.text);

	}  
}
