using UnityEngine;
using System.Collections;
/*
public enum GameGrade{
	EASY,
	NORMAL,
	DIFFICULTY
}

public enum ControlType{
	KEYBOARD,
	TOUCH,
	MOUSE
}

*/
public class MoveSetting : MonoBehaviour {
	public TweenPosition startPanelTween;
	public TweenPosition optionPanelTween;
	/*
	public float volume = 1;
	public GameGrade gameGrade = GameGrade.NORMAL;
	public ControlType controlType = ControlType.KEYBOARD;
	public bool isFullScreen = false;



	public void OnVolumeChanged(){
		print ("Volume = " + UIProgressBar.current.value);
		volume = UIProgressBar.current.value;
	}

	public void OnGameGradeChanged(){
		print ("GameGrade = "+UIPopupList.current.value);
		//gameGrade = UIPopupList.current.value;
		switch (UIPopupList.current.value.Trim()) {
		case "简单" :
				gameGrade = GameGrade.EASY;
				break;
		case "常规": 
				gameGrade = GameGrade.NORMAL;
				break;
		case "困难":
			gameGrade = GameGrade.DIFFICULTY;
				break;
		}
	}

	public void OnControlTypeChanged(){
		print ("ControlType = "+UIPopupList.current.value);
	//	controlType = UIPopupList.current.value;
		switch (UIPopupList.current.value.Trim()) {
		case "鼠标":
			controlType = ControlType.MOUSE;
			break;
		case "键盘":
			controlType = ControlType.KEYBOARD;
			break;
		case "触屏":
			controlType = ControlType.TOUCH;
			break;
		}
	}

	public void OnIsFullScreenChanged(){
		print ("IsFullScreen = "+UIToggle.current.value);
		isFullScreen = UIToggle.current.value;

	}
*/
	public void OnOptionButtonClick(){
		print ("click the optionbutton");
		startPanelTween.PlayForward ();
		optionPanelTween.PlayForward ();
	}


	public void OnCompelishButtonclick(){
		print ("click the oncomplishbutton");
		startPanelTween.PlayReverse ();
		optionPanelTween.PlayReverse ();
	}


}
