using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    //Controlled by Users.cs 
    public Sprite profileSprite;
    public string name;
    public string jobName;
    public string deptName;

    //These are the gameobjects for the variables above 
    public Image profilePic;
    private TextMeshProUGUI nameTextMesh;
    private TextMeshProUGUI jobNameTextMesh;
    private TextMeshProUGUI deptNameTextMesh;

    //Animation
    private Animator animator;
    [SerializeField]
    private ParticleSystem particles;

    //Buttons 
    [SerializeField]
    private Button editButton, deleteButton;

    // Start is called before the first frame update
    void Start()
    {

        // 0 - Profilepic 1 - Name 2 - JobName 3 - Department
        //Reference the variables in the instiantiated button.
        profilePic = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        nameTextMesh = this.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        jobNameTextMesh = this.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        deptNameTextMesh = this.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        animator = GetComponent<Animator>();

        ChangeUserFields(name, jobName, deptName, profileSprite);

        //Add onclick event to the EDIT AND DELETE BUTTONS.


    }

    public void ChangeUserFields(string nameChange, string jobChange, string deptChange, Sprite imgChange)
    {
        //Initialize these variables so we can access them outside of class.
        nameTextMesh.text = name;
        jobNameTextMesh.text = jobName;
        deptNameTextMesh.text = deptName;

        //If no pic, load default
        if (!profilePic)
        {
            print("Error, no pic found");
            //Load a default profile pic if non exist
            profilePic.sprite = Resources.Load<Sprite>("Profile Pictures/1 (1)");
        }
        else
        {
            profilePic.sprite = profileSprite;
        }
    }

    public void setName(string Name)
    {
        name = Name;
    }

    public void setJobName(string JobName)
    {
        jobName = JobName;
    }

    public void setDeptName(string DeptName)
    {
        deptName = DeptName;
    }

    public void setProfilePic(Sprite ProfilePic)
    {
        profileSprite = ProfilePic;
    }

    //This is to initialize the buttons (BOTH EDIT AND DELETE)
    public void initializeButtons()
    {
       
    }

    public Animator GetUserAnimator()
    {
        return animator;
    }

    public void DeleteAnimation()
    {
        animator.SetTrigger("Delete");
    }



}
