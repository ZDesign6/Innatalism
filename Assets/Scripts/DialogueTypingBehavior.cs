using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueTypingBehavior : MonoBehaviour
{
    // -- REFS --
    //ref to the VoiceBehavior Script from the Voices object in the Scene. Assigned during Start()
    VoicesBehavior voiceScript;
    //ref to the singleton. Assigned during Start
    GameManagerBehavior gameManager;

    //---UI STUFFS---//
    
    //The parent gameobject holding every piece of the dialogue box
    GameObject dialogueBox;
    //Text that is displayed inside the dialogue box
    TextMeshProUGUI dialogueText;
    //font colorssss
    public String typedColorHex;
    public String untypedColorHex;
    
    //---TYPING STUFFS---//
    bool inDialogue = false;
    //Text that the user has already typed
    string typedLine;
    //Text that the user has not typed
    string untypedLine;
    //The character that appears between the typed and untyped line strings
    char cursor = '|';
    //the last character that was parsed
    string keyPressed;
    //List of characters to accept when parsing input
    List<string> recognizedCharacters = new List<string>()
    {
        " ", ".", ",", "!", "?", "\"", "'", ";", ":", "&", "/","\\", "-", ")", "(",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    };

    
    //---OTHER---//
    // option for a line to be displayed when the scene loads
    public bool hasEntryLine;
    public string entryLine;

    void Awake()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        CloseDialogueBox();
    }

    private void Start()
    {
        //if this instance has an entry line, display it
        if (hasEntryLine)
        {
            DisplayLine(entryLine);
        }
        //assign ref to the VoiceBehavior Script in the scene
        voiceScript = GameObject.Find("Voices").GetComponent<VoicesBehavior>();
        //and assign self to the voiceScript's typingScript ref
        voiceScript.typingScript = this;
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    private void Update()
    {
        //if we are in dialogue we want to be listening for texttttt
        if (inDialogue)
        {
            
            //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
            dialogueText.text =  "<color=" + typedColorHex + ">" + typedLine + cursor + "<color=" + untypedColorHex + ">"+ untypedLine;
            
            //if a key was pressed
            if (Input.anyKeyDown)
            {
                //store the keypress in a temp variable
                keyPressed = Input.inputString;

                //if this character is one accepted by the system
                if (recognizedCharacters.Contains(keyPressed) && keyPressed.Length == 1)
                {
                    //write it into the playerResponse string, to be parsed later during Listening
                    gameManager.playerResponse = gameManager.playerResponse + keyPressed;
                    //if this character is the correct character 
                    if (untypedLine.IndexOf(keyPressed) == 0)
                    {
                        //write a true boolean into the playerResponseAccuracy List, to be parsed later during Listening
                        gameManager.playerResponseAccuracy.Add(true);
                        //play a positive man sound from the Voices Script
                        voiceScript.PlayPositiveManSound(keyPressed[0]);
                        //BABY GROWTH CALCULATION SHIT!!!
                        
                    }
                    //if the character was incorrect
                    else
                    {
                        //write a false boolean into the playerResponseAccuracy List, to be parsed later during Listening
                        gameManager.playerResponseAccuracy.Add(false);
                        //play a negative man sound from the Voices Script
                        voiceScript.PlayNegativeManSound(keyPressed[0]);
                        //BABY GROWTH CALCULATION SHIT!!!!

                    }
                    
                    //regardless of whether the key is right, 
                    //add the key to the typed line
                    typedLine += keyPressed;
                
                    //remove the key that was intended to be typed from the untyped line
                    untypedLine = untypedLine.Remove(0, 1);
                    
                    //if there is nothing left to be typed
                    if (untypedLine.Length == 0)
                    {
                        //we are done typin.
                        Debug.Log("finished typing line!");
                        inDialogue = false;
                        //close box for now. later i'll allow multiple lines to run at a time without the box closing
                        CloseDialogueBox();
                        //flip currentlyListening on in the gameManager to begin the Listening sequence.
                        gameManager.currentlyListening = true;
                    }
                    
                    //display the typed line in the color we assigned, then the cursor character, then untyped line in the color we assigned
                    dialogueText.text =  "<color=" + typedColorHex + ">" + typedLine + cursor + "<color=" + untypedColorHex + ">"+ untypedLine;
                }
                
            }
        }
    }

    void DisplayLine(string line)
    {
        OpenDialogueBox();
        //we are now in dialogue
        inDialogue = true;
        //the entire line we want to display has not been typed yet
        untypedLine = line;
        
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
