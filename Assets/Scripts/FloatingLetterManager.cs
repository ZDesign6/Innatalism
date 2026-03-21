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
    public List<Vector2> posDialogue1FloatingLetterPos = new List<Vector2>();
    public List<Vector2> posDialogue2FloatingLetterPos = new List<Vector2>();
    public List<Vector2> posDialogue3FloatingLetterPos = new List<Vector2>();
    public List<Vector2> posDialogue4FloatingLetterPos = new List<Vector2>();
    public List<Vector2> negDialogue1FloatingLetterPos = new List<Vector2>();
    public List<Vector2> negDialogue2FloatingLetterPos = new List<Vector2>();
    public List<Vector2> negDialogue3FloatingLetterPos = new List<Vector2>();
    public List<Vector2> negDialogue4FloatingLetterPos = new List<Vector2>();
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
        GameObject newLetter = Instantiate(floatingLetterPrefab, new Vector3(0,0,0), Quaternion.identity);

        //first, check whicih line we are parsing
        if (babyTypingScript.dialoguesIndex == 1)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(posDialogue1FloatingLetterPos[letterIndex].x, posDialogue1FloatingLetterPos[letterIndex].y, letterZPos);
            }
            else
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(negDialogue1FloatingLetterPos[letterIndex].x, negDialogue1FloatingLetterPos[letterIndex].y, letterZPos);
            }
        }
        else if (babyTypingScript.dialoguesIndex == 2)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(posDialogue2FloatingLetterPos[letterIndex].x, posDialogue2FloatingLetterPos[letterIndex].y, letterZPos);
            }
            else
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(negDialogue2FloatingLetterPos[letterIndex].x, negDialogue2FloatingLetterPos[letterIndex].y, letterZPos);
            }
        }
        else if (babyTypingScript.dialoguesIndex == 3)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(posDialogue3FloatingLetterPos[letterIndex].x, posDialogue3FloatingLetterPos[letterIndex].y, letterZPos);
            }
            else
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(negDialogue3FloatingLetterPos[letterIndex].x, negDialogue3FloatingLetterPos[letterIndex].y, letterZPos);
            }
        }
        else if (babyTypingScript.dialoguesIndex == 4)
        {
            //then check whether it was a pos or neg line (from gameManager)
            if (gameManager.positiveChange == true)
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(posDialogue4FloatingLetterPos[letterIndex].x, posDialogue4FloatingLetterPos[letterIndex].y, letterZPos);
            }
            else
            {
                //assign the newLetter's pos equal to the pos from the corresponding List
                newLetter.GetComponent<RectTransform>().position = new Vector3(negDialogue4FloatingLetterPos[letterIndex].x, negDialogue4FloatingLetterPos[letterIndex].y, letterZPos);
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
