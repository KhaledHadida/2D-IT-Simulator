using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//CLass that handles everything in the performance tab
public class PerformanceTab : MonoBehaviour
{
    //all the ticket's time it took to finish
    public List<int> ticketTimes;

    private GameObject performanceTab;

    private int timeSoFar;

    //Text from the boss
    public string bossText;
    public TextMeshProUGUI bossTextObj;

    //These are for the notifications when a boss requests the player's pressence.
    public GameObject performanceCheck;
    public Animator performanceCheckAnimator;
   
    [SerializeField]
    private GameObject arrowNotification;
    //This is the parent of the arrow gameobj, since we cannot change the position of animated arrow.
    private GameObject arrowNotificationParent;

    //Failed ticket counter
    private int failedTicketsNum;
    private int successTicketsNum;
    private int averageTicketTime;
    private int totalTicketsDone;
    //Warnings for job performance
    private int warnings = -1;

    private Animator warningAnimate;
    private bool reachedDestination = false;

    [SerializeField]
    private SwitchTabs tabsManager;

    public int FailedTicketsNum
    {
        get => failedTicketsNum;
        set => failedTicketsNum = value;
    }

    public int SuccessTicketsNum
    {
        get => successTicketsNum;
        set => successTicketsNum = value;
    }

    public int AverageTicketTime
    {
        get { return averageTicketTime; }
        set {
          timeSoFar += value;
          averageTicketTime = timeSoFar/totalTicketsDone; }
    }

    public int TotalTicketsDone
    {
        get => totalTicketsDone;
        set => totalTicketsDone = value;
    }

    public int Warnings
    {
        get => warnings;
        set => warnings = value;
    }

    private bool performaneNotification;

    public bool PerformanceNotification
    {
        get => performaneNotification;
        set => performaneNotification = value;
    }

    //Objects
    public TextMeshProUGUI failedTicketsObj;
    public TextMeshProUGUI successTicketsObj;
    public TextMeshProUGUI averageTicketTimeObj;
    public TextMeshProUGUI totalTicketsDoneObj;

    //Proceed button 
    [SerializeField]
    private GameObject proceedButton;

    //the strikes (they are 3)
    [SerializeField]
    private List<Image> strikeIcons;

    //this is stored image so we can indicate a strike is given
    [SerializeField]
    private Sprite grayedOutStrike;
    //The animation for the three warnings (3)
    public Animator warningsAnimator;
    [SerializeField]
    private TextMeshProUGUI warningText;

    string angryBossResponse = "You have a ticket success rate of: {0}%\nThe boss does not seem to like your performance.. You need to increase your ticket success rate to {1}%!\n\nYou have been given a warning!!";
    string happyBossResponse = "You have a ticket success rate of: {0}%\nThe boss seems to like your performance.. Make sure to stay at a ticket success rate of at least {1}%!\n\nKeep up the good work!";
    string terminationResposne = "You have deleted the user database!\nYou are fired!";

    // Start is called before the first frame update
    void Start()
    {
        if (performanceTab == null)
        {
            performanceTab = GameObject.Find("PerformancePanel");
        }
        
        if (!failedTicketsObj)
        {
            failedTicketsObj = GameObject.Find("TicketsFailed").GetComponent<TextMeshProUGUI>();

        }
        if (!successTicketsObj)
        {
            successTicketsObj = GameObject.Find("TicketsSuccess").GetComponent<TextMeshProUGUI>();

        }
        if (!averageTicketTimeObj)
        {
            averageTicketTimeObj = GameObject.Find("AverageTime").GetComponent<TextMeshProUGUI>();

        }
        if (!totalTicketsDoneObj)
        {
            totalTicketsDoneObj = GameObject.Find("TicketsDone").GetComponent<TextMeshProUGUI>();

        }

        //Update the fields after each day is ended
        UpdateFields();

        //Close the dialogue after we intialized all fields!
        GameManager.instance.CloseTransitionDayBox();

        StartCoroutine(GameManager.AnimateTexts(bossTextObj, "The boss has no comments as of now...", 0.02f));

        if (arrowNotification != null)
        {
            warningAnimate = arrowNotification.GetComponent<Animator>();
        }
        else
        {
            arrowNotification = GameObject.Find("Arrow");
            warningAnimate = arrowNotification.GetComponent<Animator>();
        }
        performanceCheckAnimator = performanceCheck.GetComponent<Animator>();
        arrowNotificationParent = arrowNotification.transform.parent.gameObject;

        if(proceedButton == null)
        {
            proceedButton = GameObject.Find("Proceed");
        }
        proceedButton.SetActive(false);
    }

