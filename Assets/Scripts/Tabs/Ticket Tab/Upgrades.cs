using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class will be specifically for all upgrades that can be bought from the shop
public class Upgrades
{

    //Upgrades for tickets
    private int numberOfTickets = 2;

    public Upgrades()
    {

    }


    public int NumberOfTickets
    {
        get => numberOfTickets;
        set => numberOfTickets = value;
    }

}
