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
    [SerializeField][Range(.1f, .9f)] private float correctnessPrecentageThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //fetch the first component that falls unyder the collider2D parent class
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void OnMouseDown()
    {
        //and the game manager confirms that listening has been completed
        if (gameManager.listeningComplete == true)
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
            Debug.Log("Player had " + correctChars + " out of " + gameManager.playerResponseAccuracy.Count + ", making accuracy " +  responseAccuracy);
            /*finally, if the responseAccuracy exceeded the accuracyThreshold, then move extremism UP*/
            if (responseAccuracy > correctnessPrecentageThreshold)
            {

                gameManager.babyExtremism = gameManager.babyExtremism + 1;
                Debug.Log("That exceeds the threshold of " + correctnessPrecentageThreshold + ", making the new Extremism " + gameManager.babyExtremism);
            }
            /*else, move extremism DOWN*/
            else
            {
                gameManager.babyExtremism = gameManager.babyExtremism - 1;
                Debug.Log("That does NOT exceed the threshold of " + correctnessPrecentageThreshold + ", making the new Extremism " + gameManager.babyExtremism);
            }

            // -- CLEANUP --

            //assign player response equal to an empty string
            gameManager.playerResponse = "";
            //empty our the playerresponse accuracy list
            gameManager.playerResponseAccuracy.Clear();
            //and flip listeningComplete to false
            gameManager.listeningComplete = false;
            //increase currentDay to ensure the baby's sprite changes accurately
            gameManager.currentDay = gameManager.currentDay + 1;
            //reset room dialogue completed to prep for next day!!!
            gameManager.roomDialogueCompleted = false;
            //reset talkedToBaby to prep for next day
            gameManager.talkedToBaby = false;

            //load the next day
            SceneManager.LoadScene(nextDaySceneName);
        }
        else
        {
            //HOOK IN POINT FOR DENIAL BEHAVIOR
            Debug.Log("Listening has not been completed, transition denied");
        }

        
    }
}
