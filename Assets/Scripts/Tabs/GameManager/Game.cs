using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//THIS SCRIPT WILL FACILITATE THE GAME ORDER. 
public class Game : MonoBehaviour
{
    //Tickets component (first tab tickets) deals with creating tickets
    public Tickets ticketManager;
    //usersManager deals with everything inside of users, we have functionaltiy such as create users, delete users, modify users
    public ManageButtons usersManager;
    public DaysManager daysManager;
    public QuestChecker questCheckerManager;

    private int ticketsSpawned = 3;




    // Start is called before the first frame update
    void Start()
    {
        //Tutorial here?
        GameObject d = new GameObject();
        //Add the script
        d.AddComponent<TutorialManager>();
        //Add the tutorial location
        d.GetComponent<TutorialManager>().commands.Add(
            new Command("Click here!", new Vector2(4,4), new Vector2(2, 2), Command.DIRECTION.NORTH));
        d.GetComponent<TutorialManager>().commands.Add(
            new Command("Click here!", new Vector2(4, 4), new Vector2(2, 2), Command.DIRECTION.NORTH));


        //Order of execution:
        //First mute the sounds so we don't hear it when we press play
        SoundManager.muted = true;
        //First we call CreateTicket from Tickets.cs, this creates us a ticket and gives us info 
        ///////////////////// TICKET PART /////////////////////
        ticketManager = GetComponent<Tickets>();
        daysManager = GetComponent<DaysManager>();
        questCheckerManager = GetComponent<QuestChecker>();

        if (usersManager == null)
        {
            usersManager = GameObject.Find("UsersViewPanel").GetComponent<ManageButtons>();
        }

        StartTheDay();
        //Delay mute by 1 second
        StartCoroutine(ExecuteAfterTime());

    }

    public IEnumerator ExecuteAfterTime()
    {
        yield return new WaitForSeconds(1f);
        //Unmute
        SoundManager.muted = false;
    }

    public void StartTheDay()
    {
        ///////////////////// USERS PART /////////////////////
        for (int i = 0; i < ticketsSpawned*daysManager.CurrentDay; i++)
        {

            //Make a random person so we can convert it to user
            var generatedPerson = Tickets.GenerateARandomEmployee();
            string randomName = generatedPerson.Item1;
            string jobName = generatedPerson.Item2;
            string deptName = generatedPerson.Item3;
            Sprite chosenProfilePic = generatedPerson.Item4;
            //For regular tickets (this is going to be dependent on days?)
            int randomTimer = Random.Range(25, 60);

            //Spawn X tickets
            StartCoroutine(ticketManager.SpawnTickets(randomName, jobName, deptName, chosenProfilePic, randomTimer));

            //Current ticket
            Ticket currentTicket = ticketManager.recordOfTickets[i].GetComponent<Ticket>();


            //Create "artificial tickets" until the player deals with them accordingly (i.e remove, modify..)
            if (currentTicket.type == Tickets.TicketTypes.EMPLOYEE_FIRED || currentTicket.type == Tickets.TicketTypes.EMPLOYEE_LEFT
                || currentTicket.type == Tickets.TicketTypes.EMPLOYEE_MISTAKE || currentTicket.type == Tickets.TicketTypes.EMPLOYEE_PROMOTED)
            {
                //Create a user so the player has to deal with it
                usersManager.CreateUser(randomName, jobName, deptName, chosenProfilePic);
            }


        }
        //Only for first ticket we do this 
        ticketManager.displayTicket(ticketManager.recordOfTickets[0]);
        
    }




    public int TicketsSpawned
    {
        get => ticketsSpawned;
        set => ticketsSpawned = value;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    //Load scenes from here
    public static void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
