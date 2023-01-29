using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//convert this to scriptable object?
public class Ticket : MonoBehaviour
{

    //managed by script: Tickets.cs
    public string ticketerName;
    public string ticketDesc;
    public int timer;
    public bool accepted;
    public bool urgent;
    public Tickets.TicketTypes type;
    public Sprite profilePic;
    public Animator animation;

    public UserTemplate template;


    // Start is called before the first frame update
    void Start()
    {

        //if this is an urgent ticket
        if (urgent)
        {
            Image currentImg = GetComponent<Image>();
            //currentImg.color = Color.red;
            
        }
        

        //Set the tickets that are urgent to animate (red border)
        animation = GetComponent<Animator>();

        
        if (!urgent)
        {
            //animation.enabled = false;
        }
        else
        {
            animation.SetTrigger("Flash");
        }
        animation.SetTrigger("Spawn");
    }

    public void RemoveTicketAnim()
    {
        SoundManage.PlaySound(SoundManage.Sound.InteractTicket);
        animation.SetTrigger("Remove");
    }

    public void SpawnTicketAnim()
    {
        animation.SetTrigger("Spawn");

        if(urgent) animation.SetTrigger("Flash");
    }






}
