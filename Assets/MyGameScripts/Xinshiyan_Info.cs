using UnityEngine;
using System.Collections;

public class Xinshiyan_Info : MonoBehaviour {

	public GameObject Show_lable;
	public UILabel Lable;
	void Start () {
		
	}
	void OnMouseDown()          //判断鼠标是否已经点击电梯门按钮
	{
		string s = "102	值班室\n" +
			"103	省、市重点实验室办公室\n" +
			"104	电力电子与运动控制实验室\n" +
			"105	常州市传感网与环境感知重点实验室\n" +
			"106	气体绝缘金属开关柜实验室\n" +
			"107	输配电装备状态监测与电工理论方向实验室\n" +
			"108	特种变压器实验室\n";

		Lable.text = s;
		Show_lable.SetActive (true);
		print ("It has been cliked!!");
	}
	
	public void Return()
	{
		Show_lable.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
