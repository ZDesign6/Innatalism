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
    //ref to the typing behavior script in the Scene. Assigned during Start()
    BabyTypingBehavior typingScript; 
    //ref to the sprite changer script in the Scene. Assigned during Start()
    BabySpriteChanger spriteChanger;
    //ref to the letter manager
    FloatingLetterManager letterManager;

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
    
    // -- TRANSITION ANIMATION --
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;
    
    //animator on the canvas which plays the transition anim
    Animator transitionAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to Voice Script
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //find and assign transition animator
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        //assign ref to the VoiceBehavior Script in the scene
        typingScript = GameObject.Find("Baby").GetComponent<BabyTypingBehavior>();
        //assign ref to the sprite changer Script in the scene
        spriteChanger = GameObject.Find("Baby").GetComponent<BabySpriteChanger>();
        //assign ref to letterManager
        letterManager = GameObject.Find("Baby").GetComponent<FloatingLetterManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if currently listening
        if (gameManager.currentlyListening == true)
        {
            Debug.Log("Listening delay is currently " + currentDelay);
            //and currentDelay is 0
            if (currentDelay <= 0)
            {
                print("ATTEMPTING TO PARSE PLAYER RESPONSE & ACCURACY AT " + parsingIndex);
                //parse the char at parsingIndex
                char currentChar = gameManager.playerResponse[parsingIndex];
                //then parse the bool at parsingIndex to determine if the currentChar was correct or not
                bool charCorrect = gameManager.playerResponseAccuracy[parsingIndex];
                //if correct, pass the parsed char through to VoicesBehavior PlayPositiveBabySound
                if (charCorrect == true)
                {
                    //play a pos baby sound
                    voiceScript.PlayPositiveBabySound(currentChar);
                    //instantiate a Floating Letter (passing in that it was accurate)
                    letterManager.MakeLetter(currentChar, true);
                }
                //else pass the parsed char through to VoicesBehavior PlayNegativeBabySound
                else
                {
                    //play a neg baby sound
                    voiceScript.PlayNegativeBabySound(currentChar);
                    //instantiate a Floating Letter (passing in that it was accurate)
                    letterManager.MakeLetter(currentChar, false);
                }
                //then play a pulse animation according to whether the char was correct or not
                PlayPulseAnimation(charCorrect);
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
        
        if (waitingToTransition)
        {
            if (waitTime <= 0)
            {
                SceneManager.LoadScene(sceneToTransitionTo);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        
        
                    
    }
    //this fct handles all activities that should be carried out ONCE before listening ends.
    void Cleanup()
    {
        //mark currentlyListening in the gameManager as false
        gameManager.currentlyListening = false;
        spriteChanger.StopBabyAnimation();
        //rest the letterIndex in the FloatingLetterManager to 0, so that it can read the next list accurately (it does not align with parsingIndex)
        this.gameObject.GetComponent<FloatingLetterManager>().letterIndex = 0;
        //stop playing the fetal doppler sound
        this.gameObject.GetComponent<AudioSource>().Stop();
        //if we are not done typing
        if(typingScript.dialoguesIndex != typingScript.activeDialoguesList.Count)
        {
            //load the next line
            typingScript.LoadLine();
        } 
        else 
        {
            //if we are done, mark listeningComplete in the gameManager as true
            gameManager.listeningComplete = true;

             //if we are a tub baby, transition to the sceneToTransitionTo        
            if (isTubBaby == true)
            {
                //transition out of scene with an animation. 
                transitionAnimator.Play("TransitionOutOfScene");
                //kickstart timer to actually move to the next scene 
                waitingToTransition = true;               
            }
        }
       
    }
    //This fct plays a pulsing animation each time the baby speaks a phoneme. Plays a different clip depending on whether the phoneme was true or false (charCorrect).
    void PlayPulseAnimation(bool charCorrect)
    {
        //
    }
    private void OnMouseDown()
    {
        //DEBUG: On click, set parsingIndex equal to the last index to end listening early

        //as long as we are currently listening
        if (gameManager.currentlyListening == true)
        {
            //and as long as skipping is enabled
            if(enableSkipping == true)
            {
                //set the parsing index equal to the last index to end counting next frame
                parsingIndex = gameManager.playerResponse.Count() - 1;
            }
        }
        
    }
}
