using System;
using TMPro;
using UnityEngine;

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
    
    //---BABY GROWTH CALCULATION---//
    //percentage of characters typed in the dialogue that are correct
    float correctnessPercentage;
    //percentage of correct characters that need to be typed correctly in order to progress the baby's extremism "positively" (more clone-like)
    public float minimumCorrectValue;

    private void Start()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        DisplayLine("This is a temporary line to test stuff i looooooveee unity!!! ayay");
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
                
                //add the key to the typed line
                typedLine += keyPressed;
                
                //remove the key that was intended to be typed from the untyped line
                untypedLine = untypedLine.Remove(0, 1);

                //if there is nothing left to be typed
                if (untypedLine.Length == 0)
                {
                    //we are done typin. eventually we'll probably have a pause and then close the dialogue box
                    Debug.Log("finished typing line!");
                    inDialogue = false;
                }
            }
        }
    }

    void DisplayLine(string line)
    {
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
