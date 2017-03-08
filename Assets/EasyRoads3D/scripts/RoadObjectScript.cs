using UnityEngine;
using System.Collections;
using System;
using EasyRoads3D;

public class RoadObjectScript : MonoBehaviour {
	
	public bool displayRoad = true;
	public float roadWidth = 5.0f;
	public float indent = 3.0f;
	public float surrounding = 15.0f;
	public float raise = 1.0f;
	public float raiseMarkers = 0.5f;
	public bool closedTrack = false;
	public bool renderRoad = true;
	public bool beveledRoad = false;
	public bool applySplatmap = false;
	public int splatmapLayer = 4;
	public bool autoUpdate = true;
	public float geoResolution = 5.0f;
	public int roadResolution = 1;
	public float tuw =  15.0f;
	public int splatmapSmoothLevel;
	public float opacity = 1.0f;
	public int expand = 0;
	public int offsetX = 0;
	public int offsetY = 0;
	private Material surfaceMaterial;
	public float surfaceOpacity = 1.0f;
	public float smoothDistance = 1.0f;
	public float smoothSurDistance = 3.0f;
	private bool handleInsertFlag;
	public bool hd1 = true;
	public float td1 = 2.0f;
	public float dd1 = 1f;
	public int materialType = 1;
	String[] materialStrings;
	private MarkerScript[] mSc;
	private bool am;
	private bool[] ms = null;
	private bool[] oms = null;
	public string[] mS;
	public string[] tL;
	public int[] tLInt;
	public int dS = -1;
	public int cS = -1; 
	static public GUISkin cGS;
	static public GUISkin dGS;
	public bool gE = false;
	private Vector3 cPos;
	private Vector3 ePos;
	public bool idi;
	public Texture2D lo;
	public int markers = 1;
	public CKC cs;
	private GameObject lM;
	private bool tD = false;
	private Transform sM = null;
	public GameObject[] sMs;
	private static string sce = "";
	private Transform obj;
	private string objn;
	public static string erInit = "";
	static public Transform slO;
	public Transform selSurface;
	private RoadObjectScript rS;
	public bool flyby;
	private Vector3 pos;
	private float fl;
	private float oldfl;
	private bool gC;
	private bool sC;
	private bool gEC;
	private Transform markerobjects;
	private TextAnchor origAnchor;

	public void a(){
		l(transform);
	}
	
	public void d(MarkerScript markerScript){
		sM = markerScript.transform;	

		GameObject[] tmp = new GameObject[sMs.Length];
		for(int i=0;i<tmp.Length;i++){
			tmp[i] = sMs[i];
		}
		sMs = new GameObject[tmp.Length + 1];
		for(int i=0;i<tmp.Length;i++){
			sMs[i] = tmp[i];
		}
		sMs[sMs.Length - 1] = markerScript.transform.gameObject;
		
		cs.z2(sM, sMs, markerScript.ssn, markerScript.ssd, markerobjects, out markerScript.sMs, out markerScript.trperc, sMs);
		
		cS = -1;		
	}

	public void e(MarkerScript markerScript){
			if(markerScript.ssd != markerScript.oldSoftSelDistance || markerScript.ssd != markerScript.oldSoftSelDistance){
				cs.z2(sM, sMs, markerScript.ssn, markerScript.ssd, markerobjects, out markerScript.sMs, out markerScript.trperc, sMs);
				markerScript.oldSoftSelection = markerScript.ssn;
				markerScript.oldSoftSelDistance = markerScript.ssd;
			}		
			if(rS.autoUpdate) f(rS.geoResolution, false, false);
	}

	public void ResetMaterials(MarkerScript markerScript){
				if(cs != null)cs.z2(sM, sMs, markerScript.ssn, markerScript.ssd, markerobjects, out markerScript.sMs, out markerScript.trperc, sMs);
	}
	
	public void s(MarkerScript markerScript){
			if(markerScript.ssd != markerScript.oldSoftSelDistance){
				cs.z2(sM, sMs, markerScript.ssn, markerScript.ssd, markerobjects, out markerScript.sMs, out markerScript.trperc, sMs);	
				markerScript.oldSoftSelDistance = markerScript.ssd;
			}
			f(rS.geoResolution, false, false);
	}	
	
