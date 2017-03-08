@CustomEditor(EasyRoads3DTerrainID)
class TerrainEditorScript extends Editor
{
	function OnSceneGUI()
	{
		if(Event.current.shift && RoadObjectScript.slO != null) Selection.activeGameObject = RoadObjectScript.slO.gameObject;
		else RoadObjectScript.slO = null;
	}
}
