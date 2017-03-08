@CustomEditor(MarkerScript)
class MarkerEditorScript extends Editor
{
		var oldPos : Vector3;
		var pos : Vector3;
		var cGS : GUISkin;
		var dGS : GUISkin;
		var showGui : int;
		var sC : boolean;
		
	function OnInspectorGUI()
	{
		if(target.objectScript == null) target.SetObjectScript();
		showGui = EasyRoadsGUIMenu(false, false, target.objectScript);
		if(showGui != -1) Selection.activeGameObject =  target.transform.parent.parent.gameObject;
		else if(target.objectScript.sMs.length <= 1) ERMarkerGUI(target);
		else  if(target.objectScript.sMs.length == 2) MSMarkerGUI(target);
	}
				
	function OnSceneGUI() {
		if(target.objectScript == null) target.SetObjectScript();
		if(target.objectScript.cs == null || target.objectScript.erInit == "") Selection.activeGameObject =  target.transform.parent.parent.gameObject;//target.objectScript.a();
		else MarkerOnScene(target);		
	}
	
	
function EasyRoadsGUIMenu(flag : boolean, senderIsMain : boolean,  nRoadScript : RoadObjectScript) : int {
	if((target.objectScript.ms == null || target.objectScript.oms == null || target.objectScript.rS == null) && target.objectScript.cs){
		target.objectScript.ms = new boolean[5];
		target.objectScript.oms = new boolean[5];
		target.objectScript.rS = nRoadScript;
		target.objectScript.mS = target.objectScript.cs.c2();
		target.objectScript.tL = target.objectScript.cs.k2();
		target.objectScript.tLInt = target.objectScript.cs.l2();
	}else if(target.objectScript.cs == null) return;

	origAnchor = GUI.skin.box.alignment;
		
	if(target.objectScript.cGS == null){
		target.objectScript.cGS = Resources.Load("EasyRoads3D/ER3DSkin", GUISkin);
		target.objectScript.lo = Resources.Load("EasyRoads3D/ER3DLogo", Texture2D);
}
		
	if(!flag) target.objectScript.z();
		
	GUI.skin = target.objectScript.cGS;
	EditorGUILayout.Space();

	EditorGUILayout.BeginHorizontal ();				
	GUILayout.FlexibleSpace();
	target.objectScript.ms[0] = GUILayout.Toggle(target.objectScript.ms[0] ,new GUIContent("", " Add road markers. "),"addmarkers",GUILayout.Width(40), GUILayout.Height(22)); 
	if(target.objectScript.ms[0] == true && target.objectScript.oms[0] == false) {
		target.objectScript.z();
		target.objectScript.ms[0] = true;  target.objectScript.oms[0] = true;
		target.objectScript.q();
	}
	target.objectScript.ms[1]  = GUILayout.Toggle(target.objectScript.ms[1] ,new GUIContent("", " Insert road markers. "),"insertMarkers",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.objectScript.ms[1] == true && target.objectScript.oms[1] == false) {
		target.objectScript.z();
		target.objectScript.ms[1] = true;  target.objectScript.oms[1] = true;
		target.objectScript.q();
	}
	target.objectScript.ms[2]  = GUILayout.Toggle(target.objectScript.ms[2] ,new GUIContent("", " Process the terrain and create road geometry. "),"terrain",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.objectScript.ms[2] == true && target.objectScript.oms[2] == false) {
		if(target.objectScript.markers < 2){
			EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
			target.objectScript.ms[2] = false; 
		}else{
			target.objectScript.z();
			target.objectScript.ms[2] = true;  target.objectScript.oms[2] = true;
	
			target.objectScript.tD = true;	
			target.objectScript.k();
		}
	}
	target.objectScript.ms[3]  = GUILayout.Toggle(target.objectScript.ms[3] ,new GUIContent("", " General settings. "),"settings",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.objectScript.ms[3] == true && target.objectScript.oms[3] == false) {
		target.objectScript.z();
		target.objectScript.ms[3] = true;  target.objectScript.oms[3] = true;
	}
	target.objectScript.ms[4]  = GUILayout.Toggle(target.objectScript.ms[4] ,new GUIContent("", "Version and Purchase Info"),"info",GUILayout.Width(40),GUILayout.Height(22)); 
	if(target.objectScript.ms[4] == true && target.objectScript.oms[4] == false) {
		target.objectScript.z();
		target.objectScript.ms[4] = true;  target.objectScript.oms[4] = true;
	}
	GUILayout.FlexibleSpace();
	EditorGUILayout.EndHorizontal();
		
	GUI.skin = null;

	target.objectScript.dS = -1;
		
	for(var i : int  = 0; i < 5; i++){ 
		if(target.objectScript.ms[i]){
			target.objectScript.dS = i;
			target.objectScript.cS = i;
		}
	}
	if(target.objectScript.dS == -1) target.objectScript.z();
		
	var markerMenuDisplay : int = 1;
	if(target.objectScript.dS == 0 || target.objectScript.dS == 1) markerMenuDisplay = 0;	
	else if(target.objectScript.dS == 2 || target.objectScript.dS == 3 || target.objectScript.dS == 4) markerMenuDisplay = 0;
				
	if(target.objectScript.tD && !target.objectScript.ms[2]){
		target.objectScript.tD = false;
		target.objectScript.v4();
	}
	
	GUI.skin.box.alignment = TextAnchor.UpperLeft;

	if(target.objectScript.dS >= 0 && target.objectScript.dS != 4){
		if(target.objectScript.mS == null || target.objectScript.mS.Length == 0){
			target.objectScript.mS = target.objectScript.cs.c2();
			target.objectScript.tL = target.objectScript.cs.k2();
			target.objectScript.tLInt = target.objectScript.cs.l2();
		}
		EditorGUILayout.BeginHorizontal();

		GUILayout.Box(target.objectScript.mS[target.objectScript.dS], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
	}
	
		return target.objectScript.dS;
	}
	
	
	function ERMarkerGUI( markerScript : MarkerScript){
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(" Road Marker Settings");
		EditorGUILayout.EndHorizontal();
		
		var oldss : boolean = markerScript.ssn;
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Soft Selection", "When selected, the settings of other road markers within the selected distance will change according their distance to this marker."),  GUILayout.Width(105));  
		GUI.SetNextControlName ("ssn");
		markerScript.ssn = EditorGUILayout.Toggle (markerScript.ssn, GUILayout.Width(25));
		EditorGUILayout.EndHorizontal();

		if(markerScript.ssn){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("         Distance", "The soft selection distance within other markers should change too."),  GUILayout.Width(105));  
			markerScript.ssd = EditorGUILayout.Slider(markerScript.ssd, 0, 500);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
		
		if(oldss != markerScript.ssd) target.objectScript.ResetMaterials(markerScript);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Left Indent", "The distance from the left side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
		GUI.SetNextControlName ("ri");
		oldfl = markerScript.ri;
		markerScript.ri = EditorGUILayout.Slider(markerScript.ri, target.objectScript.indent, 100);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.ri){
			target.objectScript.h("ri", markerScript);
			markerScript.oldLeftIndent = markerScript.ri;
		}

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Right Indent", "The distance from the right side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
		oldfl = markerScript.li;
		markerScript.li =  EditorGUILayout.Slider(markerScript.li, target.objectScript.indent, 100);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.li){
			target.objectScript.h("li", markerScript);
			markerScript.oldRightIndent = markerScript.li;
		}

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Left Surrounding", "This represents the distance over which the terrain will be gradually leveled on the left side of the road to the original terrain height"),  GUILayout.Width(105));  
		oldfl = markerScript.rs;
		GUI.SetNextControlName ("rs");
		markerScript.rs = EditorGUILayout.Slider(markerScript.rs,  target.objectScript.indent, 100);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.rs){
			target.objectScript.h("rs", markerScript);
			markerScript.oldLeftSurrounding = markerScript.rs;
		}
	
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Right Surrounding", "This represents the distance over which the terrain will be gradually leveled on the right side of the road to the original terrain height"),  GUILayout.Width(105));  
		oldfl = markerScript.ls;
		markerScript.ls = EditorGUILayout.Slider(markerScript.ls,  target.objectScript.indent, 100);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.ls){
			target.objectScript.h("ls", markerScript);
			markerScript.oldRightSurrounding = markerScript.ls;
		}

		EditorGUILayout.BeginHorizontal();
		oldfl = markerScript.rt;
		GUILayout.Label(new GUIContent("    Left Tilting", "Use this setting to tilt the road on the left side (m)."),  GUILayout.Width(105));  
		markerScript.rt = EditorGUILayout.Slider(markerScript.rt, 0, target.objectScript.roadWidth);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.rt){
			target.objectScript.h("rt", markerScript);
			markerScript.oldLeftTilting = markerScript.rt;
		}

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Right Tilting", "Use this setting to tilt the road on the right side (cm)."),  GUILayout.Width(105));  
		oldfl = markerScript.lt;
		markerScript.lt = EditorGUILayout.Slider(markerScript.lt, 0, target.objectScript.roadWidth);
		EditorGUILayout.EndHorizontal();
		
		if(oldfl != markerScript.lt){
			target.objectScript.h("lt", markerScript);
			markerScript.oldRightTilting = markerScript.lt;
		}

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("    Bridge Object", "When selected this road segment will be treated as a bridge segment."),  GUILayout.Width(105));  
		GUI.SetNextControlName ("bridgeObject");
		markerScript.bridgeObject = EditorGUILayout.Toggle (markerScript.bridgeObject, GUILayout.Width(10));

		if(markerScript.bridgeObject){
			GUILayout.Label(new GUIContent("    Distribute Heights", "When selected the terrain, the terrain will be gradually leveled between the height of this road segment and the current terrain height of the inner bridge segment."),  GUILayout.Width(105));  
			GUI.SetNextControlName ("distHeights");
			markerScript.distHeights = EditorGUILayout.Toggle (markerScript.distHeights);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		if (GUI.changed && !target.objectScript.gC){
			target.objectScript.gC = true;
			EditorUtility.SetDirty(target);
		}else if(target.objectScript.gC){		
			target.objectScript.e(markerScript);
			target.objectScript.gC = false;
		}

		oldfl = markerScript.rs;
				
	}

	function MSMarkerGUI( markerScript : MarkerScript){
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button (new GUIContent(" Align XYZ", "Click to distribute the x, y and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
			target.objectScript.cs.am1(target.objectScript.sMs, 0);
			target.objectScript.r();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button (new GUIContent(" Align XZ", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
			target.objectScript.cs.am1(target.objectScript.sMs, 1);
			target.objectScript.r();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button (new GUIContent(" Align XZ  Snap Y", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers and snap the y value to the terrain height at the new position."), GUILayout.Width(150))){
			target.objectScript.cs.am1(target.objectScript.sMs, 2);
			target.objectScript.r();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}

	function MarkerOnScene(markerScript : MarkerScript){
		var cEvent : Event = Event.current;

		if(Event.current.clickCount == 2 ) Selection.activeGameObject = target.transform.parent.parent.gameObject;
		else if(cEvent.shift && (target.objectScript.cS == 0 || target.objectScript.cS == 1)) {
		}else if(cEvent.shift && target.objectScript.sM != target.transform){
			target.objectScript.d(markerScript);
			Selection.objects = target.objectScript.sMs;
		}else if(target.objectScript.sM != target.transform){
			target.objectScript.sMs = new GameObject[0];
			target.objectScript.d(markerScript);
			Selection.objects = target.objectScript.sMs;
		}else{
			pos = markerScript.oldPos;
			if(pos  != oldPos && !markerScript.changed){
				oldPos = pos;
				if(!cEvent.shift) target.objectScript.s(markerScript);			
			}
		}

		if(cEvent.shift && markerScript.changed){
			sC = true;
		}

		markerScript.changed = false;
			
		if(!cEvent.shift && sC){
			target.objectScript.s(markerScript);
			sC = false;
		}
	}

}

