import EasyRoads3D;
import EasyRoads3DEditor;

@CustomEditor(RoadObjectScript)
class RoadObjectEditorScript extends Editor
{
	var counter : int;
	var pe : float;

	function OnInspectorGUI(){
		if(target.cs == null){
			target.a(); 

			
		}
		EasyRoadsGUIMenu(true, true, target);
	}

	function OnSceneGUI() {
		if(target.cs == null){
			target.a(); 
			if(target.sce != EditorApplication.currentScene && target.cs == null){
				CKC.terrainList.Clear();
				target.sce = EditorApplication.currentScene;
			}
		}
		OnScene();
	}

function EasyRoadsGUIMenu(flag : boolean, senderIsMain : boolean,  nRoadScript : RoadObjectScript) : int {
	if(target.ms == null || target.oms == null || target.rS == null || target.ms.Length == 0 ){
		target.ms = new boolean[5];
		target.oms = new boolean[5];
		target.rS = nRoadScript;
		target.mS = target.cs.c2();
		target.tL = target.cs.k2();
		target.tLInt = target.cs.l2();
	}
	origAnchor = GUI.skin.box.alignment;
		
	if(target.cGS == null){
			target.cGS = Resources.Load("EasyRoads3D/ER3DSkin", GUISkin);
			target.lo = Resources.Load("EasyRoads3D/ER3DLogo", Texture2D);
	}
		
	if(!flag) target.z();

	if(target.dS == -1) target.sM = null;
		
	GUI.skin = target.cGS;
	EditorGUILayout.Space();

	EditorGUILayout.BeginHorizontal ();				
	GUILayout.FlexibleSpace();
	target.ms[0] = GUILayout.Toggle(target.ms[0] ,new GUIContent("", " Add road markers. "),"addmarkers",GUILayout.Width(40), GUILayout.Height(22)); 
	if(target.ms[0] == true && target.oms[0] == false) {
		target.z();
		target.ms[0] = true;  target.oms[0] = true;
		target.q();
	}
	target.ms[1]  = GUILayout.Toggle(target.ms[1] ,new GUIContent("", " Insert road markers. "),"insertMarkers",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.ms[1] == true && target.oms[1] == false) {
		target.z();
		target.ms[1] = true;  target.oms[1] = true;
		target.q();
	}
	target.ms[2]  = GUILayout.Toggle(target.ms[2] ,new GUIContent("", " Process the terrain and create road geometry. "),"terrain",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.ms[2] == true && target.oms[2] == false) {
		if(target.markers < 2){
			EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
			target.ms[2] = false; 
		}else{
			target.z();
			target.ms[2] = true;  target.oms[2] = true;
			target.tD = true;	
			CKC.bOs = false;
			target.k();
		}
	}
	target.ms[3]  = GUILayout.Toggle(target.ms[3] ,new GUIContent("", " General settings. "),"settings",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.ms[3] == true && target.oms[3] == false) {
		target.z();
		target.ms[3] = true;  target.oms[3] = true;
	}
	target.ms[4]  = GUILayout.Toggle(target.ms[4] ,new GUIContent("", "Version and Purchase Info"),"info",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.ms[4] == true && target.oms[4] == false) {
		target.z();
		target.ms[4] = true;  target.oms[4] = true;
	}
	GUILayout.FlexibleSpace();
	EditorGUILayout.EndHorizontal();
		
	GUI.skin = null;

	target.dS = -1;
		
	for(var i : int  = 0; i < 5; i++){ 
		if(target.ms[i]){
			target.dS = i;
			target.cS = i;
		}
	}
	if(target.dS == -1) target.z();
		
	var markerMenuDisplay : int = 1;
	if(target.dS == 0 || target.dS == 1) markerMenuDisplay = 0;	
	else if(target.dS == 2 || target.dS == 3 || target.dS == 4) markerMenuDisplay = 0;
				
	if(target.tD && !target.ms[2]){
		target.tD = false;
		target.v4();
	}
	
	GUI.skin.box.alignment = TextAnchor.UpperLeft;

	if(target.dS >= 0 && target.dS != 4){
		if(target.mS == null || target.mS.Length == 0){
			target.mS = target.cs.c2();
			target.tL = target.cs.k2();
			target.tLInt = target.cs.l2();
		}
		EditorGUILayout.BeginHorizontal();

		GUILayout.Box(target.mS[target.dS], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
	}
	
	if(target.dS == -1 && target.sM != null) Selection.activeGameObject =  target.sM.gameObject;
	
	GUI.skin.box.alignment = origAnchor;

	if(target.erInit == ""){
		target.erInit = EasyRoads3DEditor.ToolBar();
		target.cs.erInit = target.erInit;
	}
	
	if(target.erInit.Length == 0){

	}else if(target.dS == 0 || target.dS == 1){
	
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button ("Refresh Surfaces", GUILayout.Width(200))){
			target.q();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

	}else if(target.dS == 3){
		GUILayout.Label(" Settings");

		var oldDisplay : boolean = target.displayRoad;	
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Display object", "This will activate/deactivate the road object transforms"), GUILayout.Width(125) );
		target.displayRoad = EditorGUILayout.Toggle (target.displayRoad, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();

		if(oldDisplay != target.displayRoad){
			target.cs.j1(target.displayRoad, target.markerobjects);
		}
		
		if(target.materialStrings == null){target.materialStrings = new String[2]; target.materialStrings[0] = "Diffuse Shader"; target.materialStrings[1] = "Transparant Shader"; }
		if(target.materialStrings.Length == 0){target.materialStrings = new String[2]; target.materialStrings[0] = "Diffuse Shader"; target.materialStrings[1] = "Transparant Shader"; }
		var cm : int = target.materialType;
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Surface Material", "The material type used for the road surfaces."), GUILayout.Width(125) );
		target.materialType = EditorGUILayout.Popup (target.materialType, target.materialStrings,   GUILayout.Width(115));
		EditorGUILayout.EndHorizontal();
		if(cm != target.materialType) target.cs.mrh1(target.materialType);		
		
		if(target.materialType == 1){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("        Surface Opacity", "This controls the transparacy level of the surface objects."), GUILayout.Width(125) );
			var so : float = target.surfaceOpacity;
			target.surfaceOpacity = EditorGUILayout.Slider(target.surfaceOpacity, 0, 1,  GUILayout.Width(150));
			EditorGUILayout.EndHorizontal();
			if(so != target.surfaceOpacity) target.cs.mau1(target.surfaceOpacity);
		}
		
		EditorGUILayout.BeginHorizontal();
		var wd : float = target.roadWidth;
		GUILayout.Label(new GUIContent("    Road width", "The width of the road") ,  GUILayout.Width(125));
		target.roadWidth = EditorGUILayout.FloatField(target.roadWidth, GUILayout.Width(40) );
		EditorGUILayout.EndHorizontal();

		if(wd != target.roadWidth) target.f(target.geoResolution, false, false);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Road indent", "The distance from the left and right side of the road to the part of the terrain levelled at the same height as the road"),  GUILayout.Width(125));
		target.indent = EditorGUILayout.FloatField(target.indent, GUILayout.Width(40));
		EditorGUILayout.EndHorizontal();
	

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Raise markers", "This will raise the position of the markers (m)."), GUILayout.Width(125) );;
		target.raiseMarkers = EditorGUILayout.FloatField (target.raiseMarkers, GUILayout.Width(40));
		EditorGUILayout.EndHorizontal();	

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Surrounding", "This represents the distance over which the terrain will be gradually leveled to the original terrain height"),  GUILayout.Width(125));  
		target.surrounding = EditorGUILayout.FloatField(target.surrounding, GUILayout.Width(40));
		EditorGUILayout.EndHorizontal();

		var OldClosedTrack : boolean = target.closedTrack;
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Closed Track", "This will connect the 'start' and 'end' of the road"), GUILayout.Width(125) );;
		target.closedTrack = EditorGUILayout.Toggle (target.closedTrack, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();

		if(OldClosedTrack != target.closedTrack){
			target.q();
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Geometry Resolution", "The polycount of the generated surfaces. It is recommended to use a low resolution while creating the road. Use the maximum resolution when processing the final terrain."), GUILayout.Width(125) );
		var gr : float = target.geoResolution;
		target.geoResolution = EditorGUILayout.Slider(target.geoResolution, 0.5, 5,  GUILayout.Width(150));
		EditorGUILayout.EndHorizontal();

		if(gr != target.geoResolution) target.f(target.geoResolution, false, false);
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Update Vegetation", "When toggled on tree and detail objects near the road will be removed when rendering the terrain."), GUILayout.Width(125) );;
		target.hd1 = EditorGUILayout.Toggle (target.hd1, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();
		
		if(target.hd1){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Tree Distance (m)", "The distance from the left and the right of the road up to which terrain trees should be removed."), GUILayout.Width(125) );
			var tr : float = target.td1;
			target.td1 = EditorGUILayout.Slider(target.td1, 0, 25,  GUILayout.Width(150));
			EditorGUILayout.EndHorizontal();
			if(tr != target.td1) target.cs.td1 = target.td1;
		
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Detail Distance (m)", "The distance from the left and the right of the road up to which terrain detail opbjects should be removed."), GUILayout.Width(125) );
			tr = target.dd1;
			target.dd1 = EditorGUILayout.Slider(target.dd1, 0, 25,  GUILayout.Width(150));
			EditorGUILayout.EndHorizontal();
			if(tr != target.dd1) target.cs.dd1 = target.dd1;		
		}

		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Enable Debugging", "This will enable debugging."), GUILayout.Width(125) );;
		CKC.debugFlag = EditorGUILayout.Toggle (CKC.debugFlag, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

	}else if(target.dS == 2){
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(" Road Geometry:");
		EditorGUILayout.EndHorizontal();

		var oldRoad : boolean = target.renderRoad;
		var oldRoadResolution : float = target.roadResolution;
		var oldRoadUV : float = target.tuw;
		var oldRaise : float = target.raise;	
	
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("      Render"," When active, terrain matching road geometry will be created."), GUILayout.Width(125) );
		target.renderRoad = EditorGUILayout.Toggle (target.renderRoad, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();
	
		if(target.renderRoad){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Resolution"," The resolution level of the road geometry."), GUILayout.Width(125) );
			target.roadResolution = EditorGUILayout.IntSlider(target.roadResolution, 1, 10,  GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      UV Mapping"," Use the slider to control texture uv mapping of the road geometry."), GUILayout.Width(125) );
			target.tuw = EditorGUILayout.Slider(target.tuw, 1, 30,  GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();
	
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Raise (cm)","Optionally increase this setting when parts of the terrain stick through the road geometry. It is recommended to adjust these areas using the terrain tools!"), GUILayout.Width(125) );
			target.raise = EditorGUILayout.Slider(target.raise, 0, 100, GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button ("Save Geometry", GUILayout.Width(175))){
				target.b();
				Debug.Log("Road object geometry saved");
			}
			GUILayout.FlexibleSpace();	
			EditorGUILayout.EndHorizontal();
		
		}
	
		EditorGUILayout.Space();
	
		if(oldRoad != target.renderRoad || oldRoadResolution != target.roadResolution || oldRoadUV != target.tuw || oldRaise != target.raise) target.u1();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("  Terrain Splatmap Handling:");
		EditorGUILayout.EndHorizontal();

		var oldApplySplatmap : boolean = target.applySplatmap;	
	
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("      Apply Splatmap"," When active, the road will be added to the terrain splatmap. The quality highly depends on the terrain Control Texture Resolution size."), GUILayout.Width(125) );
		target.applySplatmap = EditorGUILayout.Toggle (target.applySplatmap, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();
	
		if(target.applySplatmap){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Terrain texture", "This represents the terrain texture which will be used for the road spatmap."), GUILayout.Width(125) );
			target.splatmapLayer = EditorGUILayout.IntPopup (target.splatmapLayer, target.tL, target.tLInt,   GUILayout.Width(115));
			EditorGUILayout.EndHorizontal();		
		
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Expand"," Use this setting to increase the size of the splatmap."), GUILayout.Width(125) );
			target.expand = EditorGUILayout.IntSlider(target.expand,0, 3, GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Smooth Level"," Use this setting to blur the road splatmap for smoother results."), GUILayout.Width(125) );
			target.splatmapSmoothLevel = EditorGUILayout.IntSlider (target.splatmapSmoothLevel, 0, 3,  GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();
	
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Offset x"," Moves the road splatmap in the x direction."), GUILayout.Width(125) );
			target.offsetX = EditorGUILayout.IntField (target.offsetX, GUILayout.Width(50));
			EditorGUILayout.EndHorizontal();
	
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Offset y"," Moves the road splatmap in the y direction."), GUILayout.Width(125) );
			target.offsetY= EditorGUILayout.IntField (target.offsetY, GUILayout.Width(50));	
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("      Opacity","Use this setting to blend the road splatmap with the terrain splatmap."), GUILayout.Width(125) );
			target.opacity = EditorGUILayout.Slider (target.opacity, 0, 1,  GUILayout.Width(175));
			EditorGUILayout.EndHorizontal();
			
			GUI.enabled = target.gE;
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button ("Apply Changes", GUILayout.Width(175))){
				target.d1();
				target.gE = false;
			}
			GUILayout.FlexibleSpace();	
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space();
			
	
		if(oldApplySplatmap != target.applySplatmap) target.d1();
		
		GUI.enabled = true;

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("  Smooth Edges:", "This will smoothen the terrain near the surface edges according the below distance."), GUILayout.Width(175) );
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("      Distance (m)","This represents the smoothen distance."), GUILayout.Width(125) );
		target.smoothDistance = EditorGUILayout.Slider (target.smoothDistance, 0, 5,  GUILayout.Width(175));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button ("Smooth Edges", GUILayout.Width(175))){
			target.cs.ts1(target.smoothDistance, 0);
		}
		GUILayout.FlexibleSpace();	
		EditorGUILayout.EndHorizontal();	

		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent(" Smooth Surrounding:", "This will smoothen the terrain near the surrounding edges according the below distance."), GUILayout.Width(175) );
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("      Distance (m)","This represents the smoothen distance."), GUILayout.Width(125) );
		target.smoothSurDistance = EditorGUILayout.Slider (target.smoothSurDistance, 0, 15,  GUILayout.Width(175));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button ("  Smooth Surrounding", GUILayout.Width(175))){
			target.cs.ts1(target.smoothSurDistance, 1);
		}
		GUILayout.FlexibleSpace();	
		EditorGUILayout.EndHorizontal();	

		EditorGUILayout.Space();		
		EditorGUILayout.Space();
	}else if(target.dS == 4){
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		GUILayout.Label(target.lo);
		GUILayout.FlexibleSpace();	
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();	

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(" EasyRoads3D - FullVersion 1.8.1");		
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();	
		GUILayout.Label(" Version Type: Full Version", GUILayout.Height(22));
		if(GUILayout.Button ("i", GUILayout.Width(22))){
			Application.OpenURL ("http://www.unityterraintools.com");
		}
		GUILayout.FlexibleSpace();	
		EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.Space();	

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button ("Manual", GUILayout.Width(150))){
			Application.OpenURL ("http://www.unityterraintools.com/manual.php");
		}		
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}else{
		if(target.markers != target.markerobjects.childCount){
			target.q();
		}	
	}


	EditorGUILayout.Space();

	if (GUI.tooltip != "") GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 200, 40), GUI.tooltip); 
	
