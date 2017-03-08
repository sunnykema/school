using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Threading;
using com.google.zxing.qrcode;
public class WebCameraScript : MonoBehaviour {
	
	public GameObject myCameraTexture;
	public UILabel label;
	private WebCamTexture webCameraTexture;


	private Texture2D qrCodeImage;
	private string _decodedString;

    public GameObject MyEasyTouch;  


	private Thread qrThread;
	
	private Color32[] c;
	private sbyte[] d;
	private int W, H, WxH;
	private int x, y, z;
	private int num=1;

	private bool isCheck=false;

    private bool isClick = false;
    
	void Start () {
		

		webCameraTexture = new WebCamTexture(WebCamTexture.devices[0].name, 640, 480,15);
		if(myCameraTexture.renderer)
			myCameraTexture.renderer.material.mainTexture = webCameraTexture;
		else
			myCameraTexture.GetComponent<UITexture>().mainTexture = webCameraTexture;
		// Starts the camera
		//webCameraTexture.Play();

		//OnEnable ();
	//	qrThread = new Thread(DecodeQR);
	//	qrThread.Start();
		//InvokeRepeating("DecodeQR",1,1.5f);
	}

    
    public void OnTwoDemButtonClick() {
        isClick = true;
        webCameraTexture = new WebCamTexture(WebCamTexture.devices[0].name, 640, 480, 15);
        if (myCameraTexture.renderer)
            myCameraTexture.renderer.material.mainTexture = webCameraTexture;
        else
            myCameraTexture.GetComponent<UITexture>().mainTexture = webCameraTexture;
        // Starts the camera
        //webCameraTexture.Play();

        OnEnable();
    }

	void OnGUI()
	{
        if (!isClick) {
            return;
        }

		GUI.Label (new Rect (0, 150, 100, 100), _decodedString);
		label.text = _decodedString;
	} 

	void Update()
	{       
        if (!isClick) {
            return;
        }
		c = webCameraTexture.GetPixels32();
		if(isCheck)
		try{
			d = new sbyte[WxH];
			z = 0;
			for(y = H - 1; y >= 0; y--) {
				for(x = 0; x < W; x++) {
					d[z++] = (sbyte)(((int)c[y * W + x].r) << 16 | ((int)c[y * W + x].g) << 8 | ((int)c[y * W + x].b));
				}
			}
			
			_decodedString=new QRCodeReader().decode(d, W, H).Text;
			print(_decodedString);
			Debug.Log("3443443"+_decodedString);
            label.text = _decodedString;
            ChangePosition changePosition = new ChangePosition();
            changePosition.ChangePositionByTwoDemOrNFC(_decodedString);
            GameObject twoDem = GameObject.Find("Window_Camera");
            twoDem.SetActive(false);
            //扫码成功后，把easyTouch显示出来
            MyEasyTouch.SetActive(true);
			OnDestroy();
		}catch
		{
			//Debug.Log("error");
		}
	}
	
	void OnEnable () {
		if(webCameraTexture != null) {
			webCameraTexture.Play();
			W = webCameraTexture.width;
			H = webCameraTexture.height;
			WxH = W * H;
			isCheck=true;
		}
	}


    public void DestroyCamera() {
       // isCheck = false;
      //  isClick = false;
        OnDestroy();

    }
	void OnDisable () {
		if(webCameraTexture != null) {
			webCameraTexture.Pause();
			isCheck=false;
            isClick = false;
		}
	}
	
	void OnDestroy () {
		//sqrThread.Abort();
		webCameraTexture.Stop();
		isCheck = false;
        isClick = false;
	}







}