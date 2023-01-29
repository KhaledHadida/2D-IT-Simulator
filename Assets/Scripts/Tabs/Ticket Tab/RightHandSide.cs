using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//This script will be placed on RightHandSide object.
public class RightHandSide : MonoBehaviour
{
    //This is the big panel on right (we will use it to place tickets as "accepted")
    public GameObject rightSidePanel;

    //Bottom text displaying MAX tickets accepted;
    private TextMeshProUGUI maxTicketText;

    public Tickets ticketManager;

    private Animator animator;

    public Upgrades perkUpgrades;


    // Start is called before the first frame update
    void Start()
    {
        rightSidePanel = GameObject.Find("RightSideScrolling");
        maxTicketText = GameObject.Find("MaxTicketNumber").GetComponent<TextMeshProUGUI>();
        animator = maxTicketText.GetComponent<Animator>();
        ticketManager = GameObject.Find("_manager").GetComponent<Tickets>();
        perkUpgrades = ticketManager.upgradeManager;
        maxTicketText.text = ""+perkUpgrades.NumberOfTickets;
    }

    //play animation
    public void Flash()
    {
        animator.SetTrigger("Flash");
    }

    public void SetText()
    {
        maxTicketText.text = ""+perkUpgrades.NumberOfTickets;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
