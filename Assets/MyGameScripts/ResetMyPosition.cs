using UnityEngine;
using System.Collections;

public class ResetMyPosition : MonoBehaviour {
    public UIGrid myGrid;
    private static int mychildCount = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    //目前只支持最多10个目标
	void Update () {
        if (myGrid.transform.childCount != 0) {
            int tmp = 1;
            
            foreach (Transform tra in myGrid.transform) {
                if (tra.name[0] >= '0' && tra.name[0] <= '9')
                {
                    string str = tmp + "";
                    int t = 0;
                    for (int k = 0; k < tra.name.Length; k++)
                    {
                        if (tra.name[k] < '0' || tra.name[k] > '9')
                        {
                            t = k;
                            break;
                        }
                    }
                    for (int k = t; k < tra.name.Length; k++)
                    {
                        str += tra.name[k];
                    }
                    tra.name = str;
                }
                else {
                    tra.name = tmp + "" + tra.name;
                }
              
                tmp++;
            }
        }
	}
}
