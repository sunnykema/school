/**
 * 
 * create ListView 动态的改变List view 中的值，飞机场中各个物体的详细参数。
 * 
 * */

using UnityEngine;
using System.Collections;
//using System.Collections.Generic.List;

public class AddItemByValueChange : MonoBehaviour {
	
	private const int maxLen = 250;
	//动态添加scrollView中的Cell
	public UITable table;
	int count = 0;
	
	private GameObject Label;
	void Start () {
		//得到游戏对象Grid节点中得UIGrid这个脚本
		table = GameObject.Find ("Grid").GetComponent<UITable>();
		//FixUpdate ();
	}
	
	
	// Update is called once per frame
	public void ChangeListView(){
		string [] Lno  = new string[maxLen]; 
		string [] Xaxis  = new string[maxLen]; 
		string [] Yaxis = new string[maxLen]; 
		string [] Zaxis  = new string[maxLen];
		string [] tName = new string[maxLen];
		string [] remarks = new string[maxLen];
        string[] ImgURL = new string[maxLen];
        string[] typename = new string[maxLen];
        string[] storey = new string[maxLen];
		//OnValueChange onValueChange = (OnValueChange)gameObject.GetComponent ("OnValueChange");
		print ("OnValueChange.cnt ="+ControlChange.cnt);
		//第一次刷新。。
		if(ControlChange.clickCount == 1){
			//清除该列表中原来的Child
			while (table.transform.childCount > 0)
			{
				DestroyImmediate(table.transform.GetChild(0).gameObject);
			}
			
			table.Reposition ();
			table.repositionNow = true;
		}
		//print ("grid.count before= "+list.Count);
		//print ("grid.transform.childCount ="+grid.transform.childCount);
		
		if(ControlChange.cnt == 0){
			print ("OnValueChange.cnt == 0 in AddItemByValueChange");
			return ;
		}
		Lno = ControlChange.Lno;
		Xaxis = ControlChange.Xaxis;
		Yaxis = ControlChange.Yaxis;
		Zaxis = ControlChange.Zaxis;
		tName = ControlChange.tName;
		remarks = ControlChange.remarks;
        ImgURL = ControlChange.ImgURL;
        typename = ControlChange.typename;
        storey = ControlChange.storey;
		int cnt = ControlChange.cnt;
		//ControlChange.cnt = 0;
		//print ("OnvalueChange.cnt after ="+ControlChange.cnt);
		for (int i = 0; i < cnt; i++) {
			//加载resources中的预制体 --- Instantiate(Resources.Load("预制体名字"))
			GameObject objectItem = (GameObject)Instantiate(Resources.Load("ItemLabel"));
			string str = Lno[i]; 
			objectItem.name = str;
			//向某个游戏对象节点中添加子节点
			objectItem.transform.parent = GameObject.Find ("Grid").transform;
			GameObject item = GameObject.Find (objectItem.name);
			item.transform.localPosition = new Vector3 (0,0,0);
			item.transform.localScale = new Vector3 (1,1,1);
			Label = GameObject.Find("Label");
			Label.name = objectItem.name;
			UILabel uiLabel = Label.GetComponent<UILabel>();
			uiLabel.text = tName[i];
			print ("uiLabel.text"+uiLabel.text);
			
            uiLabel = GameObject.Find("Position").GetComponent<UILabel>();
            uiLabel.name = "Position" + Lno[i];
            uiLabel.text = storey[i];

            uiLabel = GameObject.Find("Type").GetComponent<UILabel>();
            uiLabel.name = "Type" + Lno[i];
            uiLabel.text = typename[i];


			GameObject labelDes = GameObject.Find("Label - Description");
			print ("labelDes = "+labelDes);
			labelDes.name = objectItem.name+"0";
			
			UILabel des = labelDes.GetComponent<UILabel>();
			des.text = remarks[i]+","+Xaxis[i]+","+Yaxis[i]+","+Zaxis[i];
			print ("remarks = "+remarks[i]);
            //图片的URL
            WebImage webImg = GameObject.Find("Texture").GetComponent<WebImage>();
            webImg.name = "Texture" + Lno[i];
            webImg.url = ImgURL[i];

			count ++;
			//添加成功后，动态刷新listView
			table.repositionNow = true;
		}
		
	}
	
	/*
	public void changeDescription(){
		string [] remarks = new string[maxLen];

		remarks = ControlChange.remarks;
		GameObject labelDes = GameObject.Find("Label - Description");
		print ("labelDes = "+labelDes);
		//labelDes.name = remarks[0];
		
		UILabel des = labelDes.GetComponent<UILabel>();
		des.text = remarks[0];
		print ("remarks = "+remarks[0]);
	}
*/
	/*

	void OnClick()
	{
		//加载resources中的预制体 --- Instantiate(Resources.Load("预制体名字"))
		GameObject objectItem = (GameObject)Instantiate(Resources.Load("item"));
		objectItem.name = "item" + count;
		//向某个游戏对象节点中添加子节点
		objectItem.transform.parent = GameObject.Find ("Grid").transform;
		GameObject item = GameObject.Find (objectItem.name);
		item.transform.localPosition = new Vector3 (0,0,0);
		item.transform.localScale = new Vector3 (1,1,1);
		Label = GameObject.Find("Label");
		Label.name = objectItem.name;
		print ("Label name "+objectItem.name);
		count ++;
		//添加成功后，动态刷新listView
		grid.repositionNow = true;
	}
	*/
}