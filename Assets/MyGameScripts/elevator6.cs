using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class elevator6 : MonoBehaviour
{
    public CharacterController hero;        //人物
    public Transform ele;               //电梯板
    public GUIStyle myStyle;
    public Transform open1;         //电梯的门
    public Transform open2;
    public Transform open3;
    public Transform open4;
    public Transform open5;
    public Transform open6;
    protected bool flag = false;
    protected bool Select_A = false;            //选择的楼层
    protected bool Select_B = false;
    protected bool Select_C = false;
    protected bool flagshow = false;
    protected float floor1 = 5.1f;          //每层楼的高度
    protected float floor2 = 17.0f;
    protected float floor3 = 27.5f;
    protected float floor = 5.0f;
    protected float M = 1.8f;
    protected bool reach = false;           //判断是否到达
    protected bool reach1 = false;
    protected bool Is_Close_Door = false;
    protected float f_curtime = 0.0f;
    protected float f_curtime1 = 0.0f;
    protected float Close_Time = 0.0f;
    protected float f_curtime2 = 0.0f;      //用于记录电梯到达后的关门时间间隔
    protected string s;
    protected float Same_Floor = 8.0f;      //判断是否在同一楼层的距离参数
    protected float On_Floor_Distance = 2.8f;       //判断是否在电梯板上的距离参数
    Vector3 po;             //电梯门的位置参数
    Vector3 po1;
    Vector3 po2;
    Vector3 po3;
    Vector3 po4;
    Vector3 po5;
    Vector3 po6;
    protected float temp1 = 210.75f;         //电梯门的开门距离
    protected float temp2 = 205.91f;
    protected float temp3 = 209.0f;
    protected float temp4 = 207.32f;
    protected bool Is_Click_Button = false;     //判断电梯按钮是否被按上
    protected bool End_Opendoor = false;        //判断是否结束开门
    protected bool End_Closedoor = false;       //判断是否结束关门
    protected bool End_Rise = false;            //判断是否结束上升
    protected bool flagdown1 = false;
    protected bool flagdown2 = false;
    protected bool Is_Same_Floor = false;           //判断是否在同一楼层，用于之后开门的判断
    protected bool Is_Onplane = false;          //判断人物是否在电梯板上
    private GameObject People;
    private GameObject Plane;

    //电梯按钮
    public UISprite myLift;
    // Use this for initialization
    public void Awake()
    {
        People = GameObject.Find("people");
        Plane = GameObject.Find("eleplane6");
    }
    public void Judge_Samefloor()                 //用于计算电梯板和游戏体的距离差
    {
        Vector3 People_Position;
        Vector3 Plane_Position;
        float x, y, z;
        People_Position = People.transform.position;
        Plane_Position = Plane.transform.position;
        x = Math.Abs(People_Position.x - Plane_Position.x);     //计算位置差
        y = Math.Abs(People_Position.y - Plane_Position.y);
        z = Math.Abs(People_Position.z - Plane_Position.z);
        // print("x = " + x);
        // print("y = " + y);
        //  print("z = " + z);
        if (x < Same_Floor && z < Same_Floor && y < 2.0f)           //如果电梯与游戏体在同一层楼，则可以开门
        {
            Is_Same_Floor = true;
            //print("Is_Same_Floor is true!!!!!");
        }
        else
            Is_Same_Floor = false;
    }
    public void Judge_Onplane()                 //用于计算电梯板和游戏体的距离差
    {
        Vector3 People_Position;
        Vector3 Plane_Position;
        float x, y, z;
        People_Position = People.transform.position;
        Plane_Position = Plane.transform.position;
        x = Math.Abs(People_Position.x - Plane_Position.x);
        y = Math.Abs(People_Position.y - Plane_Position.y);
        z = Math.Abs(People_Position.z - Plane_Position.z);
        // print("x = " + x);
        //  print("y = " + y);
        //  print("z = " + z);
        /*if (Select_A == false && Select_B == false && Select_C == false)
        {
            if (x < On_Floor_Distance && z < On_Floor_Distance && y < 2.0f)           //如果电梯与游戏体在同一层楼，则可以开门
            {
                Is_Onplane = true;
            }
            else
                Is_Onplane = false;
        }
        else
        {
            Is_Onplane = false;

        }*/
        if (x < On_Floor_Distance && z < On_Floor_Distance && y < 2.0f)          //如果电梯与游戏体在同一层楼，则可以开门
        {
            Is_Onplane = true;
            myLift.active = true;
        }
        else
        {
            Is_Onplane = false;
            myLift.active = false;
        }
    }
    public void Judge_Close()
    {
        Vector3 People_Position;
        Vector3 Plane_Position;
        
        float x, y, z;
        People_Position = People.transform.position;
        Plane_Position = Plane.transform.position;
        x = Math.Abs(People_Position.x - Plane_Position.x);
        y = Math.Abs(People_Position.y - Plane_Position.y);
        z = Math.Abs(People_Position.z - Plane_Position.z);
        // print("x = " + x);
        //  print("y = " + y);
        //  print("z = " + z);
        if (x < On_Floor_Distance && z < On_Floor_Distance && y < 2.0f)           //如果电梯与游戏体在同一层楼，则可以开门
        {
            /*if (reach)
                Is_Close_Door = true;*/
            //print("Is_Close_Door = false;");
            Is_Close_Door = false;
        }
        else
            Is_Close_Door = true;

    }
    public void Rise()          //上升函数
    {
        po = ele.position;
        Vector3 dir = hero.transform.position;
        if (po.y < floor - 0.3)
        {
            ele.Translate(Vector3.up * 0.05f);
            if (System.Math.Abs(dir.x - po.x) < M && System.Math.Abs(dir.z - po.z) < M)     //如果人站在电梯板上，才会升上去
            {
                hero.Move(Vector3.up * 0.05f);
                flagshow = true;
            }

        }
        else if (po.y > floor)
        {
            ele.Translate(Vector3.down * 0.05f);
            if (System.Math.Abs(dir.x - po.x) < M && System.Math.Abs(dir.z - po.z) < M)
            {
                hero.Move(Vector3.down * 0.05f);
                flagshow = true;
            }
        }
        else
        {
            print("reach = true");
            reach = true;           //到达
            End_Rise = true;        //结束
        }
    }
    void OnMouseDown()          //判断鼠标是否已经点击电梯门按钮
    {
        print("Is_Click_Button is true!!!!");
        Is_Click_Button = true;
        print("Is_Click_Button = " + Is_Click_Button);
    }
    void OnGUI()
    {

        Judge_Onplane();
        /*
        if (Is_Onplane)         //站在电梯板上才会显示
        {         //如果点击了电梯按钮
            if (GUI.Button(new Rect(200, 30, 50, 50), "B1"))
            {
                floor = floor1;
                Select_A = true;
                //print("Select_A = " + Select_A);
            }
            if (GUI.Button(new Rect(260, 30, 50, 50), "F1"))
            {
                floor = floor2;
                Select_B = true;
                //print("Select_B = " + Select_B);
            }
            if (GUI.Button(new Rect(320, 30, 50, 50), "F2"))
            {
                floor = floor3;
                Select_C = true;
                //print("Select_C = " + Select_C);
            }

            //flag = false;
        }
         * */
        if (reach)
        {            //如果到达某一层楼
            //f_curtime += Time.deltaTime;        //开始计时
            if (Select_A)
            {
                s = "B1 !";
            }
            else if (Select_B)
            {
                s = "F1 !";
            }
            else if (Select_C)
            {
                s = "F2 !";
            }
            if (flagshow)
                GUI.Label(new Rect(350, 100, 200, 200), "您已经到达 " + s, myStyle);
            /*if (f_curtime >= 5.0f)
            {
               // f_curtime = 0.0f;
                reach = false;
                flagshow = false;
                reach1 = true;
            }*/
            Select_A = false;
            Select_B = false;
            Select_C = false;
            // print("Reach!");
        }

    }


    public void CliclkB1()
    {
        floor = floor1;
        Select_A = true;
    }

    public void CliclkF1()
    {
        floor = floor2;
        Select_B = true;
    }

    public void CliclkF2()
    {
        floor = floor3;
        Select_C = true;
    }
    // Update is called once per frame
    public void Open_Door()             //开门函数，这里方便操作，就让三层楼的门都打开，就不用判断点击的是哪一层的按钮
    {
        po1 = open1.position;
        po2 = open2.position;
        po3 = open3.position;
        po4 = open4.position;
        po5 = open5.position;
        po6 = open6.position;
        if (po1.x < temp1 - 0.1)
        {
            open1.Translate(Vector3.right * 0.02f * 2);
        }
        if (po2.x > temp2 - 0.1)
        {
            open2.Translate(Vector3.left * 0.02f * 2);
        }
        else
            End_Opendoor = true;
        if (po3.x < temp1 - 0.1)
        {
            open3.Translate(Vector3.right * 0.02f * 2);
        }
        if (po4.x > temp2 - 0.1)
        {
            //print("po4 move!!!!");
            open4.Translate(Vector3.left * 0.02f * 2);
        }

        if (po5.x < temp1 - 0.1)
        {
            open5.Translate(Vector3.right * 0.02f * 2);
        }
        if (po6.x > temp2 - 0.1)
        {
            open6.Translate(Vector3.left * 0.02f * 2);
        }

    }
    public void Close_Door()            //关门函数，这里同样也是让所有的门都关闭，节省操作复杂度。
    {
        //print("Close_Door has been invoked!!!");
        po1 = open1.position;
        po2 = open2.position;
        po3 = open3.position;
        po4 = open4.position;
        po5 = open5.position;
        po6 = open6.position;
        if (po1.x > temp3 - 0.1)
        {
            //print("PO1!!!");
            open1.Translate(Vector3.left * 0.02f * 2);
        }
        if (po2.x < temp4 - 0.1)
        {
            // print("PO2");
            open2.Translate(Vector3.right * 0.02f * 2);
        }
        else
            End_Closedoor = true;
        if (po3.x > temp3 - 0.1)
        {
            open3.Translate(Vector3.left * 0.02f * 2);
        }
        if (po4.x < temp4 - 0.1)
        {
            open4.Translate(Vector3.right * 0.02f * 2);
        }
        if (po5.x > temp3 - 0.1)
        {
            open5.Translate(Vector3.left * 0.02f * 2);
        }
        if (po6.x < temp4 - 0.1)
        {
            open6.Translate(Vector3.right * 0.02f * 2);
        }
    }
    void Update()
    {
        if (Is_Click_Button)            //如果游戏体按下电梯门按钮
        {
            ///print("Is_Click_Button = " + Is_Click_Button);
            //print("*****");

            Judge_Samefloor();          //判断是否电梯和游戏体是否在同一层楼
            if (Is_Same_Floor)                  //如果在同一层楼则打开电梯门
            {
                Open_Door();
                if (End_Opendoor)
                {
                    Is_Click_Button = false;
                    End_Opendoor = false;
                }
            }
        }
        if (reach)          //到达打开电梯门
        {
            reach1 = true;
            Open_Door();
            if (End_Opendoor)
            {
                reach = false;
                End_Opendoor = false;
            }
            //Is_Close_Door = true;
            /*Judge_Close();
            if (Is_Close_Door)
            {
                
            }
            if (End_Closedoor)
                reach = false;*/
        }
        if (reach1)         //如果到达 ,则开始计时
        {
            f_curtime += Time.deltaTime;
            // print("f_curtime = " + f_curtime);
        }

        if (f_curtime > 3.0f)       //到达一定时间，则关门
        {
            // print("f_curtime > 3.0f");
            Close_Door();
            if (End_Closedoor)
            {
                End_Closedoor = false;
                f_curtime = 0.0f;
                reach1 = false;
            }
        }
        /*
        else
        {
            if (f_curtime > 5.0f)
            {
                if (!End_Closedoor)
                    Close_Door();
                else
                    f_curtime = 0.0f;
            }
                
        }*/
        /*if (reach1) {
            f_curtime2 += Time.deltaTime;
            po1 = open3.position;
            po2 = open4.position;
            if (!flagdown1)
            {
                /* print("po1.z ="+po1.z);
                print("temp1 =" + temp1);
                // print("55555");
                if (po2.z > temp2 - 0.1)
                {
                    open4.Translate(Vector3.back * 0.02f * 2);
                }
                if(f_curtime2 > 8.0f)
                {
                    // print("44444");
                    flagdown1 = true;       //用于停止电梯的开门
                    flagdown2 = false;
                }
				
                if (po1.z < temp1 - 0.1)
                {
                    open3.Translate(Vector3.forward * 0.02f * 2);
                }
            }
            if (f_curtime2 > 8.0f)
            {
                //flagdown1 = false;
                if (!flagdown2)
                {
                    //print("*****");
                    if (po2.z < temp4 - 0.1)
                    {
                        open4.Translate(Vector3.forward * 0.02f * 2);
                        // print("111111");
                    }
                    else
                        flagdown2 = true;
                    if (po1.z > temp3 - 0.1)
                    {
                        open3.Translate(Vector3.back * 0.02f * 2);
                    }
                }
				
            }
        }*/

        //Vector3 dir = CalculateVelocity(GetFeetPosition());
        if (Select_A || Select_B || Select_C)           //选定楼层之后，关门并计时，到达一定时间就上升
        {
            Is_Click_Button = false;
            // print("Is_Click_Button = " + Is_Click_Button);
            Close_Door();
            Close_Time += Time.deltaTime;
            if (Close_Time > 3.0f)
            {
                Rise();
                End_Closedoor = false;
                if (End_Rise)
                {
                    Close_Time = 0.0f;
                    End_Rise = false;
                }

            }

        }

    }
}
