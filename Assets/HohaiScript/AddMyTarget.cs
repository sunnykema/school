using UnityEngine;
using System.Collections;

public class AddMyTarget : MonoBehaviour {

	//public string[] TargetLocation;
	// Use this for initialization
	public static bool Is_AddTarget;
	private float f_curtime;
	public static int Target_Count;
    private float M = 5.6f;
	void Start()
	{
		Is_AddTarget = false;
		f_curtime = 0.0f;
		Target_Count = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	Vector3 po;
	public void Clear()         //清空当前father存在的子物体，重新规划路线
	{
		Transform people;
		people = GameObject.Find("father").transform;
		foreach (Transform child in people)
		{
			child.parent = null;
		}
		KGFMapSystem.DeletePoints();
		/*
        foreach (Transform aUserFlagTransform in KGFMapSystem.itsContainerFlags)
        {
            GameObject.Destroy(aUserFlagTransform.gameObject);
            //print("jjjj");
        }*/
	}
	public void AddTargetLocation()
	{
		UIGrid myGrid = GameObject.Find("MyGrid").GetComponent<UIGrid>();
		print("myGrid.transform.childCount = " + myGrid.transform.childCount);
		/*GameObject temp = myGrid.transform.GetChild(0).gameObject;
        GameObject father = (GameObject)Instantiate(Resources.Load("Door"));
        father.name = myGrid.transform.GetChild(i).gameObject.name + "A";*/
		//GameObject father;
		//myGrid.Reposition(myGrid.GetChildList());
		/* myGrid.Reposition();
        for (int k = 0; k < myGrid.transform.childCount;k++ )
        {
            print("k = "+myGrid.transform.GetChild(k).name);
           
        }*/
		Clear();
		Target_Count = myGrid.transform.childCount;
		for (int i = 0; i < myGrid.transform.childCount; i++)       //所有的目标
		{
			//print("***" + myGrid.transform.GetChild(i).gameObject.name);
			
			//string str = myGrid.transform.GetChild(i).gameObject.name + "0";
			string str = "";
			int t = 0;
			//除掉前面的数字
			for (int k = 0; k < myGrid.transform.GetChild(i).gameObject.name.Length; k++)
			{
				if (myGrid.transform.GetChild(i).gameObject.name[k] < '0' || myGrid.transform.GetChild(i).gameObject.name[k] > '9')
				{
					t = k;
					break;
				}
			}
			//找出后面真正的名字
			for (int k = t; k < myGrid.transform.GetChild(i).gameObject.name.Length; k++) {
				str += myGrid.transform.GetChild(i).gameObject.name[k];
			}
			str = str + "0";
			print("string = " + str);
			UILabel targetLabel = GameObject.Find(str).GetComponent<UILabel>();
			
			string tmp = "";
			tmp = targetLabel.text;
			
			float[] arr = new float[3];
			int cnt = 0;
			int j = 0;
			string myStr = "";
			while (tmp[j] != ',')
			{
				j++;
			}
			for (j++; j < tmp.Length; j++)
			{
				if (tmp[j] == ',' || j == tmp.Length - 1)
				{
					if (j == tmp.Length - 1)
					{
						myStr += tmp[j];
					}
					arr[cnt] = float.Parse(myStr);
					print("arr = " + myStr);
					cnt++;
					myStr = "";
				}
				else
				{
					myStr += tmp[j];
				}
			}
			//load perferb 
			GameObject myTarget = (GameObject)Instantiate(Resources.Load("Door"));
			/*if (i == 0)
                myTarget.name = "father";
            else*/
			myTarget.name = myGrid.transform.GetChild(i).gameObject.name + "A";
			print("arr[0] = "+arr[0]);
			print("arr[1] = "+arr[1]);
			print ("arr[2] = "+arr[2]);
			//ChangePosition changePosition = new ChangePosition();
			//changePosition.ChangePeople(arr[0],arr[1],arr[2]);
			po.x = arr[0];
			po.y = M;
			po.z = arr[1];
			KGFMapSystem.Target_Flag[i] = po;
			/*
            if (arr[2] - 0.0 < 0.1)
            {
                po.x = 369 - arr[0] * 173;
                po.y = arr[2] * 40 + 12;
                po.z = 190 - arr[1] * 120;
            }
            else
            {
                po.x = 369 - arr[0] * 173;
                po.y = arr[2] * 40 + 12;
                po.z = 190 - arr[1] * 120;
            }
             * */
			myTarget.transform.position = po;
			//if(i > 0)
            print("Add");
			myTarget.transform.parent = GameObject.Find("father").transform;
			Is_AddTarget = true;
            print("Add!!!");
            print("Is_AddTarget = " + Is_AddTarget);
			//print("myTarget.transform.parent = " + myTarget.transform.parent);
		}
		//print("target childCount = " + GameObject.Find("target").transform.childCount);
		//AIPath.FindFather();
		//BotAI.IsOk = true;
		AIPath.IsOk = true;
	}
	//将小地图上的红色标记位置加入到target中
	public void AddNear() {
		// Clear();
		//Is_AddTarget = true;
		Vector3 po;
		po = KGFMapSystem.Near;
		//Grid myGrid = GameObject.Find("MyGrid").GetComponent<UIGrid>();
		//string str = myGrid.transform.GetChild(i).gameObject.name + "0";
		// UILabel targetLabel = GameObject.Find(str).GetComponent<UILabel>();
		GameObject myTarget = (GameObject)Instantiate(Resources.Load("Door"));
		/*if (i == 0)
            myTarget.name = "father";
        else*/
		//myTarget.name = myGrid.transform.GetChild(i).gameObject.name + "A";
		Transform person = GameObject.Find("people").transform;
		po.y = person.position.y - 1.8f;
		myTarget.name = myTarget.name + "A";
		myTarget.transform.position = po;
		myTarget.transform.parent = GameObject.Find("father").transform;
		print("It has been invoked！！！");
	}
	/// <summary>
	/// 生成到达的飞机行李盘的实物。
	/// </summary>
	public void GetLineForFlightIn() {
		Clear();
		Is_AddTarget = true;
		float x, y, z;
		x = float.Parse(ControlChange.Xaxis[0]);
		y = float.Parse(ControlChange.Yaxis[0]);
		z = float.Parse(ControlChange.Zaxis[0]);
		ChangePosition changePosition = new ChangePosition();
		changePosition.ChangePeople(x, y, z);
		po.x = changePosition.GetX();
		po.y = changePosition.GetY();
		po.z = changePosition.GetZ();
		
		//load perferb 
		GameObject myTarget = (GameObject)Instantiate(Resources.Load("Door"));
		myTarget.name = ControlChange.Lno[0];
		myTarget.transform.position = po;
		myTarget.transform.parent = GameObject.Find("father").transform;
	}
	
	/// <summary>
	/// 生成值机柜台和登机口的实物
	/// </summary>
	public void GetLineForFlightOut() {
		Clear();
		Is_AddTarget = true;
		int cnt = ControlChange.cnt;
		for (int i = 0; i < cnt; i++)
		{
			float x, y, z;
			x = float.Parse(ControlChange.Xaxis[i]);
			y = float.Parse(ControlChange.Yaxis[i]);
			z = float.Parse(ControlChange.Zaxis[i]);
			ChangePosition changePosition = new ChangePosition();
			changePosition.ChangePeople(x, y, z);
			po.x = changePosition.GetX();
			po.y = changePosition.GetY();
			po.z = changePosition.GetZ();
			
			//load perferb 
			GameObject myTarget = (GameObject)Instantiate(Resources.Load("Door"));
			myTarget.name = ControlChange.Lno[i];
			myTarget.transform.position = po;
			myTarget.transform.parent = GameObject.Find("father").transform;
		}
	}
	
	public void IsNav() {
		AIPath.IsNav = true;
	}
	
	public void IsLine() {
		AIPath.IsNav = false;
	}
}
