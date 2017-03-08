using UnityEngine;
using System.Collections;

public class TouchItemCallback : MonoBehaviour {
	
	// Use this for initialization
	private UIGrid grid = null;
	
	void Start () {
		grid = GameObject.Find ("Grid").GetComponent<UIGrid>();
	}
	
	// 点击listView中的cell，修改cell的内容
	void OnClick()
	{
		Debug.Log (gameObject);
		if (gameObject) {
			UISprite sprite = gameObject.GetComponent<UISprite>();
			//sprite.color = Color.red;
		}
	}
}