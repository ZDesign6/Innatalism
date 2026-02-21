using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class BabyTypingBehavior : MonoBehaviour
{
    // -- REFS --
    //ref to the VoiceBehavior Script from the Voices object in the Scene. Assigned during Start()
    VoicesBehavior voiceScript;
    //ref to the singleton. Assigned during Start
    GameManagerBehavior gameManager;

    //---UI STUFFS---//
    
    //The parent gameobject holding every piece of the dialogue box
    public GameObject dialogueBox;
    //Text that is displayed inside the dialogue box
    public TextMeshProUGUI dialogueText;
    //font colors. When a key is inputted incorrectly, it turns blob colored. otherwise, it's clone.
    String cloneColorHex = "#F79449";
    String blobColorHex = "#D64995";
    public String untypedColorHex;

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
    //List of characters to accept when parsing input
    List<string> recognizedCharacters = new List<string>()
    {
        " ", ".", ",", "!", "?", "\"", "'", ";", ":", "&", "/","\\", "-", ")", "(",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    };
    
    private void Start()
    {
        //assign ref to the VoiceBehavior Script in the scene
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //and assign self to the voiceScript's typingScript ref
        voiceScript.babyTypingScript = this;
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    private void Update()
    {
        //if we are in dialogue we want to be listening for texttttt
        if (inDialogue)
        {
            
            //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
            dialogueText.text =  typedLine + currentCursor + "<color=" + untypedColorHex + ">"+ untypedLine;
            
            //if a key was pressed
            if (Input.anyKeyDown)
            {
                //store the keypress in a temp variable
                keyPressed = Input.inputString;

                //if this character is one accepted by the system
                if (recognizedCharacters.Contains(keyPressed) && keyPressed.Length == 1)
                {
                    Debug.Log("gameManager is currently " + gameManager.gameObject.name);
                    Debug.Log("Player response accuracy from the game manager is currently " + gameManager.playerResponseAccuracy);
                    //write it into the playerResponse string, to be parsed later during Listening
                    gameManager.playerResponse = gameManager.playerResponse + keyPressed;
                    //if this character is the correct character 
                    if (untypedLine.IndexOf(keyPressed) == 0)
                    {
                        //write a true boolean into the playerResponseAccuracy List, to be parsed later during Listening
                        gameManager.playerResponseAccuracy.Add(true);
                        //play a positive man sound from the Voices Script
                        voiceScript.PlayPositiveManSound(keyPressed[0]);
                        //add the key to the typed line in clone color 
                        typedLine += "<color=" + cloneColorHex + ">"+ keyPressed+"</color>";  
                    }
                    //if the character was incorrect
                    else
                    {
                        //write a false boolean into the playerResponseAccuracy List, to be parsed later during Listening
                        gameManager.playerResponseAccuracy.Add(false);
                        //play a negative man sound from the Voices Script
                        voiceScript.PlayNegativeManSound(keyPressed[0]);
                        typedLine += "<color=" + blobColorHex + ">"+ keyPressed+"</color>";  
                    }
                                
                    //remove the key that was intended to be typed from the untyped line
                    untypedLine = untypedLine.Remove(0, 1);
                    
                    //if there is nothing left to be typed
                    if (untypedLine.Length == 0)
                    {
                        //inc index meow
                        dialoguesIndex++;
                        
                        //we are done typing this line.
                        Debug.Log("finished typing line!");
                        //If the current INdex (which has just been incremented) is not equal to the count, we should load the next line
                        if (dialoguesIndex != dialogues.Count)
                        {
                            //load the next line to continue typing
                            LoadLine();
                        }
                        //else, if the index is now equal to the count, then we are done loading. Cleanup and exit.
                        else
                        {
                            // -- CLEANUP --

                            Cleanup();
                        }
                    }
                }
            }
        }
    }
    //this fct takes care of everything that should happen one time before typing ends
    void Cleanup()
    {
        //flip inDialogue to false
        inDialogue = false;
        //flip currentlyTyping to false
        gameManager.currentlyTyping = false;
        //close the dialogue box
        CloseDialogueBox();
        //flip currentlyListening to start Listening
        gameManager.currentlyListening = true;
        //start playing the fetal doppler
        this.gameObject.GetComponent<AudioSource>().Play();
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
    }
    public void LoadLine()
    {
        //flip talkedToBaby on in case it hasn't been yet
        gameManager.talkedToBaby = true;
        //flip currentlyTyping on
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


}
