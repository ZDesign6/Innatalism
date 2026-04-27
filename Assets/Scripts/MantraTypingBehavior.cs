using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MantraTypingBehavior : MonoBehaviour
{
    // -- REFS --
    //ref to the VoiceBehavior Script from the Voices object in the Scene. Assigned during Start()
    VoicesBehavior voiceScript;
    //ref to the singleton. Assigned during Start
    GameManagerBehavior gameManager;
    SceneTransitionBehavior sceneTransitionBehavior;

    //---UI STUFFS---//
    
    //The parent gameobject holding every piece of the dialogue box
    public GameObject dialogueBox;
    //Text that is displayed inside the dialogue box
    public TextMeshProUGUI dialogueText;

    //---TYPING STUFFS---//

    //tracks if we should be displaying our text. Toggled on by LoadLine, toggled off during cleanup.
    bool inDialogue = false;
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
    //the color to draw Mantra text in. Computed during Start() by lerping between BlobColorRGB and CloneColorRGB according to how extreme we currently are
    Color currentMantraTextColor;
    //List of characters to accept when parsing input
    List<string> recognizedCharacters = new List<string>()

    {
        " ", ".", ",", "!", "?", "\"", "'", ";", ":", "&", "/","\\", "-", ")", "(",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    };

    // -- FAKE TYPING STUFF --

    //this tracks the base delay, in frames, between letters being "typed"
    public int baseTypeDelay;
    //this sets the variability in actual delay between letters being "typed". Used to modify the base delay before applying it
    [SerializeField][Range(.15f, .75f)] private float maxPercentVariation = .25f;
    //used to track the currently remaining frames between "typing" letters. REduced every frame.
    int currentTypeDelay = 0;

    void Awake()
    {
        sceneTransitionBehavior = GameObject.Find("SceneTransitioner").GetComponent<SceneTransitionBehavior>();
    }

    private void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to the VoiceBehavior Script in the scene
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //and assign self to the voiceScript's typingScript ref
        voiceScript.mantraTypingScript = this;
        //abstract how extreme we currently are by adding an offset to ensure all numbers are positive
        float extremismProportion = (float)gameManager.babyExtremism + 8f / 16f;
        Debug.Log("current extremism proportion is " + extremismProportion);
        //compute currentMantraTextColor
        currentMantraTextColor = Color.Lerp(gameManager.blobColorRGB, gameManager.cloneColorRGB, extremismProportion);
        //kick start dialogue
        LoadLine();
        
    }

    private void Update()
    {
        //if we are in dialogue and NOT in the Cloney Ending Mantra
        if (inDialogue == true && SceneManager.GetActiveScene().name != "CloneyEndingMirror")
        {
            //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
            dialogueText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentMantraTextColor) + ">" + typedLine + currentCursor + "<color=" + gameManager.untypedColorHex + ">"+ untypedLine;
            
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
                        //play a positive man sound from the Voices Script
                        voiceScript.PlayPositiveManSound(keyPressed[0]);
                    }
                    //if the character was incorrect
                    else
                    {
                        //play a negative man sound from the Voices Script
                        voiceScript.PlayNegativeManSound(keyPressed[0]);
                    }
                    
                    //add the key to the typed line regardless of accuracy
                    typedLine += keyPressed;              
                    //remove the key that was intended to be typed from the untyped line
                    untypedLine = untypedLine.Remove(0, 1);
                    
                    //if there is nothing left to be typed
                    if (untypedLine.Length == 0)
                    {
                        //inc index meow
                        dialoguesIndex++;
                        
                        //we are done typin.
                        Debug.Log("finished typing line!");
                        
                        if (dialoguesIndex != dialogues.Count)
                        {
                            LoadLine();
                        }
                        else
                        {
                            // -- CLEANUP --

                            //flip inDialogue to false so we don't keep displaying text
                            inDialogue = false;
                            //flip currentlyTyping in gameManger to false so we can once again trigger SceneTransitioners
                            gameManager.currentlyTyping = false;
                            CloseDialogueBox();
                            //transition
                            sceneTransitionBehavior.TransitionTo();
                            
                        }
                    }
                    
                }
                
            }
        }
        
    }
    private void FixedUpdate()
    {
        // FAKE TYPING
        //else, if we are inDialogue and the currentScene is the CLoney Ending Mirror, execute differently (fake typing)
        if (inDialogue == true && SceneManager.GetActiveScene().name == "CloneyEndingMirror")
        {
            //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
            dialogueText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentMantraTextColor) + ">" + typedLine + currentCursor + "<color=" + gameManager.untypedColorHex + ">" + untypedLine;

            //if the timer has reached 0
            if (currentTypeDelay <= 0)
            {
                //reset currentTypeDelay to a variation of baseTypeDelay
                int newDelay = (int)Mathf.Round(baseTypeDelay * (UnityEngine.Random.Range(maxPercentVariation, maxPercentVariation + 1)));
                Debug.Log("set delay to" + newDelay);
                currentTypeDelay = newDelay;
                //play a positive man sound from the Voices Script
                voiceScript.PlayPositiveManSound(untypedLine[0]);
                //add the key to the typed line regardless of accuracy
                typedLine += untypedLine[0];
                //remove the key that was intended to be typed from the untyped line
                untypedLine = untypedLine.Remove(0, 1);
                //if there is nothing left to be typed
                if (untypedLine.Length == 0)
                {
                    //inc index meow
                    dialoguesIndex++;

                    //we are done typin.
                    Debug.Log("finished typing line!");

                    if (dialoguesIndex != dialogues.Count)
                    {
                        LoadLine();
                    }
                    else
                    {
                        Cleanup();
                        Invoke("MoveToCredits", 1f);
                    }
                }
            }
            //finally, reduce the currentTypeDelay by one to decrease the timer
            currentTypeDelay = currentTypeDelay - 1;
        }
        //if the frameCounter mod cursorBlinkTime is 0, flip cursorEmpty
        if (gameManager.frameCounter % cursorBlinkTime == 0)
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
    }


    void LoadLine()
    {
        //set gameManger currently typing to true so we cannot trigger and SceneTransitioners. It will be flipped off during cleanup.
        gameManager.currentlyTyping = true;
        OpenDialogueBox();
        //we are now in dialogue
        inDialogue = true;
        //the entire line we want to display has not been typed yet
        untypedLine = dialogues[dialoguesIndex];
        typedLine = "";
    }
    
    void CloseDialogueBox()
    {
        //disable dialogue box. might change this to be an animation later.
        dialogueBox.SetActive(false);
    }

    void OpenDialogueBox()
    {
        //enable dialogue box. might change this to be an animation later.
        dialogueBox.SetActive(true);
    }

    void Cleanup()
    {
        // -- CLEANUP --

        //flip inDialogue to false so we don't keep displaying text
        inDialogue = false;
        //flip currentlyTyping in gameManger to false so we can once again trigger SceneTransitioners
        gameManager.currentlyTyping = false;
        CloseDialogueBox();
    }

    void MoveToCredits()
    {
        //transition
        sceneTransitionBehavior.TransitionTo();
    }
    
    
}
