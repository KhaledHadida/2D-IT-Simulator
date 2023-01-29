using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool bossNotification;
    public static float SUCCESS_RATE = 50f;

    //This static bool makes sure the player cannot press other tabs except the following
    public static bool DISABLE_TABS;

    //Game manager will take care of the days (day to day)
    //Game manager will take care of menu, pausing, and options
    //Game manager will take care of saving through maybe player.prefs? 
    //Game manager must persist since it will go from Main Menu screen to GameMode screen


    //what to save:
    //Days # as an int
    //Upgrades 
    //Menu panel to toggle
    private GameObject menuPanel;
    [SerializeField]
    private GameObject transitionToNextDay;

    public Game game;

    public DaysManager daysManage;
    public SwitchTabs tabManager;
    public PerformanceTab performanceManager;
    public GameObject gameOverObj;

    //Prefab for game-over

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    //SO what do we need here?
    //We need to instantiate ALL tickets (depends on day)
    //
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }

    public void PauseGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void ToggleSound()
    {

    }


    //Game over
    //First step instantiate the gameover UI panel
    public void GameOver()
    {
        //The canvas is the parent that we need to set the instantiated gameover obj.
        GameObject canvas = GameObject.Find("Canvas");
        GameObject.Instantiate(gameOverObj, canvas.transform.position, Quaternion.identity, canvas.transform);
    }

    public IEnumerator DeletedAllUsers()
    {
        performanceManager.InitiateBoss();
        yield return new WaitForSeconds(1f);
    }

    public static IEnumerator AnimateTexts(TextMeshProUGUI obj, string text, float speed)
    {

        for (int i = 0; i < text.Length; i++)
        {
            obj.text = text.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }
    }

    //This returns the animation clip length (Static because it may be used in many places)
    public static float AnimClipTimes(Animator anim, string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {

            if (clipName.Equals(clip.name))
            {
                return clip.length;
            }
        }
        return 0;
    }

    //Hope here is to replace lot of the functions to only use this?
    public static IEnumerator ToggleObject(GameObject g, float time, bool state)
    {
        yield return new WaitForSeconds(time);
        g.SetActive(state);
    }

    //TEMPORARY (MAY REMOVE THIS IS TERRIBLE PRACTICE)
    //THIS IS HOOKED UP TO THE MoveToNextDay BUTTON.
    public void StartIT()
    {
        game.StartTheDay();
    }

    public void ExitGame()
    {
        //for now exit out of transition state

    }


    //This closes the "congratulation" dialogue
    public void CloseTransitionDayBox()
    {
        transitionToNextDay.SetActive(false);
    }

    //Here we decide whether the bossman comes through or we proceed to next day..
    public void EndTheDay()
    {
        //Chance of the boss to appear. 
        float chanceOfApperance = Random.Range(0f, 1.0f);

        //Chance 50% 
        if(chanceOfApperance > 0.5f)
        {
            //Boss notification
            performanceManager.InitiateBoss();
        }
        else
        {
            transitionToNextDay.SetActive(true);
            transitionToNextDay.GetComponent<Animator>().SetTrigger("Animate");
        }


    }


    public enum StateType
    {
        MENU,
        GAMEPLAY,

    }

}