    public void UpdateFields()
    {
        failedTicketsObj.text = "Failed tickets: " + failedTicketsNum;
        successTicketsObj.text = "Successful tickets: " + successTicketsNum;
        averageTicketTimeObj.text = "Average Time: " + averageTicketTime;
        totalTicketsDoneObj.text = "Number of tickets done: " + totalTicketsDone;

       /* print("Tickets failed = " + failedTicketsNum);
        print("Tickets success = " + successTicketsNum);
        print("Tickets done = " + totalTicketsDone);
        print("Tickets time elapsed = " + averageTicketTime + " Time so far " + timeSoFar);*/
    }

    //Boss will check on player ever 3-5 days randomly
    public void InitiateBoss()
    {
        StartCoroutine(initiate(this.gameObject));
    }

    private IEnumerator initiate(GameObject tabObj)
    {
        //Give it a second so that any other animation would be done!
        yield return new WaitForSeconds(1f);
        //Spawn the notification
        performanceCheck.SetActive(true);

        //Shake the "performance check" text
        performanceCheckAnimator.SetTrigger("Animate");
        yield return new WaitForSeconds(2f);
        //osciliate
        warningAnimate.SetTrigger("Osciliate");
        //spawn the arrow towards the object desired
        arrowNotificationParent.transform.position = tabObj.transform.position + new Vector3(0,-100);
        arrowNotification.transform.Rotate(0, 0, 90);

        //Turn on the bool for notification
        PerformanceNotification = true;

    }

    //This dictates whether the user is performing well or not
    public IEnumerator EvaluatePerformance()
    {

        double averageSuccessRate = (double)SuccessTicketsNum / TotalTicketsDone *100;
        //50% success ticket rate is not good
        if (averageSuccessRate < GameManager.SUCCESS_RATE)
        {
            print("You are performing terribly!");
            yield return StartCoroutine(GameManager.AnimateTexts(bossTextObj,string.Format(angryBossResponse, Math.Round(averageSuccessRate),
                GameManager.SUCCESS_RATE), 0.02f));
            //Give a strike!
            StrikeIncrement();
        }
        else
        {
            //50% and above success rate!!
            print("You are performing well!");
            StartCoroutine(GameManager.AnimateTexts(bossTextObj,string.Format(happyBossResponse, Math.Round(averageSuccessRate),
                GameManager.SUCCESS_RATE), 0.02f));
        }

        //Make the button appear when we intiate the boss so we can move on.
        proceedButton.SetActive(true);

    }

    //Reset the notification
    public void ResetBossNotification()
    {
        RectTransform rectTransform = (RectTransform)arrowNotificationParent.transform;
        //reset position of arrow
        rectTransform.anchoredPosition = new Vector3(-423, -20, 0);
        arrowNotification.transform.Rotate(0, 0, -90);

        //Toggle off the performance check 
        performanceCheck.SetActive(false);

        //Set the bool off for notification
        PerformanceNotification = false;
        //show the performance!!
        StartCoroutine(EvaluatePerformance());

    }



    //This is to advance to next day (separate from menu button, I may remove this?)
    public void DisplayProceedButton()
    {
        //Turn off the proceed button 
        proceedButton.SetActive(false);
        
    }

    public IEnumerator AnimateWarning()
    {
        yield return new WaitForSeconds(2f);
    }


    public void StrikeIncrement()
    {

        //Change icon and set the text on UI.
        Warnings += 1;
        warningText.text = ""+(Warnings+1);

        strikeIcons[Warnings].sprite = grayedOutStrike;

        //Animation potential? 
        warningsAnimator = strikeIcons[Warnings].GetComponent<Animator>();

        warningsAnimator.SetTrigger("FadeOut");

        //Make sure we do not exceed into out of bounds
        if (Warnings >= strikeIcons.Count - 1)
        {
            //YOU LOSE
            print("you lost");
            GameManager.instance.GameOver();
        }


    }

}
