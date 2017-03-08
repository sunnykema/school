using UnityEngine;
using System.Collections;

public class TEST : MonoBehaviour {



    /*void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "test"))
        {
			print ("you click the button");
             AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			print ("1");
	         AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			print ("2");
             jo.Call("Show");
			print ("3");
        }
    }


    //这个是Android调用的方法  
    void GetString(string str)
    {
        data.val = str;
        Application.LoadLevel(2);
    }  
    */
    private string temp;
    private float[] arr = { 0, 0, 0 };
    private GameObject people;
    private Transform tran;
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
        temp = str;
        int k = 0;
        int flag = 0;
        string tmp = "";
        try
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (temp[i] == '[')
                {
                    continue;
                }
                if (temp[i] == ',' || temp[i] == ']')
                {
                    //print("tmp = "+tmp);
                    arr[k] = float.Parse(tmp);
                    tmp = "";
                    /*s
                    for (int j = i - 1; j >= 0; j--) {
                        if (temp[i] == '.')
                            break;
                        arr[k] += temp[i];
                        arr[k] /= 10;
                    }
                     * */
                    k++;
                }
                else
                    tmp += temp[i];
            }
        }
        catch { 
            
        }
        people = GameObject.Find("people");
        // print("People = "+people);
        tran = people.transform;
        Vector3 po;
        po = tran.position;
        ChangePosition changePosition = new ChangePosition();
        changePosition.ChangePeople(arr[0],arr[1],arr[2]);
        po.x = changePosition.GetX();
        po.y = changePosition.GetY();
        po.z = changePosition.GetZ();
       // po.x = 400 - arr[0] * 217;
       // po.y = arr[2] * 40 + 12;
     //   po.z = 225 - arr[1] * 180;
        people.transform.position = po;
        print(po.x);
        print(po.y);
        print(po.z);
	}  
}
