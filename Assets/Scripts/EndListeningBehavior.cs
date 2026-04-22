using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndListeningBehavior : MonoBehaviour
{
    // -- REFS --
    //ref to the singleton. assigned during Start()
    GameManagerBehavior gameManager;
    //ref to the VoicesBehavior script in the Scene. Assigned during Start()
    VoicesBehavior voiceScript;
    //ref to the letter manager
    EndFloatingLetterManager letterManager;
	//ref to ending walking soundeffect
	AudioSource walkingSfx;


    //string which holds the scene name for the scene which will be transitioned to after Listening IF the baby is a tub baby
    public string sceneToTransitionTo;

    // -- PARSING-RELATED --

    //this tracks what index we are parsing when going through playerResponses during a Listening segment.
    public int parsingIndex = 0;
    //the base delay in frames between audio playback.
    public int basePlaybackDelay = 30;
    //tracks how many frames are left until the next playback. Starts at 0, set after a playback.
    int currentDelay = 0;
    //list of indices at which to Hook in.
    public List<int> hookIndicesList = new List<int>();
    //the max percentage variation in playback delay
    [SerializeField][Range(.15f, .75f)] private float maxPercentVariation = .25f;

    // -- STATE INFO --

    //tracks if this is the Cloney Ending or Blobbey Ending
    public bool isCloney = true;
    //tracks if we are waiting to cleaniup
    public bool waitingToCleanup = false;
    //wait time, in frames
    int baseCleanupTime = 300;
    //current waiting timer. Decreases every frame, triggers cleanup when done.
    int cleanupTimer = 0;
    //if i freakin paused it
    bool isPaused = false;
    public float pauseDuration = 2f;
    private float pauseTimer = 2f;
    
    // -- TRANSITION ANIMATION --
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;
    
    //animator on the canvas which plays the transition anim
    Animator transitionAnimator;

    //anim
    Animator babyAnim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to Voice Script
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //find and assign transition animator
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        //assign ref to letterManager
        letterManager = this.gameObject.GetComponent<EndFloatingLetterManager>();
        //we assign player response 
        babyAnim = GameObject.Find("BABYANIM").GetComponent<Animator>();
		//assign ref to walking audiosource
		walkingSfx = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if currently listening
        if (gameManager.currentlyListening == true && waitingToCleanup == false && !isPaused)
        {
            Debug.Log("Listening delay is currently " + currentDelay);
            //and currentDelay is 0
            if (currentDelay <= 0)
            {
                Debug.Log("ATTEMPTING TO PARSE PLAYER RESPONSE & ACCURACY AT " + parsingIndex);
                //parse the char at parsingIndex
                char currentChar = gameManager.playerResponse[parsingIndex];
                //if this is the Cloney Ending, pass the parsed char through to VoicesBehavior PlayPositiveBabySound
                if (isCloney == true)
                {
                    //if the char was not a space, play a sound
                    if (currentChar != ' ')
                    {
                        //play a pos baby sound
                        voiceScript.PlayPositiveBabySound(currentChar);
                    }
                    //instantiate a Floating Letter (passing in that it was accurate)
                    letterManager.MakeLetter(currentChar, true);
                }
                //else pass the parsed char through to VoicesBehavior PlayNegativeBabySound
                else
                {
                    //if the char was not a space, play a negative sound
                    if (currentChar != ' ')
                    {
                        //play a neg baby sound
                        voiceScript.PlayNegativeBabySound(currentChar);
                    }
                    //instantiate a Floating Letter (passing in that it was accurate)
                    letterManager.MakeLetter(currentChar, false);
                }
                //then play a pulse animation according to whether the char was correct or not
                PlayPulseAnimation(isCloney);
                //CHECK FOR ANY HOOKS
                for (int listIndex = 0; listIndex < hookIndicesList.Count; listIndex = listIndex + 1)
                {
                    //if the current index is the same as any hook index, trigger the UniversalHook fct
                    if (parsingIndex == hookIndicesList[listIndex])
                    {
                        UniversalHook();
                    }
                }
                //finally, increase the parsingIndex to prepare for the next loop
                parsingIndex = parsingIndex + 1;
                Debug.Log("Increased parsing index to " + parsingIndex);
                //then, if the index is not equal to the COUNT of playerResponse (we still have chars left to parse)
                if (parsingIndex != gameManager.playerResponse.Length)
                {
                    //set currentDelay to a variation of basePLaybackDelay
                    currentDelay = (int)Mathf.Round(basePlaybackDelay * Random.Range(maxPercentVariation, (maxPercentVariation + 1)));
                }
                //else (we have parsed the last char)
                else
                {
                    //call hook fct
                    AfterLastChar();
                    //assign the waitTimer to the baseWaitTIme
                    cleanupTimer = baseCleanupTime;
                    //and flip waitingToCleanup to true
                    waitingToCleanup = true;

                }
            }
            //finally, as long as we are meant to be listening, subtract a frame from currentDelay to move us towards the next playback event
            currentDelay = currentDelay - 1;
        }
        //if we are waiting to trigger cleanup...
        if (waitingToCleanup == true)
        {
            //if cleanupTimer is 0, then trigger cleanup and set waitingToCleanup to false
            if (cleanupTimer == 0)
            {
                waitingToCleanup = false;
                Cleanup();
            }
            //If cleanupTimer has not elapsed, decrease it by 1
            else
            {
                //decrease cleanupTimer by 1
                cleanupTimer = cleanupTimer - 1;
            }
        }

        if (isPaused)
        {
            if (pauseTimer <= 0)
            {
                isPaused = false;
                pauseTimer = pauseDuration;
            }
            else
            {
                pauseTimer -= Time.deltaTime;
            }
        }
        
        //transition out of scene timer
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
        //if we are done, mark listeningComplete in the gameManager as true
        gameManager.listeningComplete = true;
        //transition out of scene with an animation. 
        transitionAnimator.Play("TransitionOutOfScene");
        //kickstart timer to actually move to the next scene 
        waitingToTransition = true;               
       
    }
    //This fct plays a pulsing animation each time the baby speaks a phoneme. Plays a different clip depending on whether the phoneme was true or false (charCorrect).
    void PlayPulseAnimation(bool charCorrect)
    {
        //
    }
    //this fct acts as a hook point for any desired behavior. Triggered after parsing the last char, but before Cleanup()
    void AfterLastChar()
    {
        if (!isCloney)
        {
            babyAnim.Play("BLOBBYANIM");
        }
        else
        {
            babyAnim.Play("CLONEYANIM");
			walkingSfx.Play();
        }
        
    }
    //this fct acts as a universal hook point for any desired hooks. It is called immediately parsing an index, and before incrementing the Parsing Index
    void UniversalHook()
    {
        //if this is the blobby ending
        if (!isCloney)
        {
            //kickstart the timer
            isPaused = true;
        }
        else
        {
            isPaused = true;
            
            if (parsingIndex == 26)
            {
                babyAnim.Play("CLONEY2");
                Invoke("DestroyAllLetters", .7f);

            } else if (parsingIndex == 54)
            {
                babyAnim.Play("CLONEY3");
                Invoke("DestroyAllLetters", .7f);
            }
            else if (parsingIndex == 95)
            {
                babyAnim.Play("CLONEY4");
                Invoke("DestroyAllLetters", .7f);
            }
            else if (parsingIndex == 121)
            {
                babyAnim.Play("CLONEY5");
                Invoke("DestroyAllLetters", .7f);

            }
            else if (parsingIndex == 162)
            {
                babyAnim.Play("CLONEY6");
                Invoke("DestroyAllLetters", .25f);
            }
            
        }

    }

    //this fct destroys all currently existing Letter objects in the Scene
    public void DestroyAllLetters()
    {
        //establish a list to hold all active objects to be sorted through
        GameObject[] gameObjects;
        //populate the list with all active objects
        gameObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        //iterate over all objects, destroying them if they are Floating Letter Clones
        for (int currentObject = 0; currentObject < gameObjects.Length; currentObject = currentObject + 1)
        {
            if (gameObjects[currentObject].name == "FloatingLetter(Clone)")
            {
                Destroy(gameObjects[currentObject]);
            }
        }
    }
}
