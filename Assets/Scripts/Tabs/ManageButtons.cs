using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Manage buttons will take care of the 'Users' tab 
public class ManageButtons : MonoBehaviour
{
    public enum ButtonMode { CREATE, EDIT }
    public static ButtonMode currentButtonMode;

    //This is the MASTER prefab, we need it so we can create as many copies of USER objects.
    public GameObject masterUser;
    //This is parent gameobject canvas so we can place the buttons created above as children (for canvas purposes)
    public GameObject scrollContent;


    public List<GameObject> sortedList = new List<GameObject>();
    public List<User> users = new List<User>();
    public List<GameObject> userObjects = new List<GameObject>();
    //public GameObject btn;


    //For the search bar
    public TMP_InputField searchQuery;

    //This is right side when an employee is pressed on, we select it!
    public Button currentlySelectedEmployee;

    //Title text is the "Create a user" or "Modify a user", I wanted to reuse this panel to reduce gameobjects.
    public TextMeshProUGUI titleText;

    private Image profilePic;
    private TextMeshProUGUI name;
    private TextMeshProUGUI jobName;
    private TextMeshProUGUI deptName;

    public Users usersManager;


    // Start is called before the first frame update
    void Start()
    {
        //acceptButton = GameObject.Find("AcceptButton");
        //declineButton = GameObject.Find("DeclineButton");


        searchQuery = GameObject.Find("UserSearchBar").GetComponent<TMP_InputField>();

        //Check if gameobject button is null, 
        if (masterUser == null)
        {
            print("Button doesn't exist for reference");
        }

        if (!titleText)
        {
            titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        }



        if(!name || !jobName || !deptName)
        {
            name = GameObject.Find("UserName").GetComponent<TextMeshProUGUI>();
            jobName = GameObject.Find("UserJob").GetComponent<TextMeshProUGUI>();
            deptName = GameObject.Find("UserDepartment").GetComponent<TextMeshProUGUI>();
            profilePic = GameObject.Find("UserIcon").GetComponent<Image>();
        }

        loadUsers();

        //TEMPORARY? Put it somewhere else
        //Cursor.lockState = CursorLockMode.Confined;


    }

    public void loadUsers()
    {
        //Create series of button
        for (int i = 0; i < 6; i++)
        {

            var generatePerson = Tickets.GenerateARandomEmployee();
            List<User> checkIfNameExists = UserExist(false, generatePerson.Item1);
            if (checkIfNameExists.Count < 1)
            {
                CreateUser(generatePerson.Item1, generatePerson.Item2, generatePerson.Item3, generatePerson.Item4);
            }

            //Display for firs time.. 
            if(i == 0)
            {
                DisplayUser(userObjects[0]);
            }
            


        }
    }

    public void CreateUser(string Name, string JobName, string DeptName, Sprite ProfilePic)
    {
        //create button
        GameObject btn = GameObject.Instantiate(masterUser, masterUser.transform.position, Quaternion.identity);
        //Pass these on to the class per gameobj instiantiated. 
        btn.GetComponent<User>().setName(Name);
        btn.GetComponent<User>().setJobName(JobName);
        btn.GetComponent<User>().setDeptName(DeptName);
        btn.GetComponent<User>().setProfilePic(ProfilePic);

        //Name the user
        btn.name = "User: " + Name;

        //Grab the component for the button so we can add functionality to it later
        currentlySelectedEmployee = btn.GetComponent<Button>();
        //When a ticket is clicked, it is displayed on to the middle panel (left, middle and right)
        currentlySelectedEmployee.onClick.AddListener(delegate { DisplayUser(btn); });

        //Get parent of the two buttons (EDIT AND DELETE) 
        GameObject parentButton = FindChildWithTag(currentlySelectedEmployee.gameObject, "ButtonParent");

        //get the children buttons
        Button editButton = FindChildWithTag(parentButton, "EditUser").GetComponent<Button>();
        Button deleteButton = FindChildWithTag(parentButton, "DeleteUser").GetComponent<Button>();

        //Delegate button functionality (Both EDIT and DELETE)
        editButton.onClick.AddListener(delegate { usersManager.ToggleEditUser(btn.gameObject); });
        deleteButton.onClick.AddListener(delegate { DeleteUser(btn.gameObject); });

        var rectTransform = btn.GetComponent<RectTransform>();
        rectTransform.SetParent(scrollContent.transform);

        //Store the gameobject as well
        //This might be useless now?
        userObjects.Add(btn);

        //userObjects = userObjects.OrderBy(x => x.name).ToList();

        SortUser();

    }


