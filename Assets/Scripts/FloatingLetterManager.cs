using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingLetterManager : MonoBehaviour
{
    public float letterZPos = 0;
    // -- REFS --

    //ref to singleton
    GameManagerBehavior gameManager;
    //ref to BabyTypingBehavior, used to infer what line of dialogue we are parsing
    BabyTypingBehavior babyTypingScript;
    //these lists holds the spawning positions for letters, in order. They are chosen from conditionally by MakeLetter according to whether the day's dialogue is pos or neg and which line we are parsing (determined by dialoguesIndex from BabyTyping Behavior)
    public List<Vector3> posDialogue1FloatingLetterPos = new List<Vector3>();
    public List<Vector3> posDialogue2FloatingLetterPos = new List<Vector3>();
    public List<Vector3> posDialogue3FloatingLetterPos = new List<Vector3>();
    public List<Vector3> posDialogue4FloatingLetterPos = new List<Vector3>();
    public List<Vector3> negDialogue1FloatingLetterPos = new List<Vector3>();
    public List<Vector3> negDialogue2FloatingLetterPos = new List<Vector3>();
    public List<Vector3> negDialogue3FloatingLetterPos = new List<Vector3>();
    public List<Vector3> negDialogue4FloatingLetterPos = new List<Vector3>();
    //refrence to the prefab to be instantiated
    public GameObject floatingLetterPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to babyTypingScript
        babyTypingScript = this.gameObject.GetComponent<BabyTypingBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*this fct creates an instance of a FloatingLetter prefab, using the pos at the correletaing index in the LetterPos List
     * Replaces the default char in the Prefab with the given char
     * Called during Listening when each character is parsed. */
    public void MakeLetter(int letterIndex, char charToInsert, bool isAccurate)
    {
        //instantiate the Letter and store a ref. Its pos does not matter as it will be overwritten immediately.
        GameObject newLetter = Instantiate(floatingLetterPrefab, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);

        //first, check whicih line we are parsing
        if (babyTypingScript.dialoguesIndex == 1)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = posDialogue1FloatingLetterPos[letterIndex];
            }
            else
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = negDialogue1FloatingLetterPos[letterIndex];
            }
        }
        else if (babyTypingScript.dialoguesIndex == 2)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = posDialogue2FloatingLetterPos[letterIndex];
            }
            else
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = negDialogue2FloatingLetterPos[letterIndex];
            }
        }
        else if (babyTypingScript.dialoguesIndex == 3)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = posDialogue3FloatingLetterPos[letterIndex];
            }
            else
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = negDialogue3FloatingLetterPos[letterIndex];
            }
        }
        else if (babyTypingScript.dialoguesIndex == 4)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = posDialogue4FloatingLetterPos[letterIndex];
            }
            else
            {
                //assign the newLetter's destination pos equal to the pos from the corresponding list
                newLetter.GetComponent<FloatingLetterBehavior>().destinationPos = negDialogue4FloatingLetterPos[letterIndex];
            }

        }
        //we should never get here
        else
        {
            throw new System.Exception("DIALOGUE INDEX OUT OF BOUNDS FOR FLOATING LETTER DRAWING");
        }
        //overwrite the newLetter's Text with the given charToInsert
        newLetter.GetComponent<TextMeshPro>().text = charToInsert.ToString();
        //set the letter's accuracy bool for any behavior triggers
        if (isAccurate == true)
        {
            newLetter.GetComponent<FloatingLetterBehavior>().isAccurate = true;
        }
        else
        {
            newLetter.GetComponent<FloatingLetterBehavior>().isAccurate = false;
        }

    }
}
