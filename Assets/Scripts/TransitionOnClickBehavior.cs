using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TransitionOnClickBehavior : MonoBehaviour
{
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
            //if the collider contained the mouseInWorldPos
            if (colliderToCheck.bounds.Contains(gameManager.mouseInWorldSpace))
            {
              
                //load the specified scene
                SceneManager.LoadScene(sceneName);
                
            }
            
        }
        else if (Mouse.current.leftButton.isPressed == false && holdingLMouse == true)
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
