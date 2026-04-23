using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class InterstitialTypingBehavior : MonoBehaviour
{
    // -- REFS --

    //ref to the AudioSource component on this game object. Assigned during Start()
    AudioSource typingFXSource;
    //refs to the interstitial typing sounds. Chosen from randomly during PlayTypingSFX()
    public List<AudioClip> typingAudioClips = new List<AudioClip>();
    //ref to the singleton. Assigned during Start
    GameManagerBehavior gameManager;
    SceneTransitionBehavior sceneTransitionBehavior;
    

    //---UI STUFFS---//

    //The parent Gameobject for the Title Dialogue
    public GameObject titleDialogueBox;
    //Text that is displayed inside the title dialogue box
    public TextMeshProUGUI titleDialogueText;
    //The parent GameObject for the Subtitle Dialogue
    public GameObject subtitleDialogueBox;
    //Text that is displayed inside the subtitle dialogue box
    public TextMeshProUGUI subtitleDialogueText;
    //font colorssss
    public String typedColorHex;
    public String untypedColorHex;

    // -- TRANSITION ANIMATION --
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;
    //animator on the canvas which plays the ORIGINAL transition anim
    Animator transitionAnimator;
    //animator on the canvas which covers the title with a big ass box
    Animator titleAnimator;
    //animator on the canvas which covers the subtitle with a big ass box
    Animator subtitleAnimator;
    //string representing the name of the scene to load
    public string sceneToLoad;


    //---TYPING STUFFS---//

    //tracks if we should be displaying our text. Toggled on by LoadLine, toggled off during cleanup.
    bool inDialogue = false;
    //container to hold the current Dialogue Box we want to operate on. Set to titleDialogueBox suring Start, Changed by LoadLine().
    GameObject currentDialogueBox;
    //container to hold the currentDialogue Box we are operating on. Set to titleDialogueText during Start, changed by Loadline().
    TextMeshProUGUI currentDialogueText;
    //List of Strings to be displayed
    public List<String> dialogues;
    //Index used to access the dialogues List. 
    public int dialoguesIndex = 0;
    //Text that the user has already typed
    string typedLine;
    //Text that the user has not typed
    string untypedLine;
    //The character that is inserted between the typed and untyped line strings every frame. set to empty when renderCursor is false.
    char currentCursor = '|';
    //This boolean controls what char the currentCursor holds, and is flipped when cursorBlinkTime elapses.
    bool cursorEmpty = false;
    //how frequently, in frames, the cursor should change between rendered and not rendered.
    int cursorBlinkTime = 30;
    //the last character that was parsed
    string keyPressed;
    //List of characters to accept when parsing input
    List<string> recognizedCharacters = new List<string>()
    {
        " ", ".", ",", "!", "?", "\"", "'", ";", ":", "&", "/","\\", "-", ")", "(",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
    };

    private void Start()
    {
        //assign ref to the VoiceBehavior Script in the scene
        typingFXSource = this.gameObject.GetComponent<AudioSource>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign current Dialogue Boxes to title assets
        currentDialogueBox = titleDialogueBox;
        currentDialogueText = titleDialogueText;
        //find and assign animators
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        titleAnimator = GameObject.Find("TitleFadeImage").GetComponent<Animator>();
        subtitleAnimator = GameObject.Find("SubtitleFadeImage").GetComponent<Animator>();

        //if the room dialogue has not been completed 
        if (!gameManager.roomDialogueCompleted)
        {
            //kickstart first line
            LoadLine();
        }

    }

    private void Update()
    {
        //if we are in dialogue we want to be listening for texttttt
        if (inDialogue)
        {
            //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
            currentDialogueText.text = "<color=" + typedColorHex + ">" + typedLine + currentCursor + "<color=" + untypedColorHex + ">" + untypedLine;
            //if a key was pressed
            if (Input.anyKeyDown)
            {
                //store the keypress in a temp variable
                keyPressed = Input.inputString;

                //if this character is one accepted by the system
                if (recognizedCharacters.Contains(keyPressed) && keyPressed.Length == 1)
                {
                    //if this character is the correct character 
                    if (untypedLine.IndexOf(keyPressed) == 0)
                    {
                        //play a typing sound
                        PlayTypingFX();
                        Debug.Log("Correct character! Adding " +  keyPressed + " to line.");
                        //add the key to the typed line
                        typedLine += keyPressed;
                        Debug.Log("Successfully concatenated " +  typedLine);
                        //remove the key that was intended to be typed from the untyped line
                        untypedLine = untypedLine.Remove(0, 1);
                    }

                    //if there is nothing left to be typed
                    if (untypedLine.Length == 0)
                    {
                        //inc index meow
                        dialoguesIndex++;
                        //we are done typin.
                        Debug.Log("finished typing line!");

                        //If there is still a line left to parse...
                        if (dialoguesIndex != dialogues.Count)
                        {
                            //load the next line and repeat
                            LoadLine();
                        }
                        //if there is no more lines left to parse...
                        else
                        {

                            //call fade out animation, which leads to cleanup and termination
                            FadeOutTitleAndSubtitle();
                        }

                    }
                }

            }
        }
    }
    private void FixedUpdate()
    {
        //if the frameCounter mod cursorBlinkTime is 0, flip cursorEmpty. Don't flip if line length is 0, so that the cursor stays empty after subtitles are finished.
        if (gameManager.frameCounter % cursorBlinkTime == 0 && untypedLine.Length != 0)
        {
            cursorEmpty = !cursorEmpty;
        }
        //then, if cursorEmpty is true, set cursor to empty char
        if (cursorEmpty == true)
        {
            currentCursor = ' ';
        }
        //else set cursor to vertical bar
        else
        {
            currentCursor = '|';
        }

        //TRANSITION ANIMATION TIMER
        if (waitingToTransition)
        {
            if (waitTime <= 0)
            {
                waitingToTransition = false;
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

    }


    void LoadLine()
    {
        //set currentlyTyping to true so we cannot activate SceneTransitioners. set to false during cleanup.
        gameManager.currentlyTyping = true;
        //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
        currentDialogueText.text = "<color=" + typedColorHex + ">" + typedLine + "<color=" + untypedColorHex + ">" + untypedLine;
        //set current dialogue boxes and text meshes
        if (dialoguesIndex == 0)
        {
            FadeInTitle();
            currentDialogueBox = titleDialogueBox;
            currentDialogueText = titleDialogueText;
        }
        else if (dialoguesIndex == 1)
        {
            FadeInSubtitle();
            currentDialogueBox = subtitleDialogueBox;
            currentDialogueText = subtitleDialogueText;
        }
        OpenDialogueBox();
        //we are now in dialogue
        inDialogue = true;
        //load the next line into the untypedLine
        untypedLine = dialogues[dialoguesIndex];
        //reset typed Line
        typedLine = "";
    }

    void OpenDialogueBox()
    {
        //enable dialogue box. might change this to be an animation later.
        currentDialogueBox.SetActive(true);
    }
    //TYPING FX HOOK POINT
    void PlayTypingFX()
    {
        //make random index for the clip
        int randomIndex = UnityEngine.Random.Range(0, typingAudioClips.Count);
        //Insert any code necessary to play sfx. Probably something like:
        typingFXSource.PlayOneShot(typingAudioClips[randomIndex]);
        //feel free to make yourself a List of AudioClips under the REFS header up top if you want to play that way.
    }
    //this fct handles any operations that must be done ONCE before termination of the script. 
    void Cleanup()
    {
        //set inDialogue to false
        inDialogue = false;
        //set currentlytyping in gameManager to false so we can once again use Scene Transitioners
        gameManager.currentlyTyping = false;
        //let the gameManger know that we have completed Room Dialogue so it does not retrigger
        gameManager.roomDialogueCompleted = false;
        //play any exit animations and change scenes
        //transition out of scene with an animation. 
        transitionAnimator.Play("TransitionOutOfScene");
        //flip waitintToTransition on so that the timer can start
        waitingToTransition = true;
    }
    //HOOK POINT FOR FADING IN TITLE TEXT
    void FadeInTitle()
    {
        titleAnimator.Play("fadeout");
    }
    //HOK POINT FOR FADING IN SUBTITLE TEXT
    void FadeInSubtitle()
    {
        subtitleAnimator.Play("fadeout");
    }
    //HOOK POINT FOR FADING OUT TITLE AND SUBTITLE TEXT
    void FadeOutTitleAndSubtitle()
    {
        //set cursorEmpty to true so cursor no longer blinks
        cursorEmpty = true;
        titleAnimator.Play("fadein");
        subtitleAnimator.Play("fadein");
        //call cleanup to finalize any outstanding data before terminating the script
        Invoke("Cleanup", 2f);
    }

}
