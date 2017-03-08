using UnityEngine;
using System.Collections;

public class Hand_camera : MonoBehaviour {

	// Use this for initialization
    //var target : Transform;
    //public Transform target;    
    //缩放系数
    public float distance = 10.0f;
    //左右滑动移动速度
    private float xSpeed = 250.0f;
    private float ySpeed = 120.0f;
    //缩放限制系数
    private float yMinLimit = -20;
    private float yMaxLimit = 80;
    //摄像头的位置
    private float x = 0.0f;
    private float y = 0.0f;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    Vector2 oldPosition1;
    Vector2 oldPosition2;
    Vector2 tempPosition1;
    Vector2 tempPosition2;
	void Start () {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        //if()
	}
	
	// Update is called once per frame
	void Update () {
	    //判断触摸数量为单点触摸
	/*if(Input.touchCount == 1)
	{
		//触摸类型为移动触摸
		if(Input.GetTouch(0).phase==TouchPhase.Moved)
		{
		    //根据触摸点计算X与Y位置
			x += (float)(Input.GetAxis("Mouse X") * xSpeed * 0.02);
        	y -= (float)(Input.GetAxis("Mouse Y") * ySpeed * 0.02);
 
		}
	}*/
 
	//判断触摸数量为多点触摸
	if(Input.touchCount >1 )
    {
    	//前两只手指触摸类型都为移动触摸
    	if(Input.GetTouch(0).phase==TouchPhase.Moved||Input.GetTouch(1).phase==TouchPhase.Moved)
    	{
    		    //计算出当前两点触摸点的位置
 	   			tempPosition1 = Input.GetTouch(0).position;
				tempPosition2 = Input.GetTouch(1).position;
            	//函数返回真为放大，返回假为缩小
                
            	if(isEnlarge(oldPosition1,oldPosition2,tempPosition1,tempPosition2))
            	{
            		//放大系数超过3以后不允许继续放大
            		//这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                   // KGFMapSystem.UpdateOrthographicSize1();
                    KGFMapSystem.itsCamera.orthographicSize += 10.0f;
                   // myLabel.text = KGFMapSystem.itsCamera.orthographicSize.ToString();
               		/*if(distance > 3)
               		{
               			distance -= 0.5f;
               		}*/
                }
                else
                {
                    //KGFMapSystem.UpdateOrthographicSize2();
                    KGFMapSystem.itsCamera.orthographicSize -= 10.0f;
                }
                
				/*{
                	//缩小洗漱返回18.5后不允许继续缩小
                	//这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                	if(distance < 18.5)
                	{
                		distance += 0.5f;
                	}
            	}*/
            //备份上一次触摸点的位置，用于对比
        	oldPosition1=tempPosition1;
			oldPosition2=tempPosition2;
    	}
    }
	}
    bool isEnlarge(Vector2 oP1,Vector2 oP2,Vector2 nP1,Vector2 nP2)
    {
	    //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
         var leng1 =Mathf.Sqrt((oP1.x-oP2.x)*(oP1.x-oP2.x)+(oP1.y-oP2.y)*(oP1.y-oP2.y));
         var leng2 =Mathf.Sqrt((nP1.x-nP2.x)*(nP1.x-nP2.x)+(nP1.y-nP2.y)*(nP1.y-nP2.y));
         if(leng1<leng2)
         {
         	 //放大手势
             return true;
         }
         else
        {
        	//缩小手势
            return false;
        }
    }
    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
/*void LateUpdate () {
 
    //target为我们绑定的箱子变量，缩放旋转的参照物
    Vector3 to = new Vector3(0.0f, 0.0f, -distance);
    if (target) {		
 
    	//重置摄像机的位置
 		y = (float)(ClampAngle(y, yMinLimit, yMaxLimit));
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * to + target.position;
 
        transform.rotation = rotation;
        transform.position = position;
    }
}
 
double ClampAngle (float angle ,float min ,float max) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}*/
}

