using UnityEngine;
using System.Collections;

public class Show_Info : MonoBehaviour {

	// Use this for initialization
	public GameObject Show_lable;
	public UILabel Lable;
	void Start () {
	
	}
	void OnMouseDown()          //判断鼠标是否已经点击电梯门按钮
	{
		/*string s = "2F	外语教学部办公室 \n" +
						"2F	人文社科部办公室\n" +
						"2F	创新实验室 \n" +
						"2F	机电工程学院研究生学习室 \n" +
						"3F	机电工程学院行政办公区 \n" +
						"3F	外语教学部办公室 \n";*/
		string s1 ="学生参考阅览室：\n" +
			"星期一 —— 星期五	\n" +
			"8：00-22：00\n" +
			"星期六 —— 星期日\n" +
			"8：00-12：00  \n   " +
			"13：00-17：00\n" +
			"现刊阅览室：\n" +
			"星期一 —— 星期四 星期六\n" +
			"8：30-12：00\n" +
			"13：30-17：00\n" +
			"18：30-22：00\n" +
			"星期五 星期日\n" +
			"8：30-12：00\n" +
			"13：30-17：00\n" +
			"星期三下午不开放\n";
		Lable.text = s1;
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
