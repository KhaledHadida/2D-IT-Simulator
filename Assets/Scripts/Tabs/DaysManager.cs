using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaysManager : MonoBehaviour
{
    //Transition days
    public GameObject dayTransitionVeil;
    public GameObject daysObj;

    private TextMeshProUGUI transitionDaysText;
    public TextMeshProUGUI uiDaysText;

    private int currentDay = 1;

    public int CurrentDay
    {
        get => currentDay;
        set => currentDay = value;
    }

    //Transition days animation
    private Animator animator;
    //clip 0 is transitioning out 
    //clip 1 is transitiong into (day is ended)
    AnimationClip[] animationClips;
    // Start is called before the first frame update
    void Start()
    {
        animator = dayTransitionVeil.GetComponent<Animator>();
        transitionDaysText = daysObj.GetComponent<TextMeshProUGUI>();
        uiDaysText = GameObject.Find("Days").GetComponent<TextMeshProUGUI>();
        //gives us the animation length for clip (which is good so we can toggle off the gameobject once animation is done)
        animationClips = animator.runtimeAnimatorController.animationClips;
        //Transition the day
        StartCoroutine(TransitionDay(false));
    }

    public IEnumerator TransitionDay(bool transitionState)
    {

        transitionDaysText.text = "Day: " + currentDay;
        uiDaysText.text = "Day: " + currentDay;
        dayTransitionVeil.SetActive(true);
        //Play anim
        animator.SetBool("TransitionInto", transitionState);

        yield return new WaitForSeconds(transitionState? animationClips[1].length: animationClips[0].length);
        
        //Set it to inactive
        dayTransitionVeil.SetActive(false);
        //Switch back to first tab!
        GameManager.instance.tabManager.switchTabs(0);
    }

    //deprecated 
    public IEnumerator FinishTheDay()
    {
        CurrentDay += 1;
        StartCoroutine(TransitionDay(true));
        GameManager.instance.CloseTransitionDayBox();
        yield return new WaitForSeconds(animationClips[1].length);
        GameManager.instance.StartIT();
    }

    //For the button UI onclick event.
    public void FinishTheDayWrapper()
    {

        StartCoroutine(FinishTheDay());
    }

   


}
