using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListeningBehavior : MonoBehaviour
{
    // -- REFS --

    //ref to the singleton. assigned during Start()
    GameManagerBehavior gameManager;
    //ref to the VoicesBehavior script in the Scene. Assigned during Start()
    VoicesBehavior voiceScript;
    //string which holds the scene name for the scene which will be transitioned to after Listening IF the baby is a tub baby
    public string sceneToTransitionTo;

    // -- PARSING-RELATED --

    //this tracks what index we are parsing when going through playerResponses during a Listening segment.
    int parsingIndex = 0;
    //the base delay in frames between audio playback.
    public int basePlaybackDelay = 30;
    //the max percentage variation in playback delay
    [SerializeField][Range(.15f,.75f)] private float maxPercentVariation = .25f;
    //tracks how many frames are left until the next playback. Starts at 0, set after a playback.
    int currentDelay = 0;

    // -- STATE INFO --

    //represents if this is a tub interior baby. Used to determine if we should do a scene transition after Listening.
    public bool isTubBaby = false;
    //enables skipping the listening segment by clicking on the baby during Listening.
    public bool enableSkipping = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to Voice Script
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //if currently listening
        if (gameManager.currentlyListening == true)
        {
            Debug.Log("Listening delay is currently " + currentDelay);
            //and currentDelay is 0
            if (currentDelay == 0)
            {
                //parse the char at parsingIndex
                char currentChar = gameManager.playerResponse[parsingIndex];
                //then parse the bool at parsingIndex to determine if the currentChar was correct or not
                bool charCorrect = gameManager.playerResponseAccuracy[parsingIndex];
                //if correct, pass the parsed char through to VoicesBehavior PlayPositiveBabySound
                if (charCorrect == true)
                {
                    voiceScript.PlayPositiveBabySound(currentChar);
                }
                //else pass the parsed char through to VoicesBehavior PlayNegativeBabySound
                else
                {
                    voiceScript.PlayNegativeBabySound(currentChar);
                }
                //finally, increase the parsingIndex to prepare for the next loop
                parsingIndex = parsingIndex + 1;
                //then, if the index is not equal to the COUNT of playerResponse (we still have chars left to parse)
                if (parsingIndex != gameManager.playerResponse.Length) 
                {
                    //set currentDelay to a variation of basePLaybackDelay
                    currentDelay = (int)Mathf.Round(basePlaybackDelay * Random.Range(maxPercentVariation, (maxPercentVariation + 1)));
                }
                //else (we have parsed the last char)
                else
                {
                    //carry out cleanup
                    Cleanup();
                }
            }
            //finally, as long as we are meant to be listening, subtract a frame from currentDelay to move us towards the next playback event
            currentDelay = currentDelay - 1;
        }
                    
    }
    //this fct handles all activities that should be carried out ONCE before listening ends.
    void Cleanup()
    {
        //mark currentlyListening in the gameManager as false
        gameManager.currentlyListening = false;
        //and mark listeningComplete in the gameManager as true
        gameManager.listeningComplete = true;
        //stop playing the fetal doppler sound
        this.gameObject.GetComponent<AudioSource>().Stop();
        //if we are a tub baby, transition to the sceneToTransitionTo
        if (isTubBaby == true)
        {
            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }
    private void OnMouseDown()
    {
        //DEBUG: On click, set parsingIndex equal to the last index to end listening early

        //as long as we are currently listening
        if (gameManager.currentlyListening == true)
        {
            //set the parsing index equal to the last index to end counting next frame
            parsingIndex = gameManager.playerResponse.Count() - 1;
        }
        
    }
}
