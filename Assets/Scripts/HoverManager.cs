using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HoverManager : MonoBehaviour
{
    String Interactable = "Hover";
    String notInteractable = "NotInteractableHover";
    String notHovering = "NotHovering";   
    public Animator hoverAnimator;
    
    //called when cursor hovers over object
    private void FixedUpdate()
    {
    
    }

    public void AnimateHover(bool isInteractable)
    {
        //if obj is interactable
        if (isInteractable)
        {
            hoverAnimator.Play(Interactable);
        }
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