	if (GUI.changed)
	{
		target.gE = true;
	}		
	
		return markerMenuDisplay;	
		
	}

	function OnScene(){
		var cEvent : Event = Event.current;  
		
		if(target.cS != -1  && Event.current.shift) target.ms[target.cS] = true;

		if(target.ms == null || target.ms.Length == 0){
			target.ms = new boolean[5];
			target.oms = new boolean[5];
		}
		
		if((cEvent.shift  && cEvent.type == EventType.mouseDown) || target.ms[1])
			{ 
				var hit : RaycastHit;
				var mPos : Vector2 = cEvent.mousePosition;
				mPos.y = Screen.height - mPos.y - 35;
				var ray : Ray = Camera.current.ScreenPointToRay(mPos); 
				if (Physics.Raycast (Camera.current.transform.position, ray.direction, hit, 3000)) 
				{ 
							
					if(target.ms[0]){
						target.w(hit.point);
					}	
					else if(target.ms[1] && cEvent.type == EventType.mouseDown && cEvent.shift){
						target.y(hit.point, true); 
					}
					else if(target.ms[1]  && cEvent.shift) target.y(hit.point, false); 
					else if(target.handleInsertFlag) target.handleInsertFlag = target.cs.a2();
					
					Selection.activeGameObject = target.obj.gameObject;
				}
			}
			
			if(target.slO != target.obj || target.obj.name != target.objn){
				target.cs.r1();
				target.slO = target.obj;
				target.objn = target.obj.name;
				target.tL = target.cs.k2();
				target.tLInt = target.cs.l2();
			}
			
			if(target.sM != null){
				target.cs.a2();
			}
			
			if(target.transform.position != Vector3.zero) target.transform.position = Vector3.zero;
	}

}




