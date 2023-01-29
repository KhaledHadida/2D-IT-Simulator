using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //List
    public List<Command> commands = new List<Command>();
    public int currentCommandNum = 0;
    //The arrow we will manipulate
    public GameObject arrowPrefab;
    //The text block 
    public GameObject textObjPrefab;
    private TextMeshProUGUI commandText;

    private GameObject arrow, textObj;

    // Start is called before the first frame update
    void Start()
    {
        //Child text
        commandText = textObjPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();



        Command command = new Command("test", new Vector2(3, 3), new Vector2(2, 2), Command.DIRECTION.NORTH);
        Command command2 = new Command("test", new Vector2(5, 1), new Vector2(5, 2), Command.DIRECTION.NORTH);
        Command command3 = new Command("test", new Vector2(3, 3), new Vector2(10, 2), Command.DIRECTION.NORTH);

        commands.Add(command);
        commands.Add(command2);
        commands.Add(command3);

        //Instantiate both arrow and text obj
        arrow = Instantiate(arrowPrefab, new Vector2(0,0), Quaternion.identity);
        textObj = Instantiate(textObjPrefab, new Vector2(0, 0), Quaternion.identity);


        //Go through all commands
        for (int i = 0; i < commands.Count; i++)
        {

            print(commands.Count);
            goNextCommand(commands[i]);
            

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //No more tutorials (if currentCommandNum == 0 then terminate)
        //So we need to remove it 
        if(currentCommandNum == commands.Count)
        {
            //Destroy(this.gameObject);
        }
    }

    //Go to the next command
    private void goNextCommand(Command command)
    {
        //Make sure we don't exceed command count
        if(currentCommandNum == commands.Count)
        {
            return;
        }

        //Go next
        currentCommandNum++;

        //Update arrow location
        arrow.transform.position = command.getArrowLocation();
        //Update text
        commandText.text = command.Text;
        //Update text location
        textObj.transform.position = command.getTextLoc();
    }

    //Basically orient arrowdir 
    private void changeArrowDir(Command.DIRECTION dir)
    {
        switch (dir)
        {
            case Command.DIRECTION.NORTH:
                arrow.transform.eulerAngles = Vector3.forward * 90;
                break;
            case Command.DIRECTION.WEST:
                arrow.transform.eulerAngles = Vector3.forward * 180;
                break;
            case Command.DIRECTION.EAST:
                arrow.transform.eulerAngles = Vector3.forward * 0;
                break;
            case Command.DIRECTION.SOUTH:
                arrow.transform.eulerAngles = Vector3.forward * 270;
                break;
            default:
                arrow.transform.eulerAngles = Vector3.forward * 90;
                break;
        }
        
    }

    private void changeArrowLoc()
    {

    }
}

// This is the segments for tutorial (i.e steps)
public class Command
{
    public enum DIRECTION {NORTH, EAST, SOUTH, WEST};
    private string text;
    private Vector2 textLocation;
    private Vector2 arrowLocation;
    private DIRECTION arrowDirection;

    public Command(string ctext, Vector2 textLoc, Vector2 arrowLoc, DIRECTION arrowDir)
    {
        text = ctext;
        textLocation = textLoc;
        arrowLocation = arrowLoc;
        arrowDirection = arrowDir;
    }

    public string Text
    {
        get{ return text;}
        set { text = value; }
    }

    public Vector2 getArrowLocation()
    {
        return arrowLocation;
    }

    public DIRECTION getArrowDirection()
    {
        return arrowDirection;
    }

    public Vector2 getTextLoc()
    {
        return textLocation;
    }
    
}