    public void SortUser()
    {
        GameObject[] count = GameObject.FindGameObjectsWithTag("User");
        GameObject[] countOrdered = count.OrderBy(x => x.name).ToArray();

        for (int i = 0; i < countOrdered.Length; i++)
        {
            countOrdered[i].transform.SetSiblingIndex(i);

        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SearchUsers();
        }
    }


    //Search users for Username, Jobtitle, departmentTitle 
    public void SearchUsers()
    {
        string searchText = searchQuery.text;


        List<User> results = UserExist(false, searchText);

        //if there is something written in text search.
        if(searchText.Length > 0)
        {
            //Make sure we disable every button
            foreach (GameObject user in userObjects)
            {
                user.SetActive(false);

            }
        }
        else
        {
            //if search field is empty then RETURN ALL USERS
            foreach (GameObject user in userObjects)
            {
                user.SetActive(true);

            }
        }


        //If we get results based on search, enable them.
        foreach (var result in results)
        {
            //If we get results back 
            if (result != null && searchText.Length > 0)
            {
                result.gameObject.SetActive(true);


            }
        }



    }


    //returns a user if they exist. If not return false
    //I've split the functions so I can use this when tracking if player has achieved the quest
    public List<User> UserExist(bool inclusiveSearch, params string[] fields)
    {
        //new array
        List<User> results = new List<User>();

        //We go through user database
        foreach (GameObject user in userObjects)
        {
            User currentUser = user.GetComponent<User>();
            //If its not an inclusive search (so its || )
            if (!inclusiveSearch && fields.Length == 1)
            {
                if (currentUser.deptName.ToLower().Contains(fields[0].ToLower())
                            || currentUser.name.ToLower().Contains(fields[0].ToLower())
                            || currentUser.jobName.ToLower().Contains(fields[0].ToLower()))
                {
                    //we found it
                    results.Add(currentUser);
                    //return results;
                }
            }
            else
            {
                if (currentUser.name.ToLower().Contains(fields[0].ToLower())
                            && currentUser.jobName.ToLower().Contains(fields[1].ToLower())
                            && currentUser.deptName.ToLower().Contains(fields[2].ToLower()))
                {
                    //we found it
                    results.Add(currentUser);
                    //return currentUser;
                }
            }
            
        }

        return results;
    }

    //it will DELETE the currently selected user... 
    public void DeleteUser(GameObject currentObj)
    {
        User currentUser = currentObj.GetComponent<User>();
        //NEW
        userObjects.Remove(currentObj);

        //DO NOT PLAY THIS ANIMATION WHEN EDITING!!
        if(ManageButtons.currentButtonMode != ManageButtons.ButtonMode.EDIT)
        {
            currentUser.DeleteAnimation();
        }
        
        
        Animator clipLength = currentUser.GetUserAnimator();

        float animationLength = GameManager.AnimClipTimes(clipLength, "Ticket_Delete");

        Destroy(currentObj, animationLength);
        SoundManage.PlaySound(SoundManage.Sound.DeleteUserPop);

        //Check if the person has gone rogue by deleting the entire database
        if(getNumberOfUsers() <= 0)
        {
            //Display to user why they got fired!

            //Finish the game
            GameManager.instance.GameOver();
        }

    }


    //Whenever we select the user display the properties on the right.
    private void DisplayUser(GameObject user)
    {
        SoundManage.PlaySound(SoundManage.Sound.SelectTicket);
        //Get current button we pressed.
        currentlySelectedEmployee = user.GetComponent<Button>();
        //Display values first
        name.text = user.GetComponent<User>().name;
        jobName.text = user.GetComponent<User>().jobName;
        deptName.text = user.GetComponent<User>().deptName;

        //display profile pic
        profilePic.sprite = user.GetComponent<User>().profileSprite;


    }

    //This is handy to finding objects in child (we dont want to traverse through 50+ objs)
    public GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;

        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
        return child;
    }


    public static Sprite RetrieveSprite(string imgName)
    {
        Sprite img = Resources.Load<Sprite>("Profile Pictures/Male/" + imgName);
        return img;
    }

    //Return # of users so far
    public int getNumberOfUsers()
    {
        return userObjects.Count;
    }








}



