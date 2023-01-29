using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private UnityEvent onTimerEnd = null;

    //STATIC VAR FOR DAYS
    public static int DAYS_NUM = 1;
    public Animator animator;

    //increment days as we finish quests.
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI timerText;

    private QuestChecker quests;

    private Timer timer;


    // Start is called before the first frame update
    private void Start()
    {
        CreateATimer(0);
        quests = GameObject.Find("_manager").GetComponent<QuestChecker>();
        daysText = GameObject.Find("Days").GetComponent<TextMeshProUGUI>();
        timerText = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void CreateATimer(int time)
    {

        if(timer == null)
        {
            timer = new Timer(time);
        }
        else
        {
            //If  we have timer then we add more time
            timer.AddTime(time);
        }

        timer.OnTimerEnd += HandleTimerEnd;
    }

    private void HandleTimerEnd()
    {
        onTimerEnd.Invoke();

        //Destroy(this);
    }

    // Update is called once per frame
    private void Update()
    {
        
        timer.Tick(Time.deltaTime);
        
        
    }

    public int GetTimer()
    {
        return (int)timer.RemainingSeconds;
    }

    public void EndTimer()
    {
        
        print("Time here?");

        //do we have more than 1 ticket in queue?
        if (quests.IsListBiggerThanNum(1))
        {
            //Do nothing!
        }
        else
        {
            //If we dont have any tickets in queue
            print("new timer");
            //Set timer to ZERO
            timer = new Timer(0);

        }


    }

    private void LateUpdate()
    {
        //Change timertext colour to red if its 10 seconds on clock. (maybe make it flash)
        if(timer.RemainingSeconds <= 10 && timer.RemainingSeconds > 0)
        {
            //Trigger flash
            animator.SetTrigger("Flash");
        }
        else
        {
            //timerText.color = Color.white;
            animator.Rebind();
        }


        //No sense in updating time if there is no ticket in queue
        if (quests.IsListBiggerThanNum(0))
        {
            timerText.text = "Timer: " + Mathf.Round(timer.RemainingSeconds);
        }
        else
        {
            timerText.text = "Timer: -";
        }
        

    }
}
