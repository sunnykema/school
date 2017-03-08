/**
 * 
 * 动态的添加航班信息。。
 * 
 * 
 * 
 * 
 * */


using UnityEngine;
using System.Collections;

public class AddFlight : MonoBehaviour {
    public const int maxLen = ControlChange.maxLen;

    public UIGrid myGrid;
	// Use this for initialization
	void Start () {
        myGrid = GameObject.Find("Grid_Flight").GetComponent<UIGrid>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //flight 的信息

    public  string[] Ano = new string[maxLen];
    public  string[] FromPlace = new string[maxLen];
    public  string[] ToPlace = new string[maxLen];
    public  string[] PlanttoArrive = new string[maxLen];
    public  string[] state = new string[maxLen];
    public  string[] luggage = new string[maxLen];

    public  string[] PlantoLaunch = new string[maxLen];
    public  string[] Gates = new string[maxLen];
    public  string[] checking = new string[maxLen];
    public  string[] counter = new string[maxLen];


	public static bool IsOut = false;

    public GameObject FlightSer;
    public GameObject MainScenese;
    public GameObject MyEasyTouch;

	public void IsActiveForOut(){
		if (UIToggle.current.value) {
			IsOut = true;
			//UIToggle.current.value = false;
			print ("out value is true");		
		} else {
			IsOut = false;
			print ("out value is false");

			UIToggle uiToggle = GameObject.Find("Checkbox_In").GetComponent<UIToggle>();
			uiToggle.value = true;
		}
	}


	public void IsActiveForIn(){
		if (UIToggle.current.value) {
			print ("In value is true");		
			IsOut = false;
		} else {
			print ("In value is false");
			UIToggle uiToggle = GameObject.Find("Checkbox_Out").GetComponent<UIToggle>();
			uiToggle.value = true;
			IsOut = true;
		}
	}


	public void ChangeFlightIn(){


        //清除该列表中原来的Child
        while (myGrid.transform.childCount > 0)
        {
            DestroyImmediate(myGrid.transform.GetChild(0).gameObject);
        }

        myGrid.Reposition();
        myGrid.repositionNow = true;

        if (ControlChange.cnt == 0)
        {
            print("OnValueChange.cnt == 0 in ChangeFlightIn");
            return;
        }

        Ano = ControlChange.Ano;
        FromPlace = ControlChange.FromPlace;
        ToPlace = ControlChange.ToPlace;
        PlanttoArrive = ControlChange.PlanttoArrive;
        state = ControlChange.state;
        luggage = ControlChange.luggage;


        int cnt = ControlChange.cnt;
        
        for (int i = 0; i < cnt; i++)
        {
            //加载resources中的预制体 --- Instantiate(Resources.Load("预制体名字"))
            GameObject objectItem = (GameObject)Instantiate(Resources.Load("FlightIn"));
            string str = Ano[i];
            objectItem.name = str;
            //向某个游戏对象节点中添加子节点
            objectItem.transform.parent = GameObject.Find("Grid_Flight").transform;
            GameObject item = GameObject.Find(objectItem.name);
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale = new Vector3(1, 1, 1);

            UILabel tmp;
            tmp = GameObject.Find("ArriveCity").GetComponent<UILabel>();
            tmp.name = "ArriveCity"+Ano[i];
            tmp.text = ToPlace[i];
            tmp = GameObject.Find("StartCity").GetComponent<UILabel>();
            tmp.name = "StartCity"+Ano[i];
            tmp.text = FromPlace[i];
            print("a xi ba");
            tmp = GameObject.Find("PlanArriveTime").GetComponent<UILabel>();
            tmp.name = "PlanArriveTime"+Ano[i];
            tmp.text = PlanttoArrive[i];
            tmp = GameObject.Find("Luggage").GetComponent<UILabel>();
            tmp.name = "Luggage"+Ano[i];
            tmp.text = luggage[i];

            tmp = GameObject.Find("FlightName").GetComponent<UILabel>();
            tmp.name = Ano[i];
            tmp.text = Ano[i];

            GameObject bgImg = GameObject.Find("imgBG");
            //print("BgImg = "+bgImg);
            bgImg.name = Ano[i];

            UIPlayTween[] playTween = bgImg.GetComponents<UIPlayTween>();
            playTween[0].tweenTarget = FlightSer;
            playTween[1].tweenTarget = MainScenese;
            print("mainScense = " + MainScenese);
            playTween[2].tweenTarget = MyEasyTouch;
            
            myGrid.repositionNow = true;
            /*
            Label = GameObject.Find("Label");
            Label.name = objectItem.name;
            UILabel uiLabel = Label.GetComponent<UILabel>();
            uiLabel.text = objectItem.name + "," + tName[i];
            print("uiLabel.text" + uiLabel.text);

            GameObject labelDes = GameObject.Find("Label - Description");
            print("labelDes = " + labelDes);
            labelDes.name = objectItem.name + "0";

            UILabel des = labelDes.GetComponent<UILabel>();
            des.text = remarks[i] + "," + Xaxis[i] + "," + Yaxis[i] + "," + Zaxis[i];
            print("remarks = " + remarks[i]);
            count++;
            //添加成功后，动态刷新listView
            table.repositionNow = true;
             * */
        }
        

	}

    public void ChangeFlightOut() {


        //清除该列表中原来的Child
        while (myGrid.transform.childCount > 0)
        {
            DestroyImmediate(myGrid.transform.GetChild(0).gameObject);
        }

        myGrid.Reposition();
        myGrid.repositionNow = true;

        if (ControlChange.cnt == 0)
        {
            print("OnValueChange.cnt == 0 in ChangeFlightOut");
            return;
        }

        Ano = ControlChange.Ano;
        FromPlace = ControlChange.FromPlace;
        ToPlace = ControlChange.ToPlace;
        PlantoLaunch = ControlChange.PlantoLaunch;
        state = ControlChange.state;
        Gates = ControlChange.Gates;
        checking = ControlChange.checking;
        counter = ControlChange.counter;

        int cnt = ControlChange.cnt;
        
        for (int i = 0; i < cnt; i++)
        {
            //加载resources中的预制体 --- Instantiate(Resources.Load("预制体名字"))
            GameObject objectItem = (GameObject)Instantiate(Resources.Load("Flight"));
            string str = Ano[i];
            objectItem.name = str;
            //向某个游戏对象节点中添加子节点
            objectItem.transform.parent = GameObject.Find("Grid_Flight").transform;
            GameObject item = GameObject.Find(objectItem.name);
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale = new Vector3(1, 1, 1);
            UILabel tmp;
            tmp = GameObject.Find("ArriveCity").GetComponent<UILabel>();
            tmp.name = "ArriveCity"+Ano[i];
            tmp.text = ToPlace[i];
            tmp = GameObject.Find("StartCity").GetComponent<UILabel>();
            tmp.name = "StartCity"+Ano[i];
            tmp.text = FromPlace[i];
            tmp = GameObject.Find("StartTime").GetComponent<UILabel>();
            tmp.name = "StartTime"+Ano[i];
            tmp.text = PlantoLaunch[i];
            tmp = GameObject.Find("gateName").GetComponent<UILabel>();
            tmp.name = "gateName"+Ano[i];
            tmp.text = Gates[i];

            tmp = GameObject.Find("FlightName").GetComponent<UILabel>();
            tmp.name = Ano[i];
            tmp.text = Ano[i];
            tmp = GameObject.Find("CUSSName").GetComponent<UILabel>();
            tmp.name = "CUSSName"+Ano[i];
            tmp.text = counter[i];


            GameObject bgImg = GameObject.Find("imgPlane");
            bgImg.name = Ano[i];
            
            UIPlayTween [] playTween = bgImg.GetComponents<UIPlayTween>();
            playTween[0].tweenTarget = FlightSer;
            playTween[1].tweenTarget = MainScenese;
            print("mainScense = " + MainScenese);
            playTween[2].tweenTarget = MyEasyTouch;
           
             myGrid.repositionNow = true;
            /*
            Label = GameObject.Find("Label");
            Label.name = objectItem.name;
            UILabel uiLabel = Label.GetComponent<UILabel>();
            uiLabel.text = objectItem.name + "," + tName[i];
            print("uiLabel.text" + uiLabel.text);

            GameObject labelDes = GameObject.Find("Label - Description");
            print("labelDes = " + labelDes);
            labelDes.name = objectItem.name + "0";

            UILabel des = labelDes.GetComponent<UILabel>();
            des.text = remarks[i] + "," + Xaxis[i] + "," + Yaxis[i] + "," + Zaxis[i];
            print("remarks = " + remarks[i]);
            count++;
            //添加成功后，动态刷新listView
            table.repositionNow = true;
             */
        }
        
    }
}
