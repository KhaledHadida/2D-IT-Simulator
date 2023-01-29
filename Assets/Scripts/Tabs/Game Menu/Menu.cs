using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for the menu (attached to MenuPanel)
public class Menu : MonoBehaviour
{
    private GameObject menuPanel;

    //Sound button (ON/OFF)
    public GameObject soundButton;

    //Exit button to main menu
    public GameObject exitButton;

    //We may want to serilaize things (save variables)

    //Close menu button
    public GameObject closeMenu;

    public Animator anim;
    private float animCloseTime;
    

    // Start is called before the first frame update
    void Start()
    {
        menuPanel = this.gameObject;
        anim = GetComponent<Animator>();
        //Since both are same duration (30 ms) we can just use one
        animCloseTime = GameManager.AnimClipTimes(anim,"Menu");

        //Close menu
        toggleMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void toggleMenu(bool open)
    {
        //Toggle
        SoundManage.PlaySound(SoundManage.Sound.MenuClick);

        if (open)
        {
            
            menuPanel.SetActive(open);
        }
        else
        {
            anim.SetTrigger("Close");
            StartCoroutine(GameManager.ToggleObject(menuPanel, animCloseTime, open));
        }
        
    }





    //Exit to menu (save maybe?)
    public void exitToMainMenu()
    {
        Game.LoadScene("MainMenu");
    }


}
