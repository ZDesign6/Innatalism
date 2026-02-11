using UnityEngine;
using UnityEngine.InputSystem;
public class BabyClickToStartTyping : MonoBehaviour
{
    //This script is responsible for kickstarting Baby Dialogue on clicking

    // -- REFS --

    //ref to singleton
    GameManagerBehavior gameManager;
    //ref to the collider on the baby
    public Collider2D colliderToCheck;
    //ref to the Baby Typing Behavior script on the baby
    public BabyTypingBehavior babyTypingScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //assign ref to collider
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
        //assign ref to the Baby Typing Behavior Script on the baby
        babyTypingScript = this.gameObject.GetComponent<BabyTypingBehavior>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    private void OnMouseDown()
    {
        //if we haven't started talking to the baby yer, kickstart the typing dialogue
        if (gameManager.talkedToBaby == false)
        {
            Debug.Log("you clicked the baby! Attempting to start dialogue!");
            //then kickstart the baby Speaking process
            babyTypingScript.LoadLine();
        }
    }
}
