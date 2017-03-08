using UnityEngine;
using System.Collections;

public class ChangePosition : MonoBehaviour
{

    // Use this for initialization
    protected float x;
    protected float y;
    protected float z;
    private const float Floor1 = 5.5f;
    private const float Floor2 = 16.6f;
    private const float Floor3 = 27.3f;

    private const float Floor11 = 4.5f;
    private const float Floor22 = 15.0f;
    private const float Floor33 = 23.8f;


    private string temp;
    private float[] arr = { 0, 0, 0 };
    private GameObject people;
    private Transform tran;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangePeople(float ax, float ay, float az)
    {
        if (az - 0.0 < 0.1)
        {
            x = 289.31f - ax * 102.97f;
            y = Floor1;
            z = 213.2f - ay * 118.82f;
        }
        else if (az - 1.0 < 0.1)
        {
            print("az =  " + az);
            x = 289.31f - 105.19f*ax;
            y = Floor2;
            z = 213.2f - 101.53f*ay;
        }
        else
        {
            x = 289.31f - 105.19f * ax;
            y = Floor3;
            z = 213.2f - 101.53f * ay;
        }
    }
    
    public void ChangeLocation(float ax, float ay, float az)
    {
        if (az - 0.0 < 0.1)
        {
            x = 289.31f - ax * 102.97f;
            y = Floor1+1.5f;
            z = 213.2f - ay * 118.82f;
        }
        else if (az - 1.0 < 0.1)
        {
            print("az =  " + az);
            x = 289.31f - 105.19f * ax;
            y = Floor2+1.5f;
            z = 213.2f - 101.53f * ay;
        }
        else
        {
            x = 289.31f - 105.19f * ax;
            y = Floor3+1.5f;
            z = 213.2f - 101.53f * ay;
        }
    }
    public void ChangePositionByTwoDemOrNFC(string str)
    {
        temp = str;
        int k = 0;
        int flag = 0;
        string tmp = "";
        try
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (temp[i] == '[')
                {
                    continue;
                }
                if (temp[i] == ',' || temp[i] == ']')
                {
                    //print("tmp = "+tmp);
                    arr[k] = float.Parse(tmp);
                    tmp = "";
                    /*s
                    for (int j = i - 1; j >= 0; j--) {
                        if (temp[i] == '.')
                            break;
                        arr[k] += temp[i];
                        arr[k] /= 10;
                    }
                     * */
                    k++;
                }
                else
                    tmp += temp[i];
            }
        }
        catch
        {

        }
        people = GameObject.Find("people");
        // print("People = "+people);
        tran = people.transform;
        print("arr[0] = " + arr[0]);
        print("arr[1] = " + arr[1]);
        print("arr[2] = " + arr[2]);
        Vector3 po;
        po = tran.position;
        po.x = arr[0];
        po.y = arr[1];
        po.z = arr[2];
        // ChangeLocation(arr[0], arr[1], arr[2]);
        // po.x = GetX();
        // po.y = GetY();
        // po.z = GetZ();
        // po.x = 400 - arr[0] * 217;
        // po.y = arr[2] * 40 + 12;
        //   po.z = 225 - arr[1] * 180;
        people.transform.position = po;
        print(po.x);
        print(po.y);
        print(po.z);
    }

    //获得人物当前在第几层
    public int GetStairs(float Mylocation)
    {
        if (Mylocation < Floor22)
        {
            return 0;
        }
        else if (Mylocation < Floor33)
        {
            return 1;
        }
        else
            return 2;
    }

    public float GetX()
    {
        return x;
    }
    public float GetY()
    {
        return y;
    }
    public float GetZ()
    {
        return z;
    }
}
