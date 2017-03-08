using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MarkerScript : MonoBehaviour {
	
	public float tension = 0.5f;
	public float ri;
	public float oldLeftIndent;
	public float li;
	public float oldRightIndent;
	public float rs;
	public float oldLeftSurrounding;
	public float ls;
	public float oldRightSurrounding;
	public float rt;
	public float oldLeftTilting;
	public float lt;
	public float oldRightTilting;
	public bool ssn;
	public bool oldSoftSelection;
	public float ssd;
	public float oldSoftSelDistance;
	public Transform[] sMs;
	public float[] trperc;
	public Vector3 oldPos = Vector3.zero;
	public bool autoUpdate;
	public bool changed;
	public Transform surface;
	public bool idi;
	
	Vector3 position;

	private bool updated;
	private int frameCount;
	private float currentstamp;
	private float newstamp;

	private bool mousedown;
	private Vector3 lookAtPoint;
	
	public bool bridgeObject;
	public  bool distHeights;

	public RoadObjectScript objectScript;

	void Start () {
		foreach(Transform child in transform) surface = child;
	}
	
	void OnDrawGizmos()
	{
		Vector3 change = transform.position - oldPos;		
		if(ssn && oldPos != Vector3.zero && change != Vector3.zero){
			int i = 0;
			foreach(Transform tr in sMs){
				tr.position += change * trperc[i];
				i++;
			}
		}
		if(oldPos != Vector3.zero && change != Vector3.zero){
			changed = true;
		}	
		oldPos = transform.position;
	}
	
	void SetObjectScript(){
		objectScript = transform.parent.parent.GetComponent<RoadObjectScript>();
		if(objectScript.cs == null) objectScript.a();
	}
	
	public void LeftIndent(float change, float perc){		
		ri += change * perc;
		if(ri < objectScript.indent) ri = objectScript.indent;
		oldLeftIndent = ri;
	}

	public void RightIndent(float change, float perc){		
		li += change * perc;
		if(li < objectScript.indent) li = objectScript.indent;
		oldRightIndent = li;
	}

	public void LeftSurrounding(float change, float perc){		
		rs += change * perc;
		if(rs < objectScript.indent) rs = objectScript.indent;
		oldLeftSurrounding = rs;
	}

	public void RightSurrounding(float change, float perc){		
		ls += change * perc;
		if(ls < objectScript.indent) ls = objectScript.indent;
		oldRightSurrounding = ls;
	}

	public void LeftTilting(float change, float perc){		
		rt += change * perc;
		if(rt < 0) rt = 0;
		oldLeftTilting = rt;
	}

	public void RightTilting(float change, float perc){		
		lt += change * perc;
		if(lt < 0) lt = 0;
		oldRightTilting = lt;
	}		
}

