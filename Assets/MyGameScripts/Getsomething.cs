using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;

public class Getsomething : MonoBehaviour {
    public string[] targetPos; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void GetPosition(){
        UITable myTabel = GameObject.Find("MyGrid").GetComponent<UITable>();
        for (int i = 0; i < myTabel.transform.childCount;i++ ){
            //targetPos[i] = myTabel.transform.GetChild(i).gameObject;
        }
    }
}
