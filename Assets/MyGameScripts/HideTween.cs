using UnityEngine;
using System.Collections;

public class HideTween : MonoBehaviour
{
    public GameObject MyGrid;
    private static bool IsInhide = true;
    private static bool flag = false;
    private static int cnt = 0;
    /// <summary>
    /// ClickGridChild 隐藏Gird和MyGrid中的Tween
    /// </summary>
    public void ClickGridChild()
    {
        flag = false;
        int len = this.transform.childCount;
        foreach (Transform child in this.transform)
        {

            UIPlayTween uiPlayTween = child.GetComponent<UIPlayTween>();
            // uiPlayTween.active = false;

            if (cnt != 0)
            {
                print("childInGrid = " + child.name);
                uiPlayTween.Play(false);
                flag = true;
            }



        }

        //GameObject MyGrid = GameObject.Find("MyGrid");

        len = MyGrid.transform.childCount;
        foreach (Transform child in MyGrid.transform)
        {

            UIPlayTween uiPlayTween = child.GetComponent<UIPlayTween>();
            // uiPlayTween.active = false;

            if (cnt != 0)
            {
                print("childInMyGrid = " + child.name);
                uiPlayTween.Play(false);
                flag = true;
            }

            //  uiPlayTween.playDirection = AnimationOrTween.Direction.Reverse;
        }
        if (flag || cnt == 0)
        {
            IsInhide = !IsInhide;
        }
        cnt++;
    }
}
