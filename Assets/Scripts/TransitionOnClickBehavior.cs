using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TransitionOnClickBehavior : MonoBehaviour
{
    // -- LOGIC GATING --

    //track if this is a bed. If it is, then the scene will not transition until the gameManager confirms that the listening segment has been completed
    public bool isBed = false;

    // -- REFERERENCES --

    //variable which holds the Collider with which we will check collision
    Collider2D colliderToCheck;
    //string representing the name of the scene to load
    public string sceneName;
    //ref to singleton
    GameManagerBehavior gameManager;

    // -- INPUT --

    //tracks if the mouse is being held. toggled on after the first isPressed, toggled off after isPressed becomes false.
    bool holdingLMouse = false;

    private void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
        //feftch the first component that falls unyder the collider2D parent class
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
    }
    private void Update()
    {
        //if mouse button is being pressed, and not being held down
        if (Mouse.current.leftButton.isPressed == true && holdingLMouse == false)
        {
            //toggle holdingLMouse on to prevent spam
            holdingLMouse = true;
            //first, check if this object is a bed. If it is, require confirmation of listening being complete
            if (isBed == true)
            {
                //and the game manager confirms that listening has been completed
                if (gameManager.listeningComplete == true)
                {
                    //if the collider contained the mouseInWorldPos
                    if (colliderToCheck.bounds.Contains(gameManager.mouseInWorldSpace))
                    {
                        //load the specified scene
                        SceneManager.LoadScene(sceneName);
                        //and flip listeningComplete to false
                        gameManager.listeningComplete = false;
                    }
                }
                //HOOK IN POINT FOR A DENIAL MESSAGE
                Debug.Log("Listening has not been completed, transition denied");
            }
            //else, if we are not a bed, simply transition without checking if the listening stage has been completed 
            else
            {
                //if the collider contained the mouseInWorldPos
                if (colliderToCheck.bounds.Contains(gameManager.mouseInWorldSpace))
                {
                    //load the specified scene
                    SceneManager.LoadScene(sceneName);
                }
            } 
        }
        else
        {
            //toggle off holdingLMouse to enable another click
            holdingLMouse = false;
        }
    }

    //this fct runs if a mouse clicks while overlapping the collider on this game object(???)
   /* private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }*/
}
