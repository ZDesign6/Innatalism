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
    private void FixedUpdate()
    {
    }
    private void OnMouseDown()
    {
        //And if we aren't currently listening
        if (gameManager.currentlyListening == false)
        {
            //load the specified scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
