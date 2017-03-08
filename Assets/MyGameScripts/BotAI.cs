using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using System;

namespace Pathfinding {
	
	[RequireComponent(typeof(Seeker))]
	public class BotAI : AIPath {

       private bool firstBlood = false;
       private float curtime;
       private float Reach_Time;  //记录显示到达按钮时间
       private bool Have_Reach;     //已经到达
       private int Flash_Botton1;
       private Transform people1;
       private bool Flag_Firsttime;
        //提示的Label
       public UILabel myLabel;
       public UISprite myTurn;
       public UISprite myDownAndUp;
       public UIAtlas uiAtlas;

       //toast的时间戳
       private float ToastTime = 5f;
       private const float maxToastTime = 3f;
       private bool IsToastTime = false;
       protected static Transform Target_Middle;       //用于在小地图上显示target的红色标志

       public GameObject ToastLabel;
       private float distance;
       private string dis;
       private int dis_int;
       public UILabel ToastDis;
       public  GameObject ToastSprite;
    //   private bool flag;
		public new void Start ()
		{
            
			base.Start ();
           // print("AddMyTarget.Is_AddTarget = " + AddMyTarget.Is_AddTarget);
            f_curtime = 0.0f;
            Reach_Time = 0.0f;
            Have_Reach = false;
            people1 = GameObject.Find("people").transform;
            Flag_Firsttime = true;
          //  ToastDis = GameObject.Find("Label_amp").GetComponent<UILabel>();
        //    ToastSprite = GameObject.Find("Sprite_amp");
            //flag = false;
		}		
		
		public override Vector3 GetFeetPosition ()
		{
			return tr.position;
		}


        //打印Toast
        public void ToastState(string str)
        {
            ToastTime = 0;
            IsToastTime = true;
            //print("ToastState Has Been safjk");
            ToastLabel.active = true;
            UILabel contentLabel = GameObject.Find("ContentLabel").GetComponent<UILabel>();
            contentLabel.text = str;


        }

