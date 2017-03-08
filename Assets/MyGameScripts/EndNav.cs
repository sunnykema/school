using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using System;

public class EndNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClickNavButton() { 
        print("you click the EndNav Button!!!");
     //   ControlChange end = new ControlChange();
      //  end.GetMyLocation();
        AIPath.IsNav = false;
        BotAI people = GameObject.Find("people").GetComponent<BotAI>();
        people.ExchangeChild();
        people.OnTargetReached();

        GameObject endNav = GameObject.Find("EndNav");
        endNav.SetActive(false);
    }
}
