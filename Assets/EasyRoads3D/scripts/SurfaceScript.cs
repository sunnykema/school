using UnityEngine;
using System.Collections;

public class SurfaceScript : MonoBehaviour {
	void Start () {
		Material mat;
		if(transform.parent.GetComponent<MarkerScript>().objectScript.materialType == 0) mat = (Material)MonoBehaviour.Instantiate(Resources.Load("EasyRoads3D/surfaceMaterial", typeof(Material)));
		else mat = (Material)MonoBehaviour.Instantiate(Resources.Load("EasyRoads3D/surfaceAlphaMaterial", typeof(Material)));
		Color c = mat.color;
		c.a = transform.parent.GetComponent<MarkerScript>().objectScript.surfaceOpacity;
		gameObject.renderer.sharedMaterial = mat;
	}	

}
