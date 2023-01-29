using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    public PerformanceTab performanceTabManager;

    public Game gameManager;

    //animation stuff (these are prefabs from anim class)
    public GameObject successfulNotification, failedNotification;

    public GameObject canvasPanelParent;
    public TimerBehaviour timerButton;

    private int amountOfTickets;

    public Ticket currentTicket;

    private Animator currentTicketAnim;
    private float clipLength;

    // Start is called before the first frame update
    void Start()
    {
        if (!performanceTabManager)
        {
            performanceTabManager = GameObject.Find("PerformanceButton").GetComponent<PerformanceTab>();
        }

        if (!canvasPanelParent)
        {
            canvasPanelParent = GameObject.Find("Canvas");
        }
        if (!timerButton)
        {
            timerButton = GameObject.Find("TicketPanel").GetComponent<TimerBehaviour>();
        }
        if (!gameManager)
        {
            gameManager = GetComponent<Game>();
        }


        //currentTicket = gameManager.ticketManager.currentTicket;

    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft = timerButton.GetTimer();

        //This may be a bottleneck.. (If so, make the tickets.cs class have access to this variable so we can always have a fresh ticket)
        if (gameManager.ticketManager.currentTicket != null && timeLeft <= 0 && gameManager.ticketManager.currentTicket.accepted)
        {
           
        }

        if(timeLeft == 0 && IsListBiggerThanNum(0) && gameManager.ticketManager.currentTicket.accepted)
        {
           // FinishQuest();
        }
    }

    public static bool CheckQuestCompletion()
    {

        return true;

    }

    //All buttons end up here (decline and finish) 
    public void FinishQuest()
    {
        currentTicket = gameManager.ticketManager.currentTicket;

        

        if (currentTicket.accepted)
        {
            try
            {

                //Get the current ticket so we can validate if it exists in the database.
                string name = currentTicket.template.getName();
                string job = currentTicket.template.getJobName();
                string dept = currentTicket.template.getDeptName();
                Tickets.TicketTypes typeofTicket = currentTicket.template.getType();

                //Check if user exists (TRUE means he is in the database, false means he is not)
                List<User> userExists = gameManager.usersManager.UserExist(true, name, job, dept);


                switch (typeofTicket)
                {
                    case Tickets.TicketTypes.NEW_EMPLOYEE:
                        if (userExists.Count > 0)
                        {
                            print("Good job! User exists");
                            SuccessfulTicket();
                        }
                        else
                        {
                            FailedTicket();
                            print("failed");
                        }
                        break;
                    case Tickets.TicketTypes.EMPLOYEE_FIRED:
                        if (userExists.Count < 1)
                        {
                            SuccessfulTicket();
                            print("Good job! User does not exist");
                        }
                        else
                        {
                            FailedTicket();
                            print("failed");
                        }
                        break;
                    case Tickets.TicketTypes.EMPLOYEE_MISTAKE:
                        if (userExists.Count > 0)
                        {
                            print("Good job! User exists");
                            SuccessfulTicket();
                        }
                        else
                        {
                            FailedTicket();
                            print("failed");
                        }
                        break;
                    case Tickets.TicketTypes.EMPLOYEE_PROMOTED:
                        if (userExists.Count > 0)
                        {
                            print("Good job! User exists");
                            SuccessfulTicket();
                        }
                        else
                        {
                            FailedTicket();
                            print("failed");
                        }
                        break;
                    case Tickets.TicketTypes.EMPLOYEE_LEFT:
                        if (userExists.Count < 1)
                        {
                            SuccessfulTicket();
                            print("Good job! User does not exist");
                        }
                        else
                        {
                            FailedTicket();
                            print("failed");
                        }
                        break;
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            currentTicketAnim = currentTicket.GetComponent<Animator>();
            clipLength = GameManager.AnimClipTimes(currentTicketAnim, "Remove_anim");

            amountOfTickets = gameManager.ticketManager.ticketsAccepted.Count;

            //This was moved from "DeleteTicket" to here because it was causing bugs when declining an unaccepted ticket.
            gameManager.ticketManager.timerButton.EndTimer();


            //get rid of ticket once it is done.
            DestroyTicket(currentTicket, clipLength);

            



        }

 


    }

    public void DeclineTicket()
    {

        //We need to reference this everytime we hit the decline button.
         currentTicket = gameManager.ticketManager.currentTicket;
       
        
        if (currentTicket != null)
        {

            FailedTicket();
            DestroyTicket(currentTicket, clipLength);
        }


    }

    public void DestroyTicket(Ticket currentTicket, float time){

        //Animation of removing ticket
        currentTicket.RemoveTicketAnim();

        //Destroy it 
        Destroy(currentTicket.gameObject, time);


        //we remove accepted so we can toggle off the finish button.
        currentTicket.accepted = false;
        //Remove finish button
        StartCoroutine(gameManager.ticketManager.ToggleFinishButton(0f));

        //Remove the ticket from the record of tickets
        gameManager.ticketManager.recordOfTickets.Remove(currentTicket.gameObject);
        //eliminate the ticket from the queue
        gameManager.ticketManager.ticketsAccepted.Remove(currentTicket);

        int ticketsLeftInQueue = gameManager.ticketManager.recordOfTickets.Count;
        //Check if we're done all the tickets
        if (ticketsLeftInQueue == 0)
        {
            GameManager.instance.EndTheDay();
        }
    }




    public void SuccessfulTicket()
    {
        //summon the gameobject with the animation, then 
        GameObject success = GameObject.Instantiate(successfulNotification, canvasPanelParent.transform.position, Quaternion.identity, canvasPanelParent.transform);

        float duration = success.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        //Destroy anim so it does not linger
        Destroy(success, duration);


        performanceTabManager.SuccessTicketsNum += 1;
        performanceTabManager.TotalTicketsDone += 1;

        //calculate time lapsed for ticket
        int timesoFar = currentTicket.timer;
        int timeLeft = gameManager.ticketManager.timerButton.GetTimer();

        performanceTabManager.AverageTicketTime = timesoFar - timeLeft;
        performanceTabManager.UpdateFields();
    }

    public void FailedTicket()
    {
        //summon the gameobject with the animation, then 
        GameObject failed = GameObject.Instantiate(failedNotification, canvasPanelParent.transform.position, Quaternion.identity, canvasPanelParent.transform);

        float duration = failed.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        //Destroy anim so it does not linger
        Destroy(failed, duration);

        performanceTabManager.FailedTicketsNum += 1;
        performanceTabManager.TotalTicketsDone += 1;
        performanceTabManager.UpdateFields();
    }

    public bool IsListBiggerThanNum(int num)
    {
        return gameManager.ticketManager.ticketsAccepted.Count > num;
    }
}