	private void h(string ctrl, MarkerScript markerScript){
		int i = 0;
		foreach(Transform tr in markerScript.sMs){
				MarkerScript wsScript = (MarkerScript) tr.GetComponent<MarkerScript>();
				if(ctrl == "rs") wsScript.LeftSurrounding(markerScript.rs - markerScript.oldLeftSurrounding, markerScript.trperc[i]);
				else if(ctrl == "ls") wsScript.RightSurrounding(markerScript.ls - markerScript.oldRightSurrounding, markerScript.trperc[i]);
				else if(ctrl == "ri") wsScript.LeftIndent(markerScript.ri - markerScript.oldLeftIndent, markerScript.trperc[i]);
				else if(ctrl == "li") wsScript.RightIndent(markerScript.li - markerScript.oldRightIndent, markerScript.trperc[i]);
				else if(ctrl == "rt") wsScript.LeftTilting(markerScript.rt - markerScript.oldLeftTilting, markerScript.trperc[i]);
				else if(ctrl == "lt") wsScript.RightTilting(markerScript.lt - markerScript.oldRightTilting, markerScript.trperc[i]);
				i++;
		}
	}

	public void r(){
		if(markers > 1) f(rS.geoResolution, false, false);
	}

	public void l(Transform tr){
		cGS = (GUISkin)Resources.Load("EasyRoads3D/ER3DSkin", typeof(GUISkin));

		lo = (Texture2D)Resources.Load("EasyRoads3D/ER3DLogo", typeof(Texture2D));
		
		obj = tr;
		cs = new CKC();
		rS = obj.GetComponent<RoadObjectScript>();
		
		foreach(Transform child in obj){
			if(child.name == "Markers") markerobjects = child;
		}
		
		CKC.terrainList.Clear();
		Terrain[] terrains = (Terrain[])FindObjectsOfType(typeof(Terrain));
		foreach(Terrain terrain in terrains) {
			Terrains t = new Terrains();
			t.terrain = terrain;
			if(!terrain.gameObject.GetComponent<EasyRoads3DTerrainID>()){
				EasyRoads3DTerrainID terrainscript = (EasyRoads3DTerrainID)terrain.gameObject.AddComponent("EasyRoads3DTerrainID");
				string id = UnityEngine.Random.Range(100000000,999999999).ToString();
				terrainscript.terrainid = id;
				t.id = id;
			}else{
				t.id = terrain.gameObject.GetComponent<EasyRoads3DTerrainID>().terrainid;
			}
			cs.q2(t);
		}

		cs.o1(obj, sce, rS.roadWidth, surfaceOpacity, out idi, out indent);
		cs.dd1 = dd1;
		cs.td1 = td1;		
		
		q();
	}
	
	public void z(){
		if(ms != null){
			for(int i = 0; i < ms.Length; i++){		
				ms[i] = false;
				oms[i] = false;
			}		
		}
	}

	
public void w(Vector3 pos){

		pos.y += rS.raiseMarkers;

		GameObject go = null;
		if(lM != null) go = (GameObject)Instantiate(lM);
		else go = (GameObject)Instantiate(Resources.Load("EasyRoads3D/marker", typeof(GameObject)));
		
		Transform newnode = go.transform;
		newnode.position = pos;
	
		markers++;
		string n;
		if(markers < 10) n = "Marker000" + markers.ToString();
		else if (markers < 100) n = "Marker00" + markers.ToString();
		else n = "Marker0" + markers.ToString();
	
		newnode.gameObject.name = n;
	
		MarkerScript scr = newnode.GetComponent<MarkerScript>();
		scr.idi = false;
		scr.objectScript = obj.GetComponent<RoadObjectScript>();
		if(lM == null){
			scr.ri = rS.indent;
			scr.li = rS.indent;
			scr.rs = rS.surrounding;
			scr.ls = rS.surrounding;
			scr.tension = 0.5f;
		}
		
		newnode.parent = markerobjects;

		lM = newnode.gameObject;
	
		if(markers > 1){
			f(rS.geoResolution, false, false);
			if(materialType == 0)cs.mrh1(materialType);
		}
	}

	public void f(float geo, bool renderMode, bool camMode){
		cs.ka.Clear();	
		
		int ii = 0;
		KQK k;

		foreach(Transform child  in obj) 
		{
			if(child.name == "Markers"){
				foreach(Transform marker   in child) {
					MarkerScript markerScript = marker.GetComponent<MarkerScript>();
					
					markerScript.objectScript = obj.GetComponent<RoadObjectScript>();
					if(!markerScript.idi) markerScript.idi = cs.i1(marker);

					k  = new KQK();
					k.position = marker.position;
					k.num = cs.ka.Count;
					k.object1 = marker;
					k.object2 = markerScript.surface;
					k.tension = markerScript.tension;
					k.ri = markerScript.ri;
					k.li =markerScript.li;
					k.rt = markerScript.rt;
					k.lt = markerScript.lt;
					k.rs = markerScript.rs;
					k.drs = markerScript.rs;
					k.ls = markerScript.ls;
					k.dls = markerScript.ls;
					k.renderFlag = markerScript.bridgeObject;
					k.dhf = markerScript.distHeights;
					k.tcS = cs;
					cs.ka.Add(k);
					ii++;
				}
			}
		}

		cs.qW = rS.roadWidth;
		cs.j(geo, obj, rS.closedTrack, renderMode, camMode);

	}
	