        /// <summary>
        /// 上传的参数
        /// </summary>
		public void GetPosition()
        {
            if (!firstBlood) {
                MyLocation = ControlChange.People_Position;
                TargetLocation = ControlChange.People_Position;
                firstBlood = true;
            }
            string changePoints = Seeker.ChangePoints;
            MyLocation = TargetLocation;
           
            
            Transform father = GameObject.Find("father").transform;
            foreach (Transform child in father)
            {
                TargetLocation = child.position + "";
                print("******InFather");
                break;
            }
            print("changePoints = " + changePoints);
            print("people_Position = " + MyLocation);
            print("targetPosition = " + TargetLocation);
        }
        void OnGUI()
        {
           // print("AddMyTarget.Is_AddTarget = " + AddMyTarget.Is_AddTarget);
            /*Quaternion rot = people.rotation;
            Quaternion toTarget = Quaternion.LookRotation(targetDirection);
            //rot = Quaternion.Slerp(rot, toTarget, turningSpeed * Time.deltaTime);
            Vector3 euler = rot.eulerAngles;
            euler.z = 0;
            euler.x = 0;
            rot = Quaternion.Euler(euler);
            text.rotation = rot;*/
            // print("target = "+target);
           // print("AddMyTarget.Is_AddTarget = " + AddMyTarget.Is_AddTarget);
            if (Have_Reach)
            {
                //print("OnGUI");
                Reach_Time += Time.deltaTime;
                //GUI.Button(new Rect(30, 10, 200, 200), "Have_Reach", myStyle);
              //  myLabel.text = "到达目标";
                ToastState("到达目标!!");
            }
            if (Reach_Time > 5.0f)
            {
                Have_Reach = false;
                Reach_Time = 0.0f;
            }
            if (target == null)
                return;
            //Vector3 relativePos = people.position - target.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            //transform.rotation = rotation;
            float num1, num2;
            //print("text.rotation.y = " + text.rotation.y);
            //print("text.rotation.w =" + text.rotation.w);
            //print("text = " + text.rotation);
            // print("rotation = " + rotation);
            // print("people.rotation = " + people.rotation);
            // print("rotation.y = " + rotation.y);
            // print("rotation.w = " + rotation.w);
            num1 = Math.Abs(Math.Abs(rotation.y) - Math.Abs(people1.rotation.y));
            num2 = Math.Abs(Math.Abs(rotation.w) - Math.Abs(people1.rotation.w));
            
            
            Flash_Botton1++;
            if (Flash_Botton1 > 40)
                Flash_Botton1 = 0;
            if (Flash_Botton1 < 20)
            {
                if (target == null)
                    return;
                if (people1.position.y - target.position.y > 2.0f)
                {
                 //   GUI.Button(new Rect(350, 100, 200, 200), "down", myStyle);
                    myDownAndUp.active = true;
                    myDownAndUp.atlas = uiAtlas;
                    myDownAndUp.spriteName = "xiangshang";
                }

                if (target.position.y - people1.position.y > 2.0f)
                {
                    myDownAndUp.active = true;
                //    GUI.Button(new Rect(350, 100, 200, 200), "up", myStyle);
                    myDownAndUp.atlas = uiAtlas;
                    myDownAndUp.spriteName = "xiangxia";
                }
               if (num1 > 0.3f || num2 > 0.3f)
                {
                //print("Turn");
                //  GUI.Button(new Rect(350, 10, 200, 200), "Turn", myStyle);
                //  myLabel.text = "转弯";
                myTurn.active = true;
                myTurn.atlas = uiAtlas;
                myTurn.spriteName = "youzhuan";
              }
                //Quaternion q1 = people.rotation;
                // Quaternion q2 = Quaternion.LookRotation(target.position);
                /* print("q1.w = " + q1.w);
                 print("q1.x = " + q1.x);
                 print("q1.y = " + q1.y);
                 print("q1.z = " + q1.z);
                 print("q1 = " + q1);*/
                //float num;

                //print("num1 = " + num1);
                // print("num2 = " + num2);
                //print("text.rotation = " + text.rotation);
                // print("people.rotation =" + people.rotation);
                //print("rot = " + rot);
                //print("targetDirection = " + targetDirection);
                /*
                Vector3 relativePos = target.position - people.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                print("rotation = " + rotation);*/
                /* transform.rotation = rotation;
                 num = Quaternion.Angle(people.rotation, rotation);
                 print("num = " + num);
                 print("target = " + target);*/

                /*Vector3 vec1,vec2;
                Quaternion q = tr.rotation;
                vec1 = people.position;
                vec2 = targetDirection;
                q.SetFromToRotation(vec1,vec2);
                print("q = " + q);*/
                /*
                Quaternion rot = tr.rotation;
                Quaternion toTarget = tr.rotation;
                toTarget.SetLookRotation(people.position, targetDirection);
                Transform tt;
                tt.rotation = */

                /*rot = Quaternion.Slerp(rot, toTarget, turningSpeed * Time.deltaTime);
                Vector3 euler = rot.eulerAngles;
                euler.z = 0;
                euler.x = 0;
                rot = Quaternion.Euler(euler);

                tr.rotation = rot;*/
                /*if (people.rotation != toTarget)
                {
                    GUI.Button(new Rect(350, 10, 200, 200), "Turn", myStyle);
                }*/
            }
            else {
                myDownAndUp.active = false;
              
                myTurn.active = false;
             //   myTurn.enabled = false;
            }
          
            if (Have_Reach)
            {
                //print("OnGUI");
                Reach_Time += Time.deltaTime;
             //   GUI.Button(new Rect(30, 10, 200, 200), "Have_Reach", myStyle);
            }
            if (Reach_Time > 5.0f)
            {
                Have_Reach = false;
                Reach_Time = 0.0f;
            }

            /*
            if (target != myParent.transform)
            {
                if (flag)
                {
                    //Debug.Log("flag == true");
                    f_curtime += Time.deltaTime;
                    GUI.Label(new Rect(350, 100, 200, 200), "You have reached " + target1.name + " !", myStyle);
                    if (f_curtime >= 3.0f)
                    {
                        f_curtime = 0.0f;
                        flag = false;
                    }
                }
                switch (step)
                {
                    case 1:
                        GUI.Label(new Rect(350, 10, 200, 200), "The " + target.name + " is on the B1!", myStyle);
                        break;
                    case 2:
                        GUI.Label(new Rect(350, 10, 200, 200), "The " + target.name + " is on the F1!", myStyle);
                        break;
                    case 3:
                        GUI.Label(new Rect(350, 10, 200, 200), "The " + target.name + " is on the F2!", myStyle);
                        break;
                }
            }
            else {
                f_curtime += Time.deltaTime;
                if (f_curtime <= 3.0f)
                {
                    GUI.Label(new Rect(350, 100, 200, 200), "Navigation process is over !", myStyle);
                    isReach = false;
                    ReachInSeeker = false;
                }
            }*/
        }
        /*void OnGUI()
        {
            //print("***");
            
        }*/
		public  void Update () {
            /*
            if (!IsToastTime)
            {
                return;
            }*/
            if (target == null)
            {
                //print("target = null");
                ToastSprite.SetActive(false);
            }
            else
            {
                ToastSprite.SetActive(true);
                distance = Mathf.Sqrt((people1.position.x - target.position.x) * (people1.position.x - target.position.x) + (people1.position.z - target.position.z) * (people1.position.z - target.position.z));
                distance = distance * 2.04366f*2;
                dis_int = (int)distance;
                //print("distance = " + distance);
                dis = dis_int.ToString();
                ToastDis.text = dis + "m";
            }
            if (IsToastTime)
            {
                if (ToastTime > maxToastTime)
                {
                    ToastLabel.active = false;
                    IsToastTime = false;
                }
                else
                {
                    ToastTime = ToastTime + Time.deltaTime;

                }
            }
            /*curtime += Time.deltaTime;
            if (curtime > 10.0f)
            {
                curtime = -100.0f;
                //flag = true;
                OnTargetReached();
                print("OnTargetReached!!!");
            }*/
            if (AIPath.target == null)
            {
                Seeker.Exist_target = false;
            }
            else
                Seeker.Exist_target = true;
			if (KGFMapSystem.IsChangeTarget) {
				KGFMapSystem.IsChangeTarget = false;
				OnTargetReached();
			}
            if (isReach) {
                GetPosition();
                isReach = false;
                //KGFMapSystem.To_Target = true;
                if (Flag_Firsttime)
                {
                    Flag_Firsttime = false;
                }
                else
                    Have_Reach = true;
                //print("Have_Reach is true");
            }
            
            if (AddMyTarget.Is_AddTarget)
            {
                
                OnTargetReached();
                AddMyTarget.Is_AddTarget = false;
                
            }
			//print("IsOk = " +IsOk);
			//Get velocity in world-space
			// GUI.Button(new Rect(210, 30, 150, 150), "asfhusbng");
			if (IsOk) {
				targetPoints = GameObject.Find("father").transform;
				IsOk = false;
                
				// print("isoK");
				//print("TargetPoints = "+targetPoints.transform.childCount);
			}
			
			
			Vector3 velocity;
			if (canMove) {
				
				//Calculate desired velocity
				Vector3 dir = CalculateVelocity (GetFeetPosition());
               /* Vector3 po;
                po = GetFeetPosition();
                print("GetFeetPosition = " + po);*/
				//Rotate towards targetDirection (filled in by CalculateVelocity)
                if (IsNav)
                {
                    RotateTowards(targetDirection);
                   // print("targetDirection = " + targetDirection);
                }
					
				
				dir.y = 0;
				if (dir.sqrMagnitude < 0.2f) { 
					dir = Vector3.zero;
				}
				
				if (controller != null){
					if(IsNav)
						controller.SimpleMove(dir);
					Console.WriteLine("124124");
				}
				else
					Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
				
				velocity = controller.velocity;
			} else {
				velocity = Vector3.zero;
			}
			
			
			//Animation
			
			//Calculate the velocity relative to this transform's orientation
			/*
			Vector3 relVelocity = tr.InverseTransformDirection (velocity);
			relVelocity.y = 0;


			if (velocity.sqrMagnitude > 0.2f) 
			{
				float speed = relVelocity.z;
			}*/
		}
	}
}
