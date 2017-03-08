using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

public class NewEasyRoads3D : EditorWindow
{
	private static NewEasyRoads3D instance;
	private Vector3 scroll;
	public GUISkin cGS;
	public GUISkin dGS;
	private string objectname = "RoadObject01";
	private bool closedTrack;
	
	
	public NewEasyRoads3D()
	{
		objectname = GetNewRoadName();
		if( instance != null ){
			DestroyImmediate(this);
			return;
		}
		
		instance = this;
		
		title = "New EasyRoads3D Object";
		position = new Rect((Screen.width - 300.0f) / 2.0f, (Screen.height - 150.0f) / 2.0f, 300.0f, 150.0f);
		minSize = new Vector2(300.0f, 150.0f);
		maxSize = new Vector2(300.0f, 150.0f);
	}
	
	public void OnDestroy(){
		instance = null;
	}
	
	public static NewEasyRoads3D Instance{
		get
		{
			if( instance == null ){
				new NewEasyRoads3D();
			}
			return instance;
		}
	}

	public void OnGUI()
	{
		if(cGS == null){
			dGS = GUI.skin;
			cGS = (GUISkin)Resources.Load("EasyRoads3D/ER3DSkin", typeof(GUISkin));
		}
		
		GUI.skin = cGS;
		GUI.Box(new Rect (5, 20, 282, 70),"", "box");
		GUI.skin = dGS;			
		GUILayout.BeginArea  (new Rect (5, 5, 286, 250));
		
		GUILayout.Label("Set a name for the new EasyRoads3D Road Object");
		
		EditorGUILayout.Space();
		
		GUILayout.BeginArea  (new Rect (50, 40, 200, 150));
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Object name",GUILayout.Width(75));
		objectname = GUILayout.TextField(objectname,GUILayout.Width(125));
		EditorGUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		EditorGUILayout.Space();		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button ("Create Object", GUILayout.Width(125))){
			if(objectname == ""){
				EditorUtility.DisplayDialog("Alert", "Please fill out a name for the new road object!", "Close");
			}else{
					bool flag = false;	
					string[] dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/EasyRoads3D");
					foreach(string nm in dirs){
						string[] words = nm.Split('\\');
						words = words[words.Length - 1].Split('/');
						string nm1 = words[words.Length - 1];
						if(nm1.ToUpper() == objectname.ToUpper()){
							EditorUtility.DisplayDialog("Alert", "An EasyRoads3D object with the name '"+objectname+"' already exists!\r\n\r\nPlease use an unique name!", "Close");
							flag = true;
							break;							
						}							
					}

					if(!flag){
						GameObject go = (GameObject)MonoBehaviour.Instantiate(Resources.Load("EasyRoads3D/EasyRoad3DObject", typeof(GameObject)));
						instance.Close();
						go.name = objectname;
						go.transform.position = Vector3.zero;
						RoadObjectScript script = go.GetComponent<RoadObjectScript>();
						script.closedTrack = false;
						script.autoUpdate = true;
						script.surrounding = 15.0f;
						script.indent = 3.0f;
						script.geoResolution = 5.0f;
						Selection.activeGameObject =  go;
					}
			}
		}
	
		
		EditorGUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	public string GetNewRoadName(){

		string path = Directory.GetCurrentDirectory() + "/EasyRoads3D";
					
		if( !Directory.Exists(path)){
			try{
				Directory.CreateDirectory( path);
			}
			catch(System.Exception e){
				Debug.Log("Could not create directory: " + path + " " + e);
				return "";
			}
		}

		string[] dirs = Directory.GetDirectories(@Directory.GetCurrentDirectory() + "/EasyRoads3D");

		int c = 0;
		int num;
		foreach(string nm in dirs){
			string[] words = nm.Split('\\');
			words = words[words.Length - 1].Split('/');
			string nm1 = words[words.Length - 1];
			if(nm.IndexOf("RoadObject") != -1){
				string str = nm1.Replace("RoadObject","");
				if(int.TryParse(str, out num)){
					if(num > c) c = num;
				}
			}					
		}
		c++;
				
		string n;
		if(c < 10) n = "RoadObject0" + c.ToString();
		else n = "RoadObject" + c.ToString();
	
		return n;
	}
}
