using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionBehavior : MonoBehaviour
{
    // -- REFS --
    //ref to singleton
    private GameManagerBehavior gameManager;
    //the string of the NAME of the scene the script will transition to ^o^
    public String sceneName;

    // -- TRANSITION ANIMATION --
    //timer to transition
    private float waitTime = 0.5f;
    private bool waitingToTransition;
    
    //animator on the canvas which plays the transition anim
    Animator transitionAnimator;

    void Start()
    {
        //find and assign transition animator
        transitionAnimator = GameObject.Find("TransitionImage").GetComponent<Animator>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
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
    
    public void TransitionTo()
    {
                    
        //transition out of scene with an animation. 
        transitionAnimator.Play("TransitionOutOfScene");
        //kickstart timer to actually move to the next scene 
        waitingToTransition = true;
    }
}
