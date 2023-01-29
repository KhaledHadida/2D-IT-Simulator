using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This class is for when selecting a profile picture for the user we're adding (or modifying)
public class ProfilePicSelection : MonoBehaviour
{
    //This script will set up profile pictures to select when making a user
    public List<Sprite> profilePics;

    public GameObject prefab;

    public GameObject parent;

    //Curent button
    public Button chosenProfilePicture;

    public GameObject panel;

    public Image currentImage;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize profilePics
        foreach (var pic in Tickets.allMaleProfilePics)
        {
            profilePics.Add(pic);
        }
        //Initialize profilePics
        foreach (var pic in Tickets.allFemaleProfilePics)
        {
            profilePics.Add(pic);
        }




        if (!panel)
        {
            panel = GameObject.Find("ProfilePicSelection");
        }
        if (!currentImage)
        {
            currentImage = GameObject.Find("ProfilePictureSelector").GetComponent<Image>();
        }

        foreach (var pic in profilePics)
        {
            Button ticketButton;

            GameObject btn = GameObject.Instantiate(prefab, prefab.transform.position, Quaternion.identity);
            //btn.name = "Tickets:" + rand; 
            var rectTransform = btn.GetComponent<RectTransform>();
            rectTransform.SetParent(parent.transform);


            btn.GetComponent<Image>().sprite = pic;
            //Grab the component for the button so we can add functionality to it later
            ticketButton = btn.GetComponent<Button>();
            //When a ticket is clicked, it is displayed on to the middle panel (left, middle and right)
            ticketButton.onClick.AddListener(delegate { SelectProfilePic(btn); });
        }



        gameObject.SetActive(false);
    }


    public void SelectProfilePic(GameObject obj)
    {
        //set current ticket to be this
        chosenProfilePicture = obj.GetComponent<Button>();
        panel.SetActive(false);

        //Change the picture!
        currentImage.sprite = chosenProfilePicture.GetComponent<Image>().sprite;
    }

    public void OpenProfilePics()
    {
        panel.SetActive(true);

    }

}
