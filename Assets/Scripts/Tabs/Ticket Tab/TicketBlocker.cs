using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This will conceal/reveal the blocker with animation (when accepting tickets it hides)
//This script is attached to the "Hidden" gameobj
public class TicketBlocker : MonoBehaviour
{
    public GameObject blockerParent;
    //True means blocker is active, false means its not
    public bool blocker;
    private GameObject blockerObj;
    private Animator blockerObjAnim;


    // Start is called before the first frame update
    void Start()
    {
        blockerObj = this.gameObject;
        blockerObjAnim = GetComponent<Animator>();

        //This is solution to having the UI misalign when transitioning from Menu to Main game state
        //Refreshes the canvas and aligns everything!
        Canvas.ForceUpdateCanvases();  // *
        blockerParent.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false; // **
        blockerParent.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;

    }

    public void Show()
    {

        if (blocker)
        {
            //This solves the problem where the trigger is stuck in the other state (i.e in HIDE state)
            blockerObjAnim.ResetTrigger("Hide");
            blockerObjAnim.SetTrigger("Show");
            blocker = false;
        }
        
    }

    public void Hide()
    {

        if (!blocker)
        {
            blockerObjAnim.ResetTrigger("Show");
            blockerObjAnim.SetTrigger("Hide");
            blocker = true;
        }
        

    }





}
