using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class BedBehavior : MonoBehaviour
{

    // -- REFERERENCES --

    //variable which holds the Collider with which we will check collision
    Collider2D colliderToCheck;
    //string representing the name of the scene to load
    public string nextDaySceneName;
    //ref to singleton
    GameManagerBehavior gameManager;

    // -- MUTATION SYSTEM --
    //percentage threshold that must be met for the playerResponse to be considered overall "correct".
    [SerializeField][Range(.1f, .9f)] private float minimumCorrectnessPercentage;
    
    // -- TRANSITION ANIMATION --
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;

    // -- EDNING THRESHOLDS --
    //the threshold which must be met or exceeded to load Cloney Ending on Day 9
    int cloneyThreshold = 6;
    //the threshold which must be met or less than to load Bobbey Ending on Day 9
    int blobbeyThreshold = -6;
    
    //animator on the canvas which plays the transition anim
    Animator transitionAnimator;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find and assign transition animator
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        //fetch the first component that falls unyder the collider2D parent class
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waitingToTransition)
        {
            if (waitTime <= 0)
            {

                waitingToTransition = false;
                LoadNextScene();

            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
       
    }
    private void OnMouseDown()
    {
        //If we are currently interactible
        if (this.gameObject.GetComponent<InteractibilityManager>().isInteractible == true)
        {
            //only compute extremism changes if day is not 9
            if (gameManager.currentDay != 9)
            {
                //-- MUTATION --
                /* first, check how many correct chars were recorded */

                //local var to track how many true (correct) bools were stored
                int correctChars = 0;
                //iterate over the responseAccuracy to determine how many correct answers there were
                for (int accuracyIndex = 0; accuracyIndex < gameManager.playerResponseAccuracy.Count; accuracyIndex = accuracyIndex + 1)
                {
                    //If a true bool is encountered, tally it
                    if (gameManager.playerResponseAccuracy[accuracyIndex] == true)
                    {
                        correctChars = correctChars + 1;
                    }
                }
                /*next, calculate the overall accuracy percentage by dividing the correctChars by the total number of Chars recorded*/
                float responseAccuracy = (float)correctChars / (float)gameManager.playerResponseAccuracy.Count;
                Debug.Log("Player had " + correctChars + " out of " + gameManager.playerResponseAccuracy.Count + ", making accuracy " + responseAccuracy);
                /*finally, if the responseAccuracy exceeded the accuracyThreshold, then move extremism UP*/
                if (responseAccuracy > minimumCorrectnessPercentage)
                {
                    //inc extremism
                    gameManager.babyExtremism = gameManager.babyExtremism + 1;
                    //and set positiveChange to true
                    gameManager.positiveChange = true;
                    Debug.Log("That exceeds the threshold of " + minimumCorrectnessPercentage + ", making the new Extremism " + gameManager.babyExtremism);
                }
                /*else, move extremism DOWN*/
                else
                {
                    //dec extremism
                    gameManager.babyExtremism = gameManager.babyExtremism - 1;
                    //and set positiveChange to false
                    gameManager.positiveChange = false;
                    Debug.Log("That does NOT exceed the threshold of " + minimumCorrectnessPercentage + ", making the new Extremism " + gameManager.babyExtremism);
                }
            }

            // -- CLEANUP --

            Cleanup();
        }
        else
        {
            //HOOK IN POINT FOR DENIAL BEHAVIOR
            Debug.Log("Listening has not been completed, transition denied");
        }

        
    }
    //This fct handles resetting and clearing any data to its default state before starting the next day.
    void Cleanup()
    {
        //assign player response equal to an empty string
        gameManager.playerResponse = "";
        //empty our the playerresponse accuracy list
        gameManager.playerResponseAccuracy.Clear();
        //and flip listeningComplete to false
        gameManager.listeningComplete = false;
        if (gameManager.currentDay != 9)
        {
            //increase currentDay to ensure the baby's sprite changes accurately
            gameManager.currentDay = gameManager.currentDay + 1;
        }
        //reset room dialogue completed to prep for next day!!!
        gameManager.roomDialogueCompleted = false;
        //reset talkedToBaby to prep for next day
        gameManager.talkedToBaby = false;
        //transition out of scene with an animation. 
        transitionAnimator.Play("TransitionOutOfScene");
        //flip waitintToTransition on so that the timer can start
        waitingToTransition = true;
    }
    /*handles loading the next scene. Generally this means loading the scene named in nextDayScene.
     * On Day 9, this will conditionally load an Ending if the current extremism exceeds thresholds */
    void LoadNextScene()
    {
        Debug.Log("gameManger.currentDay is currently " + gameManager.currentDay);
        //if during day 9
        if (gameManager.currentDay == 9)
        {
            //if greater than cloney threshold
            if (gameManager.babyExtremism >= cloneyThreshold)
            {
                SceneManager.LoadScene("CloneyEnding");
            }
            //else if less than blobbey threshold
            else if (gameManager.babyExtremism <= blobbeyThreshold)
            {
                SceneManager.LoadScene("BlobbeyEnding");
            }
            //else load neutral ending
            else
            {
                SceneManager.LoadScene("NeutralEnding");
            }
        }
        //else load nextDayScene
        else
        {
            SceneManager.LoadScene(nextDaySceneName);
        }
    }
}
