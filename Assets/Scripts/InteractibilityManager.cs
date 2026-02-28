using Unity.VisualScripting;
using UnityEngine;
public class InteractibilityManager : MonoBehaviour
{
    /*THIS SCRIPT IS RESPONSIBLE FOR DETERMINING THE INTERACTIBILITY STATE OF OBJECTS. EACH INSTANCE
     * DETERMINES WHAT KIND OF OBJECT IT IS ATTACHED TO THROUGH COMPONENT DETECTION, AND THEN MANAGES 
     * ITS INTERACTIBILITY ACCORDINGLY.*/

    // -- REFS --

    //holds a ref to the singleton. Assigned during Start.
    GameManagerBehavior gameManager;

    // -- INTERACTIBILITY --

    /*ultimately determines if this object is in an interactible state or not. This should be the 
     * only part of this script that is ever externally interacted with.*/
    public bool isInteractible = true;

    // -- OBJECT DETECTION --

    /*these bools are used to abstract what kind of object we are attached to.
    Flipped during start and allow the update loop to read more cleanly.*/
    bool isMirror = false;
    bool isBed = false;
    bool isTub = false;
    bool isBaby = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // -- ASSIGN REFS --

        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;

        // -- OBJECT DETECTION --

        //If we are a Mirror, we should have a MirrorTransitionBehavior
        if (this.gameObject.GetComponent<MirrorTransitionBehavior>() != null)
        {
            isMirror = true;
        }
        //If we are a Bed, we should have a BedBehavior
        else if (this.gameObject.GetComponent<BedBehavior>() != null)
        {
            isBed = true;
        }
        //If we are a Tub, we should have a TubTransitionBehavior
        else if (this.gameObject.GetComponent<TubTransitionBehavior>() != null)
        {
            isTub = true;
        }
        //If we are a baby, we should have a BabyTypingBehavior
        else if(this.gameObject.GetComponent<BabyTypingBehavior>() != null) 
        {
            isBaby = true;
        }
        //We should never reach here
        else
        {
            throw new System.Exception("InteractibilityManger could not detect object type")
           {
               //don't need any extra behavior, just want the error
           };
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // -- INTERACTIBILITY CONDITIONS --

        //Mirror conditions
        if (isMirror == true)
        {
            //If we aren't currently listening or talking, then baby isn't being interacted with. Mirror is interactible.
            if (gameManager.currentlyListening == false && gameManager.currentlyTyping == false)
            {
                isInteractible = true;
            }
            else
            {
                isInteractible = false;
            }
        }
        //Bed conditions
        else if (isBed == true) 
        {
            //If listening is completed, then we are interactible
            if (gameManager.listeningComplete == true)
            {
                isInteractible = true;
            }
            else
            {
                isInteractible= false;
            }
        }
        //Tub conditions
        else if (isTub == true)
        {
            //If we aren't currently typing anything, and if we haven't completed listening yet, tub is interactible
            if(gameManager.currentlyTyping == false && gameManager.listeningComplete == false)
            {
                isInteractible = true;
            }
            else
            {
                isInteractible = false;
            }
        }
        //Baby conditions
        else if (isBaby == true)
        {
            //If we haven't talked to the baby yet, the baby is interactible
            if (gameManager.talkedToBaby == false)
            {
                isInteractible = true;
            }
            else
            {
                isInteractible = false;
            }
        }
    }
}
