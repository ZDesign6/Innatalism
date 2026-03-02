using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MouseCursorBehavior : MonoBehaviour
{
    //game manager
    private GameManagerBehavior gameManager;
    private Transform mouseTransform;
    private Animator mouseAnimator;
    
    public List<GameObject> objectHitList;
    void Start()
    {
        gameManager = GameManagerBehavior.singleton;
        mouseTransform = this.gameObject.GetComponent<Transform>();
        mouseAnimator = this.gameObject.GetComponent<Animator>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseTransform.position = gameManager.mouseInWorldSpace;

        fireRay();

        // -- ANIMATION STATES --

        //if we are currently listening, short circuit to Listening Animation
        if (gameManager.currentlyListening == true)
        {
            // The mouse cursor plays the listening animation
            mouseAnimator.Play("Listen Animation");
        }
        //else if we are currently typing, short circuit to typing animation
        else if (gameManager.currentlyTyping == true)
        {
            // The mouse cursor plays the speaking animation
            mouseAnimator.Play("Speak Animation");
        }
        //else, if we are NOT listening and NOT typing, if we ARE hovering over an interactible object...
        else if (objectHitList.Count > 0 && objectHitList[0].GetComponent<InteractibilityManager>() != null)
        {
            Debug.Log("Mouse cursor's ray collided with " + objectHitList[0].name);
            //if the object is interactible
            if (objectHitList[0].GetComponent<InteractibilityManager>().isInteractible == true)
            {
                //meow
                var hoverManager = objectHitList[0].GetComponent<HoverManager>();
                hoverManager.AnimateHover(true);
                
                // The mouse cursor plays the interacting animation
                mouseAnimator.Play("Interact Animation");
            }
            //if the object is not interactible
            else
            {
                var hoverManager = objectHitList[0].GetComponent<HoverManager>();
                hoverManager.AnimateHover(false);
                //mouse cursor plays the not interactible animation
                mouseAnimator.Play("No Interact Animation");
            }
        }
        //else, we are NOT listening, NOT typing, and NOT hovering. So play Neutral animation.
        else
        {
            // The mouse cursor plays the neutral animation
            mouseAnimator.Play("Neutral Animation");
        }
 
        // -- CLEANUP --
        Cleanup();
    }
    
    // Fires a ray from the mouse and detects collision
    void fireRay()
    {
        //Cast a Ray and store data about what it hit
        RaycastHit2D hit = Physics2D.Raycast(mouseTransform.position, new Vector3(0,0,1));

        // If the ray did indeed hit something
        if (hit == true)
        {
            // The object that is hit is added to the object hit list
            objectHitList.Add(hit.transform.GameObject());
        }
    }
    void Cleanup()
    {
        //empty the objectHitList
        objectHitList.Clear();
    }
}
