using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerBehavior : MonoBehaviour
{
    //this static variable holds a single persistent instance of the GameManagerBehavior script
    public static GameManagerBehavior singleton;

    // -- MOUSE INFORMATION --

    //stores the computed location of the mouse in world space
    public Vector2 mouseInWorldSpace;
    //tracks if the mouse is being held. used to prevent spamming of click events.
    public bool holdingLMouse = false;
    // -- GAME STATE --

    //tracks if we are currentlyListening
    public bool currentlyListening = false;
    //tracks if the player has completed the Listening stage
    public bool listeningComplete = false;
    //tracks if the player has talked to the baby for the day
    public bool talkedToBaby = false;
    //tracks if we are currently typing 
    public bool currentlyTyping = false;
    //tracks the current day we are at. Used by BabySpriteChanger compute the sprite that should be shown on the baby. Changed by BedBehavior during cleanup.
    public int currentDay = 1;
    //tracks the current Extremism of the baby. Increased / decreased during TransitionOnClick behavior in response to the accuracy of the playerResponse.
    public int babyExtremism = 0;
    //tracks if we have gone through the initial room text. reset by bed behavior before transitioning to a new day
    public bool roomDialogueCompleted = false;


    //-- RESPONSE PARSING -- (this should only be relevant once every day, during Listening sections. These variables start empty and are emptied again at the end of a Day during BedBehavior.)
    //an empty string, filled with chars as the player types them. Used by Voices object to determine what phonemes to play.
    public string playerResponse;
    //a List of bools, correlating one-to-one with the chars in the playerResponse. Each bool represents whether the corresponding char was a match to the prompt or not. populated as the player types.
    public List<bool> playerResponseAccuracy = new List<bool>();

    //awake runs as soon as the object the script is on is instantiated
    private void Awake()
    {
        //If there is no singleton stored...
        if (singleton == null)
        {
            //mark this instance as the singleton
            singleton = this;
        }
        //else, if there is a singleton...
        else
        {
            //and it is NOT this instance
            if (singleton != this)
            {
                //destroy this entire gameManger gameObject
                Destroy(this.gameObject);
            }
        }
        //If we made it this far without being destroyed, then this object is carries the singleton. Mark it as Don'tDestroy
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //every frame, compute the mouse's world pos and store it
        mouseInWorldSpace = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //-- L MOUSE STATE --

       //every frame, if left mouse is not being held down, reset holdingLMouse to enable another click
       if (Mouse.current.leftButton.isPressed == false)
        {
            holdingLMouse = false;
        }
    }
}
