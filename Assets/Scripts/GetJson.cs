using UnityEngine;
using System.Collections;

public class GetJson : MonoBehaviour {

	// Use this for initialization
    IEnumerator GETTest()
    {
        WWW w = new WWW("http://localhost:8001/home");
        yield return w;
        //通过&可以添加N多参数  

        //如果为查询，那么查询结果就会返回给w,通过获取w.text可以得到结果  
    }


    //在C#中进行POST查询,POST支持中文  
    IEnumerator POSTTest()
    {
        WWWForm wf = new WWWForm();
        wf.AddField("id", "我是");
        //wf.AddField可以继续添加N多参数  

        WWW w = new WWW("http://localhost:8001/home", wf);
        yield return w;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 100), "点击GET查询"))
        {
            StartCoroutine(GETTest());
        }
        if (GUI.Button(new Rect(0, 200, 200, 100), "点击POST查询"))
        {
            StartCoroutine(POSTTest());
        }
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