	public void StartCam(){
		f(0.5f, false, true);
	}

	public void q(){
		int i = 0;
		foreach(Transform child  in obj) 
		{
			if(child.name == "Markers"){
				i = 1;
				string n;
				foreach(Transform marker in child) {
					if(i < 10) n = "Marker000" + i.ToString();
					else if (i < 100) n = "Marker00" + i.ToString();
					else n = "Marker0" + i.ToString();

					marker.name = n;
					lM = marker.gameObject;

					i++;
				}

			}
		}
		markers = i - 1;
		f(rS.geoResolution, false, false);
	}	
	
	public void k(){
		RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
		ArrayList rObj = new ArrayList();
		foreach (RoadObjectScript script in scripts) {
			if(script.transform != transform) rObj.Add(script.transform);	
		}
		if(tL == null){
			tL = cs.k2();
			tLInt = cs.l2();
		}
		f(0.5f, true, false);
		cs.f1(Vector3.zero, rS.raise, obj, rS.closedTrack, rObj, hd1);
		d1();
	}
	
	public void v4(){
		f(rS.geoResolution, false, false);
		cs.v4();		
	}
	
	public void d1(){
			cs.d1(rS.applySplatmap, rS.splatmapSmoothLevel, rS.renderRoad, rS.tuw, rS.roadResolution, rS.raise, rS.opacity, rS.expand, rS.offsetX, rS.offsetY, rS.beveledRoad, rS.splatmapLayer);
	}
	
	public void u1(){	
		cs.u1(rS.renderRoad, rS.tuw, rS.roadResolution, rS.raise, rS.beveledRoad);
	}

	public void y(Vector3 pos, bool doInsert){
		int first = -1;
		int second = -1;
		float dist1 = 10000;
		float dist2 = 10000;
		Vector3 newpos = pos;
		KQK k;
		KQK k1 = (KQK)cs.ka[0];
		KQK k2 = (KQK)cs.ka[1];
		
		cs.p2(pos, out first, out second, out dist1, out dist2, out k1, out k2, out newpos);
		
		pos = newpos;
		
		if(doInsert && first >= 0 && second >= 0){

			if(rS.closedTrack && second == cs.ka.Count - 1){
				w(pos);
			}else{

				k = (KQK)cs.ka[second];
				string name = k.object1.name;
				string n;
				int j = second + 2;

				for(int i = second; i < cs.ka.Count - 1; i++){
					k = (KQK)cs.ka[i];
					if(j < 10) n = "Marker000" + j.ToString();
					else if (j < 100) n = "Marker00" + j.ToString();
					else n = "Marker0" + j.ToString();

					k.object1.name = n;
					j++;				
				}

				k = (KQK)cs.ka[first];
				Transform newnode = (Transform)Instantiate(k.object1.transform, pos, k.object1.rotation);
				newnode.gameObject.name = name;
				newnode.parent = markerobjects;
			
				MarkerScript scr = newnode.GetComponent<MarkerScript>();
				scr.idi = false;			
				float	totalDist = dist1 + dist2;
				float perc1 = dist1 / totalDist;			
				float paramDif = k1.ri - k2.ri;
				scr.ri = k1.ri - (paramDif * perc1);
				paramDif = k1.li - k2.li;
				scr.li = k1.li - (paramDif * perc1);
				paramDif = k1.rt - k2.rt;
				scr.rt = k1.rt - (paramDif * perc1);
				paramDif = k1.lt - k2.lt;
				scr.lt = k1.lt - (paramDif * perc1);
				paramDif = k1.rs - k2.rs;
				scr.rs = k1.rs - (paramDif * perc1);
				paramDif = k1.ls - k2.ls;
				scr.ls = k1.ls - (paramDif * perc1);
				f(rS.geoResolution, false, false);
				if(materialType == 0) cs.mrh1(materialType);
			}
		}
	}
	
	public void o(){
		DestroyImmediate(rS.sM.gameObject);
	}
	
	public void g(){
		if(cs == null) l(transform);
		CKC.bOs = true;
		if(!tD){
			geoResolution = 0.5f;
			q();
			k();
		}
		if(displayRoad){
			cs.road.transform.parent = null;
			cs.road.layer = 0;
			cs.road.name = gameObject.name;
		}
		else DestroyImmediate(cs.road);

	}

	public void b(){
		cs.m2(12);

	}
	
}

















