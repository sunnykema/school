using UnityEngine;
using System.Collections;

public class DragDropItem : UIDragDropItem {
	
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>
	
	public GameObject prefab;
	
	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>
	
	protected override void OnDragDropRelease (GameObject surface)
	{
		if (surface != null)
		{
			ExampleDragDropSurface dds = surface.GetComponent<ExampleDragDropSurface>();
			
			if (dds != null)
			{
				GameObject child = NGUITools.AddChild(dds.gameObject, prefab);
				child.transform.localScale = dds.transform.localScale;
				
				Transform trans = child.transform;
				trans.position = UICamera.lastWorldPosition;
				
				if (dds.rotatePlacedObject)
				{
					trans.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
				}
				
				// Destroy this icon as it's no longer needed
				NGUITools.Destroy(gameObject);
				return;
			}
			
		}
		
		//give it to target
		base.OnDragDropRelease(surface);
		/*
		if (surface != null && surface.name == "Container_Choose") {
			print ("yes");
			GameObject mySurface = GameObject.Find ("MyContainer");
			base.OnDragDropRelease (mySurface);

			UIGrid gridChoose = GameObject.Find ("GridChoose").GetComponent<UIGrid> ();
			UITable grid = GameObject.Find ("Grid").GetComponent<UITable> ();
			print ("girdChoose.child = " + gridChoose.transform.childCount);
			print ("grid.transform.childCount for=" + grid.transform.childCount);
			grid.Reposition ();
			grid.repositionNow = true;
			print ("grid.transform.childCount =" + grid.transform.childCount);
			gridChoose.Reposition ();
			gridChoose.repositionNow = true;
		} else if (surface != null && surface.name == "Container_Delete") {
			print ("on the Container_Delete");
			GameObject delete = GameObject.Find ("Container_Delete");
			while (delete.transform.childCount > 0)
			{
				DestroyImmediate(delete.transform.GetChild(0).gameObject);
			}
			//delete.Reposition ();
			//delete.repositionNow = true;
		}
		/*GameObject mySurface = GameObject.Find ("MyContainer");
		base.OnDragDropRelease (mySurface);
		
		UIGrid gridChoose =  GameObject.Find ("GridChoose").GetComponent<UIGrid>();
		UIGrid grid = GameObject.Find ("Grid").GetComponent<UIGrid> ();
		print ("girdChoose.child = "+ gridChoose.transform.childCount);
		grid.Reposition ();
		grid.repositionNow = true;
		gridChoose.Reposition ();
		gridChoose.repositionNow = true;
		*/
	}

}
