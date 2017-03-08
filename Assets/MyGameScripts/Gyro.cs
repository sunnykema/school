using UnityEngine;
using System.Collections;

public class Gyro : MonoBehaviour
{
    private bool gyroBool;
    private int a = 0;
    private Gyroscope gyro;
    private Quaternion rotFix;
    private Quaternion camRot;

    private GameObject camParent;
    public ScreenOrientation currentOrientation;

    private bool initialized = false;
    Quaternion ro;
    void Start()
    {
        ro.x = 0;
        ro.y = 180;
        ro.z = 0;
        transform.rotation = ro;
    }
    private void init()
    {
        if (!initialized)
        {
            Transform originalParent = transform.parent; // check if this transform has a parent
            camParent = new GameObject("camParent"); // make a new parent
            camParent.transform.position = transform.position; // move the new parent to this transform position
            transform.parent = camParent.transform; // make this transform a child of the new parent
            camParent.transform.parent = originalParent; // make the new parent a child of the original parent

            gyroBool = SystemInfo.supportsGyroscope;

            if (gyroBool)
            {

                gyro = Input.gyro;
                gyro.enabled = true;

            }
            else
            {
                print("NO GYRO");
                GUI.Label(new Rect(200, 100, 100, 40), "未能开启陀螺仪！！！");
            }

            initialized = true;
        }
    }

    private void updateScreenOrientation()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            camParent.transform.eulerAngles = new Vector3(90, 180, 0);
            rotFix = new Quaternion(0f, 0f, 1f, 0.14558f);
            //rotFix = new Quaternion(0f,0f,0.7071f,0.7071f);
        }
        else if (Screen.orientation == ScreenOrientation.Portrait)
        {
            camParent.transform.eulerAngles = new Vector3(90, 90, -90);
            rotFix = new Quaternion(0f, 0f, 1f, 0f);
        }
    }

    void Update()
    {
        if (a == 1)
        {
            init();
            if (currentOrientation != Screen.orientation)
            {
                currentOrientation = Screen.orientation;
                updateScreenOrientation();
            }

            if (gyroBool)
            {
                camRot = gyro.attitude * rotFix;
                transform.localRotation = camRot;
            }
        }

    }
    /// <summary>
    /// 打开或者关闭陀螺仪
    /// </summary>
    public void ChangeGyroState() {
        if (a == 0)
        {
            a = 1;
        }
        else if (a == 1)
        {
            a = 0;
        }
    
    }
    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(200, 50, 100, 40), "模式转换"))
        {
            if (a == 0)
            {
                a = 1;
            }                                                                                                   
            else if (a == 1)
            {
                a = 0;
            }
        }
    }
     * */
}