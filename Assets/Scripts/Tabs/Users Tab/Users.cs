using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Users : MonoBehaviour
{
    public GameObject nameInput, jobInput, departmentInput, profilePicInput;
    private string name, jobTitle, departmentTitle;
    private Sprite profilePic;
    //These are inputfields for creating a user/updating a user.
    public TMP_InputField nameField, jobField, departmentField;

    public Button okButton;

    //This input is strictly for the "Create User" panel when tabbing. 
    private int inputSelected;

    public GameObject createPanel;

    //Title text is the "Create a user" or "Modify a user", I wanted to reuse this panel to reduce gameobjects.
    public TextMeshProUGUI titleText; 


    //Reference Button Manager to creating the user entries 
    public ManageButtons btnManager;





    // Start is called before the first frame update
    void Start()
    {
        if(!nameField || !jobField|| !departmentField || !profilePicInput)
        {
            nameField = GameObject.Find("NameField").GetComponent<TMP_InputField>();
            jobField = GameObject.Find("JobField").GetComponent<TMP_InputField>();
            departmentField = GameObject.Find("DepartmentField").GetComponent<TMP_InputField>();
        }
       

        if (!createPanel)
        {
            createPanel = GameObject.Find("CreateUserPanel");
        }

        if (!btnManager)
        {
           
            btnManager = GameObject.Find("UsersViewPanel").GetComponent<ManageButtons>();
        }

        if (!titleText)
        {
            titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        }
        if (!profilePicInput)
        {
            profilePicInput = GameObject.Find("ProfilePictureSelector");
        }
        createPanel.SetActive(false);
    }

    //Not sure where to place this but we need to check if TAB is pressed.
    // Update is called once per frame
    void Update()
    {
        //SWITCH BETWEEN TEXT FIELDS
        if(createPanel.activeSelf && Input.GetKeyDown(KeyCode.Tab))
        {
            inputSelected++;
            if (inputSelected > 2) inputSelected = 0;
            SelectInputField();
        }

        //PRESS OK
        if(createPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
           
            GetInputFromFields();

        }
    }
    //For when tabbing across the diferent fields of making a user
    public void SelectInputField()
    {
        switch (inputSelected)
        {
            case 0: nameField.Select();
                break;
            case 1: jobField.Select();
                break;
            case 2: departmentField.Select();
                break;
        }
    }

    public void NameFieldSelected() => inputSelected = 0;
    public void JobFieldSelected() => inputSelected = 1;
    public void DepartmentFieldSelected() => inputSelected = 2;



    public void GetInputFromFields()
    {


        name = nameInput.GetComponent<TextMeshProUGUI>().text;
        jobTitle = jobInput.GetComponent<TextMeshProUGUI>().text;
        departmentTitle = departmentInput.GetComponent<TextMeshProUGUI>().text;
        profilePic = profilePicInput.GetComponent<Image>().sprite;


        //If its Edit
        if (ManageButtons.currentButtonMode == ManageButtons.ButtonMode.EDIT)
        {
            //First delete old entry
            btnManager.DeleteUser(btnManager.currentlySelectedEmployee.gameObject);


        }

        //iF its Create

        //For some reason the input text has length of 1 by default.
        if(name.Length > 1 && jobTitle.Length >1 && departmentTitle.Length>1)
        {
           
            int rand = Random.Range(0, 5);
            string img = "1 " + "(" + rand + ")";
            print(rand);
            //activate button.
            btnManager.CreateUser(name, jobTitle, departmentTitle, profilePic);


        }


        ClearFields();
        createPanel.SetActive(false);

    }

    //Clears the User Panel Fields (i.e Name, job, dept)
    public void ClearFields()
    {
        //Clear the entries 
        nameField.text = "";
        jobField.text = "";
        departmentField.text = "";

    }


    //EDIT MODE - This toggles the edit panel, 
    public void ToggleEditUser(GameObject obj)
    {
        //In case we try to edit a deleted button, which we can't of course.
        try
        {
            ClearFields();
            ChangeFields(obj);
            createPanel.SetActive(true);
        }
        catch (System.Exception)
        {
            print("Button is Null!");
            //throw new System.Exception();
        }



        ManageButtons.currentButtonMode = ManageButtons.ButtonMode.EDIT;
        print(ManageButtons.currentButtonMode);
    }


    public void ChangeFields(GameObject obj)
    {
        //EDIT USER
        TMP_InputField editName = nameField.GetComponent<TMP_InputField>();
        TMP_InputField editJob = jobField.GetComponent<TMP_InputField>();
        TMP_InputField editDeptName = departmentField.GetComponent<TMP_InputField>();

        titleText.text = "EDIT A USER";

        string currentName = obj.GetComponent<User>().name;
        string currentJob = obj.GetComponent<User>().jobName;
        string currentDeptName = obj.GetComponent<User>().deptName;
        Sprite currentSprite = obj.GetComponent<User>().profileSprite;

        //store these into the text
        editName.text = currentName;
        editJob.text = currentJob;
        editDeptName.text = currentDeptName;
        profilePicInput.GetComponent<Image>().sprite = currentSprite;

    }

    //Toggle on and off for creating users panel.
    public void ToggleCreatePanel()
    {

        //For the CREATE user only
        titleText.text = "CREATE A USER";
        ManageButtons.currentButtonMode = ManageButtons.ButtonMode.CREATE;
        //CLEAR entries because when switching from edit mode to create mode, we still carry over the fields..
        ClearFields();

        createPanel.SetActive(!createPanel.active);
    }


    
}
