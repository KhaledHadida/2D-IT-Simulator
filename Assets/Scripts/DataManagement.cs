using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagement : MonoBehaviour
{
    [Header("dependencies")]
    [SerializeField]
    TextAsset Usersfile;
    [SerializeField]
    TextAsset Ticketsfile;
    [SerializeField]
    TextAsset FemaleUserNamesfile;
    [SerializeField]
    TextAsset MaleUserNamesfile;
    [SerializeField]
    TextAsset JobNamesfile;
    public static string[] userEntries;
    //Ticket descriptions (These should be accompanied with a TicketType)
    public static string[] ticketEntries;
    //random names in a list we can access randomly
    public static string[] femaleUserNamesList;
    public static string[] maleUserNamesList;
    //Jobs and Departments in a list
    public static string[] jobNamesList;
    public void Awake()
    {
        //USER ENTRIES MAY BE OBSOLETE SINCE I DONT RELY ON IT ANYMORE (WE RANDOMLY GENERATE NOW)
        userEntries = handleText(Usersfile);
        ticketEntries = handleText(Ticketsfile);
        femaleUserNamesList = handleText(FemaleUserNamesfile);
        maleUserNamesList = handleText(MaleUserNamesfile);
        jobNamesList = handleText(JobNamesfile);
    }

    private string[] handleText(TextAsset txt)
    {
        string[] newList;

        if (txt)
        {
            return newList = txt.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        return null;
    }


}
