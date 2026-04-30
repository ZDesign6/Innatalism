using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HoverManager : MonoBehaviour
{
    String Interactable = "Interactable";
    String notInteractable = "NotInteractableHover";
    String notHovering = "NotHovering";   
    public Animator hoverAnimator;
    
    //holds a ref to the singleton. Assigned during Start.
    GameManagerBehavior gameManager;
    
    //called when cursor hovers over object
    private void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    public void AnimateHover()
    {
        //if obj is interactable, play according animation
        if (this.gameObject.GetComponent<InteractibilityManager>().isInteractible == true )
        {
            if (gameObject.name == "Mirror" && gameManager.mirrorInteracted)
            {
                hoverAnimator.Play(notInteractable);
            }
            else
            {
                hoverAnimator.Play(Interactable);
            }
            
        }
        //else, play according animation
        else
        {
            hoverAnimator.Play(notInteractable);
        }
    }
    public void StopHover()
    {
        hoverAnimator.Play(notHovering);
    }
}
