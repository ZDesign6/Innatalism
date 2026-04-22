using TMPro;
using UnityEngine;

public class FloatingLetterBehavior : MonoBehaviour
{
    // -- PUBLIC --

    //tracks if this floatingLetter was accurate. set by FloatingLetterManager when it instantiates this Floating Letter.
    public bool isAccurate = true;
    //how long Floating Characters last befoer deleting themselves
    public int lifeInFrames = 475;
    //where this letter's homePos is
    public Vector3 homePos;

    // -- MOUSE REPULSION --

    //reference to singleton
    GameManagerBehavior gameManager;
    //max distance at which repulsion can occur
    int maxDifference= 2;

    // -- "ANIMATION" --
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if this letter was accurate, set its vertex color to clone color
        if (isAccurate == true)
        {
            this.gameObject.GetComponent<TextMeshPro>().color = new Color(.969f, .580f, .286f, 1);
            animator.Play("FloatingIdle");
        }
        //else set its vertex color to blob color
        else
        {
            this.gameObject.GetComponent<TextMeshPro>().color = new Color(.839f, .286f, .584f, 1);
            animator.Play("FloatingTurn");
        }
        //asssign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //every frame, subtract one from lifeInFrames
        lifeInFrames = lifeInFrames - 1;
        //if life in frames ever goes below one, delete self
        if (lifeInFrames <= 50)
        {
            animator.Play("FloatingExit");
            if (lifeInFrames <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        // REPULSION
        MouseRepulsion();

    }
    //this fct provides force to the Floating Letter when the mouse gets to close
    void MouseRepulsion()
    {
        //Find the difference in x and y between the Letter and the Mouse Pos
        Vector2 mouseLetterDifference = new Vector2(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y) - gameManager.mouseInWorldSpace;
        //then, abstract the total difference in position
        float currentDifference = Mathf.Abs(mouseLetterDifference.x) + Mathf.Abs(mouseLetterDifference.y);
        Debug.Log("current distance from mouse is " +  currentDifference);

        //if distance between mousePos and this letter is less than the max distance
        if (currentDifference <= maxDifference)
        {
            //then add force to the letter, with that force being equivalent to a baseForce divided by (totalDifference / maxDifference)

        }
    }
}
