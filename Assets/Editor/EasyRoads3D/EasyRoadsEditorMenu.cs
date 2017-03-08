using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using EasyRoads3D;

public class EasyRoadsEditorMenu : ScriptableObject {

	[MenuItem( "EasyRoads3D/New EasyRoads3D Object" )]
	public static void  CreateEasyRoads3DObject () 
	{
			NewEasyRoads3D window = (NewEasyRoads3D)ScriptableObject.CreateInstance(typeof(NewEasyRoads3D));
			window.ShowUtility();
	}

	[MenuItem( "EasyRoads3D/Back Up Terrain Height Data" )]
	public static void  GetTerrain () 
	{
		if(GetEasyRoads3DObjects()){
			TH.GT(Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}
	}


	[MenuItem( "EasyRoads3D/Restore Terrain Height Data" )]
	public static void  SetTerrain () 
	{
		if(GetEasyRoads3DObjects()){
			TH.ST(Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}		
	}

	[MenuItem( "EasyRoads3D/Back Up Terrain Splatmaps" )]
	public static void  GS() 
	{
		if(GetEasyRoads3DObjects()){
			TH.GS(Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}				
	}

	[MenuItem( "EasyRoads3D/Restore Terrain Splatmaps" )]
	public static void  SS () 
	{
		if(GetEasyRoads3DObjects()){
			string path = "";
			if(EditorUtility.DisplayDialog("Road Splatmap", "Would you like to merge the terrain splatmap(s) with a road splatmap?", "Yes", "No")){ 
				path = EditorUtility.OpenFilePanel("Select png road splatmap texture", "", "png");
			}
			// optionally you can merge the terrain splatmaps with the road splatmap, the 2nd parameter refers to the road splatmap opacity level 0 - 100
			// the 3rd parameter refers to the terrain texture used to paint the road shape
			TH.STS(true, 100, 4, path, Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}				
	}

	[MenuItem( "EasyRoads3D/Back Up Terrain Vegetation" )]
	public static void  GVN() 
	{
		if(GetEasyRoads3DObjects()){
			TH.GVN(Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}				
	}	

	[MenuItem( "EasyRoads3D/Restore Terrain Vegetation" )]
	public static void  SVN() 
	{
		if(GetEasyRoads3DObjects()){
			TH.SVN(Selection.activeGameObject);
		}else{
			EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
		}				
	}	
	
	[MenuItem( "EasyRoads3D/Build EasyRoads3D Objects" )]
	public static void  FinalizeRoads () 
	{
		if(EditorUtility.DisplayDialog("Build EasyRoads3D Objects", "This process includes destroying all EasyRoads3D control objects. Do you want to continue?", "Yes", "No")){ 
			RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
			foreach (RoadObjectScript script in scripts) {
				if(script.transform != null){
					script.g();
					foreach(Transform child in script.transform){
						if(script.transform != child){
							DestroyImmediate(child.gameObject);
						}
					}
					DestroyImmediate(script.gameObject);						
				}				
			}
		
			EasyRoads3DTerrainID[] terrainscripts = (EasyRoads3DTerrainID[])FindObjectsOfType(typeof(EasyRoads3DTerrainID));
			foreach (EasyRoads3DTerrainID script in terrainscripts) {
				DestroyImmediate(script);
			}			
		}
	}
	
	public static bool GetEasyRoads3DObjects(){
		RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
		bool flag = false;
		foreach (RoadObjectScript script in scripts) {
			if(script.cs == null) script.l(script.transform);
			flag = true;		
		}
		return flag;		
	}

	[MenuItem( "EasyRoads3D/Position Road" )]
	public static void   PositionRoad () 
	{
		if(Selection.activeTransform == null)
		{	
			EditorUtility.DisplayDialog("Select Road Object", "Select the road object first!", "Ok"); 
			return;    	
		}
		string path = EditorUtility.OpenFilePanel("Select EasyRoads3D export file", "", "txt");
	
		if(path != null)
		{
			Selection.activeTransform.position = ReadFile(path); 
		}
	}	

	public static Vector3 ReadFile(string file)
	{
		StreamReader streamReader = File.OpenText(file);
		string line = streamReader.ReadLine();
		line = line.Replace(",",".");
		string[] lines = line.Split("\n"[0]);
		string[] arr = lines[0].Split("|"[0]);
		Vector3 pos = Vector3.zero;
		float.TryParse(arr[0],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.x);
		float.TryParse(arr[1],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.y);
		float.TryParse(arr[2],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.z);
		return pos;
	}   
	
}
