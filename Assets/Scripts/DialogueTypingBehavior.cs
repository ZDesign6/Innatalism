using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueTypingBehavior : MonoBehaviour
{
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
    
    //---AUDIO CLIPS---//
    AudioSource characterVoice;
    public AudioClip positiveVoiceClip;
    public AudioClip negativeVoiceClip;
    //adjustable range for min and max pitch of the voice clips
    [SerializeField] [Range(-2, 2)] private float pitchMin, pitchMax;
    
    //---OTHER---//
    // option for a line to be displayed when the scene loads
    public bool hasEntryLine;
    public string entryLine;

    void Awake()
    {
        characterVoice = GameObject.Find("PlayerVoice").GetComponent<AudioSource>();
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        CloseDialogueBox();
    }

    private void Start()
    {
        if (hasEntryLine)
        {
            DisplayLine(entryLine);
        }
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
                string keyPressed = Input.inputString;

                //recognized characters... this avoids keys like ctrl or shift from counting as an input
                List<string> recognizedCharacters = new List<string>()
                {
                    " ", ".", ",", "!", "?", "'", ";", ":", "&", "/", "-", ")", "(",
                    "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                };

                //if this character is one accepted by the system
                if (recognizedCharacters.Contains(keyPressed) && keyPressed.Length == 1)
                {
                    //stop previous voice clips to prevent overlap
                    characterVoice.Stop();
                    //randomize the pitch within given constraints
                    characterVoice.pitch = Random.Range(pitchMin, pitchMax);
                    
                    //if this character is the correct character 
                    if (untypedLine.IndexOf(keyPressed) == 0)
                    {
                        //BABY GROWTH CALCULATION SHIT!!!
                        //PLAY CORRECT VOICE MUMBLE SOUND HERE YAY
                        characterVoice.PlayOneShot(positiveVoiceClip);
                    }
                    else
                    {
                        //BABY GROWTH CALCULATION SHIT!!!!
                        //PLAY INCORRECT VOICE MUMBLE SOUND HERE AW :(
                        characterVoice.PlayOneShot(negativeVoiceClip);
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
