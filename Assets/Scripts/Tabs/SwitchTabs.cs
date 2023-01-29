using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is located in gameobject: _manager
public class SwitchTabs : MonoBehaviour
{
    //Tickets panel = 0
    //Users panel = 1
    //Sites panel ?= 2 (Sites may be disabled until users is fully done)
    //Performance panel = 2 or 3
    public GameObject[] tabs;

    public enum Tabs { TICKETS, USERS, SITES, PERFORMANCE }
    public static Tabs currentTab;

    public Texture2D cursorImage;

    //Transition days
    public GameObject dayTransitionVeil;
    public GameObject daysObj;

    //Transition days animation
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        //Switch tab in beginning to tickets
        StartCoroutine(LateStart(0.05f));
        
    }


    public IEnumerator LateStart(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        switchTabs(0);

    }

    //Deprecated (May remove it)
    public GameObject returnTabObj(Tabs tab)
    {
        GameObject returnObj;
        switch (tab)
        {
            case Tabs.TICKETS:
                returnObj = tabs[0];
                break;
            case Tabs.USERS:
                returnObj = tabs[1];
                break;
            case Tabs.SITES:
                returnObj = tabs[2];
                break;
            case Tabs.PERFORMANCE:
                returnObj = tabs[3];
                break;
            default:
                returnObj = tabs[0];
                break;
        }

        return returnObj;
    }

    public void switchTabs(int tabNum)
    {

        if (!GameManager.DISABLE_TABS)
        {
            switch (tabNum)
            {
                case 0:
                    tabs[0].SetActive(true);
                    tabs[1].SetActive(false);
                    tabs[3].SetActive(false);
                    currentTab = Tabs.TICKETS;

                    break;
                case 1:
                    tabs[0].SetActive(false);
                    tabs[1].SetActive(true);
                    tabs[3].SetActive(false);
                    currentTab = Tabs.USERS;
                    break;
                case 3:
                    tabs[0].SetActive(false);
                    tabs[1].SetActive(false);
                    tabs[3].SetActive(true);
                    currentTab = Tabs.PERFORMANCE;

                    //We only will reset the notification if the user clicks this performance tab.
                    if (GameManager.instance.performanceManager.PerformanceNotification)
                    {
                        print("perform notification");
                        //remove the notification 
                        GameManager.instance.performanceManager.ResetBossNotification();
                        //Show the button to proceed to next day

                    }

                    break;
            }
            SoundManage.PlaySound(SoundManage.Sound.SelectTab);
        }


    }


}

