using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndFloatingLetterManager : MonoBehaviour
{
    public float letterZPos = 0;

    // -- PARSING --

    // -- REFS --

    //ref to singleton
    GameManagerBehavior gameManager;
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
    //ref to EndListeningBehavior, used to grab parsingIndex
    EndListeningBehavior endListeningScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to endListeningBehavior
        endListeningScript = this.gameObject.GetComponent<EndListeningBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*this fct creates an instance of a FloatingLetter prefab, using the pos at the correletaing index in the LetterPos List
     * Replaces the default char in the Prefab with the given char
     * Called during Listening when each character is parsed. */
    public void MakeLetter(char charToInsert, bool isAccurate)
    {
        //instantiate the Letter and store a ref. Its pos does not matter as it will be overwritten immediately.
        GameObject newLetter = Instantiate(floatingLetterPrefab, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
        //assign the newLetter's destination pos equal to the pos from the corresponding list
        newLetter.GetComponent<FloatingLetterBehavior>().homePos = posDialogue1FloatingLetterPos[endListeningScript.parsingIndex];
        //then assign the newLetter's pos equal to the pos form the corresponding list
        newLetter.GetComponent<RectTransform>().position = posDialogue1FloatingLetterPos[endListeningScript.parsingIndex];

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
        //finally, increase the endListeningScript.parsingIndex to prepare to parse the next char
        //endListeningScript.parsingIndex = endListeningScript.parsingIndex + 1;
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
