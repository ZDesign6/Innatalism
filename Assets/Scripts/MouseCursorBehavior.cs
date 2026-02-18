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

        //if we aren't currently listening or talking
        if (gameManager.currentlyListening == false && gameManager.currentlyTyping == false)
        {
            // Fires a raycasting ray from the mouse pos every frame and detects collision
            fireRay();
        }
        //else if we are currently listening
        else if (gameManager.currentlyListening == true)
        {
            // The mouse cursor plays the listening animation
            mouseAnimator.Play("Listen Animation");
        }
        //else if we are currently typing
        else if (gameManager.currentlyTyping == true)
        {
            // The mouse cursor plays the speaking animation
            mouseAnimator.Play("Speak Animation");
        }
    }
    
    // Fires a ray from the mouse and detects collision
    void fireRay()
    {
        //A boolean that is true if the ray shot from the mouse position is hitting something
        RaycastHit2D hit = Physics2D.Raycast(mouseTransform.position, -Vector2.up);

        // If the ray is hitting something
        if (hit)
        {
            // The object that is hit is added to the object hit list
            objectHitList.Add(hit.transform.GameObject());

            // If the object hit has transition on click behavior (interactable object)
            // **For now detects transition on click but can be changed
            if (objectHitList[0].GetComponent<TransitionOnClickBehavior>() != null)
            {
                // **Insert getting values in here or something
                // **If interactable does interact animation, else does no interact animation yay
                
                // The mouse cursor plays the interacting animation
                mouseAnimator.Play("Interact Animation");
            }
            // If the object being hit is not interactable
            else
            {
                // The mouse cursor plays the neutral animation
                mouseAnimator.Play("Neutral Animation");
            }
            
            // The object that is hit is then removed from the object hit list
            objectHitList.Remove(hit.transform.GameObject());
        }
    }
}
