using UnityEngine;
using System.Collections;

public class AndroidToast : MonoBehaviour {
    private GUITexture guiTexture;
    private GUIText guiText;
    private float x;
    private bool sholdMove = true;
    private bool shouldFadeOut = true;
    public float stopMoveTime = 0;
    public bool shouldPlay = true;
    private float winStoreNotificationCenterStayTime = 3f;
    private float androidToastStayTime = 0.5f;
    public enum NotificationStyle { AndroidToast, WinStoreNotificationCenter };

    public NotificationStyle currentNotificationStyle = NotificationStyle.WinStoreNotificationCenter;
    void Start()
    {
        guiTexture = GetComponent<GUITexture>();
        guiText = GetComponent<GUIText>();

        //audio.PlayDelayed(0.5f);

        switch (currentNotificationStyle)
        {
            case NotificationStyle.AndroidToast:

                Vector3 pos = guiTexture.gameObject.transform.position;
                pos.x = 0.5f - (guiTexture.pixelInset.x / Screen.width) * 0.5f;
                pos.y = 0.5f + (guiTexture.pixelInset.y / Screen.height) * 0.5f;
                guiTexture.gameObject.transform.position = pos;
                guiText.alignment = TextAlignment.Center;

                shouldFadeOut = true;

                break;

            case NotificationStyle.WinStoreNotificationCenter:

                x = 1 - (guiTexture.pixelInset.x / Screen.width) - 0.02f;
                shouldFadeOut = false;
                break;
            default:

                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentNotificationStyle)
        {
            case NotificationStyle.AndroidToast:

                if (stopMoveTime != 0 && stopMoveTime + androidToastStayTime <= Time.time)
                {
                    shouldFadeOut = true;
                }

                if (shouldFadeOut)
                {
                    Color temp = guiTexture.color;
                    temp.a -= Time.deltaTime / 2f;
                    guiTexture.color = temp;
                    if (guiTexture.color.a <= 0)
                    {
                        shouldFadeOut = false;
                        Destroy(this.gameObject);
                    }
                }

                break;
            case NotificationStyle.WinStoreNotificationCenter:

                if (sholdMove)
                {
                    guiTexture.transform.Translate(new Vector3(-0.1f, 0, 0) * Time.deltaTime * 2);
                    if (guiTexture.transform.position.x <= x)
                    {
                        sholdMove = false;
                        stopMoveTime = Time.time;
                    }
                }

                if (stopMoveTime != 0 && stopMoveTime + winStoreNotificationCenterStayTime <= Time.time)
                {
                    shouldFadeOut = true;
                }

                if (shouldFadeOut)
                {
                    Color temp = guiTexture.color;
                    temp.a -= Time.deltaTime;
                    guiTexture.color = temp;
                    if (guiTexture.color.a <= 0)
                    {
                        shouldFadeOut = false;
                        Destroy(this.gameObject);
                    }
                }

                break;

            default:
                break;
        }
    }
}

