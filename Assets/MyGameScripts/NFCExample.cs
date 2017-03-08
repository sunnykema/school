using UnityEngine;
using System.Collections;

public class NFCExample : MonoBehaviour
{
//	public GUIText nfc_output_text;
	AndroidJavaClass pluginTutorialActivityJavaClass;
    public UILabel LabelError;
	void Start ()
	{

        try
        {
            AndroidJNI.AttachCurrentThread();
            pluginTutorialActivityJavaClass = new AndroidJavaClass("com.twinsprite.nfcplugin.NFCPluginTest");

        }
        catch { }
    
	}

	void Update ()
	{
        try{
            string value = pluginTutorialActivityJavaClass.CallStatic<string>("getValue");
            ChangePosition changePosition = new ChangePosition();
            changePosition.ChangePositionByTwoDemOrNFC(value);
            LabelError.text = "Value:\n" + value;
        //    nfc_output_text.text = "Value:\n" + value;
        //    LabelError.text = value;
   
        }
        catch{
        }
	}
}