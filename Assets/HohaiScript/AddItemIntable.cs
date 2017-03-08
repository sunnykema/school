using UnityEngine;
using System.Collections;

public class AddItemIntable : MonoBehaviour {

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
	//	string[] storey = new string[maxLen];
		//OnValueChange onValueChange = (OnValueChange)gameObject.GetComponent ("OnValueChange");
		print ("OnValueChange.cnt ="+LineSer.cnt);
		//第一次刷新。。
		if(LineSer.clickCount == 1){
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
		
		if(LineSer.cnt == 0){
			print ("OnValueChange.cnt == 0 in AddItemByValueChange");
			return ;
		}
		Lno = LineSer.Lno;
		Xaxis = LineSer.Xaxis;
		Yaxis = LineSer.Yaxis;
		Zaxis = LineSer.Zaxis;
		tName = LineSer.tName;
		remarks = LineSer.remarks;
		ImgURL = LineSer.ImgURL;
		typename = LineSer.typename;
		//storey = LineSer.storey;
		int cnt = LineSer.cnt;
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
			/*
			uiLabel = GameObject.Find("Position").GetComponent<UILabel>();
			uiLabel.name = "Position" + Lno[i];
			uiLabel.text = storey[i];
			*/
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

}
