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

    
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;
    
    //animator on the canvas which plays the transition anim
    Animator transitionAnimator;
    
    private void Start()
    {
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
        //feftch the first component that falls unyder the collider2D parent class
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (waitingToTransition)
        {
            if (waitTime <= 0)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    private void OnMouseDown()
    {
        //And if we aren't currently listening or talking
        if (gameManager.currentlyListening == false && gameManager.currentlyTyping == false)
        {
            //transition out of scene with an animation. 
            transitionAnimator.Play("TransitionOutOfScene");
            //kickstart timer to actually move to the next scene 
            waitingToTransition = true;
        }
    }
}
