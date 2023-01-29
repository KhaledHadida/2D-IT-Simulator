using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tickets : MonoBehaviour
{

    //MOVE TO USERS.CS
    //Number of prof pics we have for people
    public static int NUMBER_OF_MALE_PROFILE_PICS = 23;
    public static int NUMBER_OF_FEMALE_PROFILE_PICS = 17;

    //new employee = 1, employee left = 2... etc
    public enum TicketTypes { NEW_EMPLOYEE, EMPLOYEE_LEFT, EMPLOYEE_FIRED, EMPLOYEE_PROMOTED, EMPLOYEE_MISTAKE}

    //For record sake (I may remove this later), we will store the ticket objects, to match it with Ticket info.
    public List<GameObject> recordOfTickets;
    //accepted tickets (i.e will contain max number of tickets)
    public List<Ticket> ticketsAccepted;


    //MOVE TO USERS.CS
    [SerializeField]
    //This will have all profile pictures in the game. 
    public static List<Sprite> allMaleProfilePics = new List<Sprite>();
    //MOVE TO USERS.CS
    [SerializeField]
    //This will have all profile pictures in the game. 
    public static List<Sprite> allFemaleProfilePics = new List<Sprite>();

    //This is the MASTER prefab, we need it so we can create as many copies of TICKET objects.
    [SerializeField]
    protected GameObject masterTicket;

    //
    public Button ticketButton;
    public Ticket currentTicket;

    //This is parent gameobject canvas so we can place the buttons created above as children (for canvas purposes)
    public GameObject parentPanel;


    //Can probably put this into its own script (maybe MiddleUI.cs)
    //The ticket button generated on left hand side. 
    private Image profilePic;
    private TextMeshProUGUI ticketText;

    //This too can move to (MiddleUI.cs)
    //Middle side panel stuff (I may separate this to another class...)
    public Image middleSideProfilePic;
    public TextMeshProUGUI middleSideText;
    public TextMeshProUGUI timer;

    //Right side panel stuff
    public TimerBehaviour timerButton;
    public ManageButtons buttonManager;

    //Finish button for the ticket
    public GameObject finishButton;

    //Image blocking ticket
    public TicketBlocker blockImg;

    //SCript for upgrades
    public Upgrades upgradeManager;

    //Right hand side text
    public RightHandSide rightHandSide;

    //Buttons found under the tickets PANEL
    public GameObject acceptButton, declineButton;

    private void Awake()
    {

        //MOVE TO USERS.CS
        if (allFemaleProfilePics.Count == 0)
        {
            for (int i = 1; i <= NUMBER_OF_FEMALE_PROFILE_PICS; i++)
            {
                allFemaleProfilePics.Add(Resources.Load<Sprite>("Profile Pictures/Female/1 (" + i + ")"));
            }
        }
        //MOVE TO USERS.CS
        if (allMaleProfilePics.Count == 0)
        {
            for (int i = 1; i <= NUMBER_OF_MALE_PROFILE_PICS; i++)
            {
                allMaleProfilePics.Add(Resources.Load<Sprite>("Profile Pictures/Male/1 (" + i + ")"));
            }
        }
        if (rightHandSide == null)
        {
            rightHandSide = GameObject.Find("RightHandSide").GetComponent<RightHandSide>();
        }

        if (finishButton == null)
        {
            finishButton = GameObject.Find("CompleteTicket");
        }


        if (!middleSideProfilePic)
        {
            middleSideProfilePic = GameObject.Find("ProfilePicture").GetComponent<Image>();
        }
        if (!middleSideText)
        {
            middleSideText = GameObject.Find("TicketDescription").GetComponent<TextMeshProUGUI>();
        }
        if (!timer)
        {
            timer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        }
        if (!timerButton)
        {
            timerButton = GameObject.Find("Timer").GetComponent<TimerBehaviour>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

       
        upgradeManager = new Upgrades();
    }

    //This is called from Game.cs class which facilitates everything. 
    public IEnumerator SpawnTickets(string randomName, string jobName, string deptName, Sprite chosenProfilePic, int randomTimer)
    {
        //Keep it hidden till we're done instantiating TICKETS!!
        HideAcceptDeclineButtons(true);

        float time = 1f;

            //Create the prefab (just a plain good ol ticket gameobject)
            GameObject btn = GameObject.Instantiate(masterTicket, masterTicket.transform.position, Quaternion.identity);


            //get and set the profile picture of current user
            currentTicket = btn.GetComponent<Ticket>();

            //Ticket created
            CreateTicket( randomName,  jobName,  deptName,  chosenProfilePic, randomTimer);

            //Image
            profilePic = btn.transform.GetChild(0).gameObject.GetComponent<Image>();
            profilePic.sprite = currentTicket.profilePic;


            //Text of button, change it to the type of ticket (i.e Employee left, joined, etc)
            ticketText = btn.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            ticketText.text = currentTicket.type.ToString().Replace("_", " ");

            //btn.name = "Tickets:" + rand; 
            var rectTransform = btn.GetComponent<RectTransform>();
            rectTransform.SetParent(parentPanel.transform);
            recordOfTickets.Add(btn);

            //Grab the component for the button so we can add functionality to it later
            ticketButton = btn.GetComponent<Button>();
            //When a ticket is clicked, it is displayed on to the middle panel (left, middle and right)
            ticketButton.onClick.AddListener(delegate { displayTicket(btn); });

            yield return new WaitForSeconds(Mathf.Max(time, 0.25f));
            time /= 1.5f;
            
        HideAcceptDeclineButtons(false);

    }

    string ReturnName(int randomGender)
    {


        if(randomGender == 0)
        {
            int randomMaleUserNameIndex = Random.Range(0, DataManagement.maleUserNamesList.Length);
            //Retrieve profile pic according to Gender


            //Randomized from the textfiles
            return DataManagement.maleUserNamesList[randomMaleUserNameIndex];
        }
        else
        {
            int randomFemaleUserNameIndex = Random.Range(0, DataManagement.femaleUserNamesList.Length);
            //Retrieve profile pic according to Gender

            //Randomized from the textfiles
            return DataManagement.femaleUserNamesList[randomFemaleUserNameIndex];
        }

    }

    public Sprite ReturnProfilePicture(int randomGender)
    {
        if (randomGender == 0)
        {
            int randomProfilePic = Random.Range(0, allMaleProfilePics.Count);
            //Randomized from the textfiles
            return allMaleProfilePics[randomProfilePic];
        }
        else
        {
            int randomProfilePic = Random.Range(0, allFemaleProfilePics.Count);
            //Retrieve profile pic according to Gender

            //Randomized from the textfiles
            return allFemaleProfilePics[randomProfilePic];
        } 
    }


    //This is to create the ticket
    //The steps are as follows: we first need to 
    public void CreateTicket(string randomName, string jobName, string deptName, Sprite chosenProfilePic, int randomTimer)
    {
        //We generate a second person to get a new name basically (if the ticket is considered "mistake ticket")
        var generatedPerson2 = GenerateARandomEmployee();
        //Random values I have set off to generate as much randomness in my tickets
        int randomTicketDescIndex = Random.Range(0, DataManagement.ticketEntries.Length);

        //int randomJobAndDeptIndex = Random.Range(0, DataManagement.jobNamesList.Length);

        //10% chance of seeing an urgent ticket!
        int urgentGenerate = Random.Range(0, 10);

        //we want to make it flash! 
        if (urgentGenerate <= 1)
        {
            currentTicket.urgent = true;
            currentTicket.timer = 10;
        }
        else
        {
            currentTicket.urgent = false;
            currentTicket.timer = randomTimer;
        }

        //Retrieve the ticket's description
        string chosenTicketDescription = DataManagement.ticketEntries[randomTicketDescIndex];

        //Create the USER. 
        //TicketTypes randomlyChosen = retrieveTicketType(chosenTicketDescription.Substring(0, 3), randomName, jobName, deptName, chosenProfilePic);
        TicketTypes randomlyChosen = retrieveTicketType(chosenTicketDescription.Substring(0, 3));

        string description = chosenTicketDescription
            .Replace("\\n", "\n")
            .Replace("{X}", randomName)
            .Replace("{Y}", jobName)
            .Replace("{Z}", deptName)
            .Replace("{U}", generatedPerson2.Item1.Split(" ")[0]);



        //This is for the issuer of ticket (I may remove.. it has no use)
        currentTicket.ticketerName = chosenProfilePic.ToString();
        //Third part is the description of the ticket
        currentTicket.ticketDesc = description.Substring(3);
        //This is for the ticket issuer profile picture (can be any random pic)
        currentTicket.profilePic = chosenProfilePic;
        currentTicket.type = randomlyChosen;



        //Exception Case(s)
        //If the ticket contains {U} which basically is universal for a mistake in the name...
        if (chosenTicketDescription.Contains("{U}"))
        {
            //This is a template to compare against ticket's objective and the player's actions (i.e did player do the ticket properly?)
            string correctName = generatedPerson2.Item1.Split(" ")[0] + " " + randomName.Split(" ")[1];
            currentTicket.template = new UserTemplate(correctName, jobName, deptName, randomlyChosen);
        }
        else
        {
            //I probably should refine this type of code, I dont like it being spaghetti
            if(currentTicket.type == TicketTypes.EMPLOYEE_PROMOTED)
            {
                //If the person is already manager then append "senior" in beginning
                if (jobName.Contains("Manager"))
                {
                    currentTicket.template = new UserTemplate(randomName, "Senior "+ jobName, deptName, randomlyChosen);
                }
                //otherwise put "manager" at end.
                else
                {
                    currentTicket.template = new UserTemplate(randomName, jobName + " Manager", deptName, randomlyChosen);
                }
                
            }
            else
            {
            //This is a template to compare against ticket's objective and the player's actions (i.e did player do the ticket properly?)
            currentTicket.template = new UserTemplate(randomName, jobName, deptName, randomlyChosen);
            }

        }


        //This is a template to compare against ticket's objective and the player's actions (i.e did player do the ticket properly?)
        //currentTicket.template = new UserTemplate(randomName, jobName, deptName, randomlyChosen);
    }

    //MOVE TO USERS.CS
    //I could have divided this into separate each functions but I think a tuple does job nicely
    public static System.Tuple<string, string, string, Sprite> GenerateARandomEmployee()
    {
        //variables for returning the tuple (Uninitialized)
        string name;
        string jobName;
        string deptName;
        Sprite profilePic;

        //Random chance between MALE OR FEMALE (0 or 1)
        int randomGender = Random.Range(0, 1);

        //For gender, we first will get a name according to gender AND a sprite according to gender
        if (randomGender == 0)
        {
            int randomMaleUserNameIndex = Random.Range(0, DataManagement.maleUserNamesList.Length-1);
            name = DataManagement.maleUserNamesList[randomMaleUserNameIndex];
            //Retrieve profile pic according to Gender
            int randomProfilePic = Random.Range(0, Tickets.allMaleProfilePics.Count-1);
            profilePic = allMaleProfilePics[randomProfilePic];
            //print("Sprite for Male " + profilePic + " Number "+ randomProfilePic);
        }
        else
        {
            int randomFemaleUserNameIndex = Random.Range(0, DataManagement.femaleUserNamesList.Length-1);
            name = DataManagement.femaleUserNamesList[randomFemaleUserNameIndex];
            //Retrieve profile pic according to Gender
            int randomProfilePic = Random.Range(0, Tickets.allFemaleProfilePics.Count-1);
            profilePic= allFemaleProfilePics[randomProfilePic];
            //print("Sprite for Female " + profilePic + " Number " + randomProfilePic);
        }


        //Safe guard if there is a missing sprite just put first one:
        if(!profilePic)
        {
            profilePic = allMaleProfilePics[0];
        }

        //Random values I have set off to generate as much randomness in my tickets
        int randomJobAndDeptIndex = Random.Range(0, DataManagement.jobNamesList.Length);
        //Randomized job and department fetched from text (1) is  jobname, (0) is department
        string[] randomJobAndDept = DataManagement.jobNamesList[randomJobAndDeptIndex].Split("&");

        //Set job name
        jobName = randomJobAndDept[1];
        deptName = randomJobAndDept[0];

        return System.Tuple.Create(name,jobName,deptName,profilePic);
    }


    //Gives us back the ticket type assigned based on the ticket text (i.e if {1} in beginning of ticket then it is a new employee
    TicketTypes retrieveTicketType(string ticketNum)
    {
        //In case we get nothing we will return new employee
        TicketTypes results = TicketTypes.NEW_EMPLOYEE;

        switch (ticketNum)
        {
            case "{1}":
                results = TicketTypes.NEW_EMPLOYEE;
                break;
            case "{2}":
                results = TicketTypes.EMPLOYEE_LEFT;
                //Create a ticket in the database.
                break;
            case "{3}":
                results = TicketTypes.EMPLOYEE_FIRED;
                break;
            case "{4}":
                results = TicketTypes.EMPLOYEE_PROMOTED;
                break;
            case "{5}":
                results = TicketTypes.EMPLOYEE_MISTAKE;
                break;

        }
        return results;
    }

    public void displayTicket(GameObject b)
    {
        SoundManage.PlaySound(SoundManage.Sound.SelectTicket);

        //set current ticket to be this
        //currentButton = b.GetComponent<Button>();
        currentTicket = b.GetComponent<Ticket>();

       //Display it in middle.
        middleSideProfilePic.sprite = b.GetComponent<Ticket>().profilePic;
        middleSideText.text = b.GetComponent<Ticket>().ticketDesc;
        timer.text = ""+b.GetComponent<Ticket>().timer +" Sec";

        //reveal ticket.
        if (currentTicket.accepted)
        {
            blockImg.Hide();
            //Finish button after animation is done
            StartCoroutine(ToggleFinishButton(1f));
        }
        else
        {
            blockImg.Show();
            //INSTANTLY remove finish button
            StartCoroutine(ToggleFinishButton(0f));
        }

        //Show finish button


    }

    //We want to delay displaying finish button because when we start the game we want user to accept ticket after animation of ticket moving from unaccepted to accepted
    //Another way to do this is to integrate it into the animations as an event (ticket animation -> Spawn Animation)
    public IEnumerator ToggleFinishButton(float time)
    {
        yield return new WaitForSeconds(time);


        if (currentTicket.accepted)
        {
            finishButton.SetActive(true);
        }
        else
        {
            finishButton.SetActive(false);
        }
    }

    //Move tickets when pressing accept 
    public void moveTicket()
    {
        //If we have not accepted the ticket (so we dont bug with time)
        if (!currentTicket.accepted && ticketsAccepted.Count < upgradeManager.NumberOfTickets && currentTicket != null)
        {
            //currentTicket.RemoveTicketAnim();
            //Animation perhaps here? 

            StartCoroutine(TransferTicketAnim());
            //accept the ticket
            currentTicket.accepted = true;

            //Add timer.. to the timer
            timerButton.CreateATimer(currentTicket.timer);


            //add it to the pile of tickets we accepted!
            ticketsAccepted.Add(currentTicket);

            displayTicket(currentTicket.gameObject);
            //currentTicket.SpawnTicketAnim();
        }
        //flash the # of max tickets b/c we can't accept more!!!
        else
        {
            rightHandSide.Flash();
        }

    }

    //This will transition ticket from unaccepted to accepted.
    private IEnumerator TransferTicketAnim()
    {

        currentTicket.RemoveTicketAnim();



        yield return new WaitForSeconds(0.5f);
        var rectTransform = currentTicket.GetComponent<RectTransform>();
        rectTransform.SetParent(rightHandSide.rightSidePanel.transform);

        currentTicket.SpawnTicketAnim();
    }


    //Hide the accept/decline button so that the user DOES NOT interfer with them when tickets are just spawning in
    public void HideAcceptDeclineButtons(bool param)
    {
        if (param)
        {
            acceptButton.SetActive(false);
            declineButton.SetActive(false);
        }
        else
        {
            acceptButton.SetActive(true);
            declineButton.SetActive(true);
        }
    }

    //This will display next ticket if it exists in queue (unaccepted)
    public void GoToNextTicket()
    {
        //Return Clause to make sure we dont get errors later
        if (currentTicket == null) return;
        //get index of the current item
        int index = recordOfTickets.IndexOf(currentTicket.gameObject);


        if (recordOfTickets.Count == 0)
        {
            middleSideText.text = "No ticket is selected!\n\n\n\n\n\n\n\n";
            middleSideProfilePic.sprite = null;
            return;
        }
            

        //we dont exceed list
        if(index < recordOfTickets.Count-1)
        {
            displayTicket(recordOfTickets[index + 1]);
        }
        else
        {
            //get last item
            displayTicket(recordOfTickets[recordOfTickets.Count-1]);
        }
    }

}

//MOVE TO USERS.CS
public class UserTemplate{
    private string name;
    private string jobName;
    private string deptName;
    private Tickets.TicketTypes type;
    public UserTemplate(string Name, string JobName, string DeptName, Tickets.TicketTypes Type)
{
        name = Name;
        jobName = JobName;
        deptName = DeptName;
        type = Type;

}

    public string getName()
    {
        return name;
    }

    public string getJobName()
    {
        return jobName;
    }

    public string getDeptName()
    {
        return deptName;
    }

    public Tickets.TicketTypes getType()
    {
        return type;
    }

}


