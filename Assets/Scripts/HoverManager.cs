using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HoverManager : MonoBehaviour
{
    String Interactable = "Interactable";
    String notInteractable = "NotInteractableHover";
    String notHovering = "NotHovering";   
    public Animator hoverAnimator;
    
    //called when cursor hovers over object
    private void FixedUpdate()
    {
    
    }

    public void AnimateHover()
    {
        //if obj is interactable, play according animation
        if (this.gameObject.GetComponent<InteractibilityManager>().isInteractible == true)
        {
            hoverAnimator.Play(Interactable);
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
