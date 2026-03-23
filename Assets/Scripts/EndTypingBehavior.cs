using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EndTypingBehavior : MonoBehaviour
{
    // -- ENDING SPECIFIC --

    //the amt of frames to wait after finishing each line of dialogue
    public int dialogue1WaitTime = 100;
    public int dialogue2WaitTime = 100;
    public int dialogue3WaitTime = 100;
    public int dialogue4WaitTime = 100;
    //tracks how many frames we need to wait
    int remainingWaitTime = 0;
    //tracks if we are currently in waiting state. turned on after finishing a dialogue line. Turned off after waiting ends.
    public bool isWaiting;

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
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
    };
    
    void Awake()
    {
        sceneTransitionBehavior = GameObject.Find("SceneTransitioner").GetComponent<SceneTransitionBehavior>();
    }

    private void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //kick start dialogue
        LoadLine();
        //assign ref to the VoiceBehavior Script in the scene
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //abstract how extreme we currently are by adding an offset to ensure all numbers are positive
        float extremismProportion = (float)gameManager.babyExtremism + 8f / 16f;
        Debug.Log("current extremism proportion is " +  extremismProportion);
        //compute currentMantraTextColor
        currentMantraTextColor = Color.Lerp(gameManager.blobColorRGB, gameManager.cloneColorRGB, extremismProportion);
    }

    private void Update()
    {
        //if we are in dialogue and not waiting, allow input
        if (inDialogue && isWaiting == false)
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
                        //add the key to the typed line regardless of accuracy
                        typedLine += keyPressed;
                        //remove the key that was intended to be typed from the untyped line
                        untypedLine = untypedLine.Remove(0, 1);
                        //play a positive man sound from the Voices Script
                        voiceScript.PlayPositiveManSound(keyPressed[0]);
                    }
                    //if the character was incorrect
                    else
                    {
                        //play a negative man sound from the Voices Script
                        voiceScript.PlayNegativeManSound(keyPressed[0]);
                    }   
                    
                    //if there is nothing left to be typed
                    if (untypedLine.Length == 0)
                    {
                        //inc index meow
                        dialoguesIndex++;
                        
                        //we are done typin.
                        Debug.Log("finished typing line!");
                        //turn on isWaiting to begin waiting
                        isWaiting = true;
                        //close the dialogue box to hide it
                        CloseDialogueBox();
                        //assign remainingWaitTime according to which line we jsut finished typing. Call hook functions.
                        if (dialoguesIndex == 1)
                        {
                            remainingWaitTime = dialogue1WaitTime;
                            AfterDialogueOne();
                        }
                        else if (dialoguesIndex == 2)
                        {
                            remainingWaitTime = dialogue2WaitTime;
                            AfterDialogueTwo();
                        }
                        else if (dialoguesIndex == 3)
                        {
                            remainingWaitTime = dialogue3WaitTime;
                            AfterDialogueThree();
                        }
                        else if (dialoguesIndex == 4)
                        {
                            remainingWaitTime = dialogue4WaitTime;
                            AfterDialogueFour();
                        }
                        //should never reach here
                        else
                        {
                            throw new Exception("TOO MANY LINES OF DIALOGUE DURING ENDTYPING");
                        }
                    }
                    
                }
                
            }
        }
    }
    private void FixedUpdate()
    {
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
        //if we are waiting, call Wait()
        if (isWaiting == true)
        {
            Wait();
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
    //this fct handles all behaviors that must be done once before leaving this script.
    void Cleanup()
    {
        //flip inDialogue to false so we don't keep displaying text
        inDialogue = false;
        //flip currentlyTyping in gameManger to false so we can once again trigger SceneTransitioners
        gameManager.currentlyTyping = false;
        CloseDialogueBox();
        //transition
        sceneTransitionBehavior.TransitionTo();
    }
    //this fct does nothing except decrement the waiting timer. If called while remainingWaitTime is 0, ends waiting.
    void Wait()
    {
        //if there is no more time to wait...
        if (remainingWaitTime == 0)
        {
            //and if we are on the last line of dialogue
            if (dialoguesIndex == dialogues.Count)
            {
                //then initiate cleanup
                Cleanup();
            }
            //else if we've got more lines to parse
            else
            {
                isWaiting = false;
                LoadLine();
            }
        }
        //else, decrement remainingWaitTime
        else
        {
            remainingWaitTime = remainingWaitTime - 1;
        }
    }
    //this fct is called after finishing dialogue line 1. Acts as a hook for any desired behavior.
    void AfterDialogueOne()
    {
        //TEMP TEST
        GameObject.Find("SampleBG").GetComponent<SpriteRenderer>().color = Color.red;
    }
    //this fct is called after finishing dialogue line 2. Acts as a hook for any desired behavior.
    void AfterDialogueTwo()
    {
        //TEMP TEST
        GameObject.Find("SampleBG").GetComponent<SpriteRenderer>().color = Color.black;
    }
    //this fct is called after finishing dialogue line 3. Acts as a hook for any desired behavior.
    void AfterDialogueThree()
    {
        //TEMP TEST
        GameObject.Find("SampleBG").GetComponent<SpriteRenderer>().color = Color.blue;
    }
    //this fct is called after finishing dialogue line 4. Acts as a hook for any desired behavior.
    void AfterDialogueFour()
    {
        GameObject.Find("SampleBG").GetComponent<SpriteRenderer>().color = Color.green;
    }
}
