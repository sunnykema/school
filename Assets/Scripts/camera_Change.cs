using UnityEngine;
using System.Collections;

public class camera_Change : MonoBehaviour
{

    // Use this for initializationd
    public Transform m_camTransform;
    private Transform m_transform;
    protected float m_camHeight = 0.4f;
    //protected float z = 0.2f;
    void Start()
    {

    }
   /* void OnGUI()
    {
        if (GUI.Button(new Rect(57.6f, 10f, 150f, 50f), "切换回第一人称"))
        {
            m_transform = this.transform;
            Vector3 pos = m_transform.position;
            pos.y += m_camHeight;
            //pos.z += z;
            m_camTransform.position = pos;
            m_camTransform.rotation = m_transform.rotation;
            // m_camRot = m_camTransform.eulerAngles;
        }
    }
    * */
    void Update()
    {

    }


    public void ChangeView() {
        m_transform = this.transform;
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        //pos.z += z;
        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;
        // m_camRot = m_camTransform.eulerAngles;
    }
}