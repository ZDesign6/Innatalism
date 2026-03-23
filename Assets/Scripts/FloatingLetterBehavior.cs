using TMPro;
using UnityEngine;

public class FloatingLetterBehavior : MonoBehaviour
{
    // -- PUBLIC --

    //tracks if this floatingLetter was accurate. set by FloatingLetterManager when it instantiates this Floating Letter.
    public bool isAccurate = true;
    //how long Floating Characters last befoer deleting themselves
    public int lifeInFrames = 600;
    //where this letter's destination is
    public Vector3 destinationPos;

    // -- "ANIMATION" --

    //tracks if we have reached our destinationPos. Starts on, turned off when we reach the pos.
    public bool reachedDestinationPos = false;
    //controls the minumum distance from destinationPos we can be before we are marked as having "reached" it
    public float maxPosDeviation = .1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if this letter was accurate, set its vertex color to clone color
        if (isAccurate == true)
        {
            this.gameObject.GetComponent<TextMeshPro>().color = new Color(.969f, .580f, .286f, 1);
        }
        //else set its vertex color to blob color
        else
        {
            this.gameObject.GetComponent<TextMeshPro>().color = new Color(.839f, .286f, .584f, 1);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if we have reached our destination
        if (reachedDestinationPos == true)
        {
            //every frame, subtract one from lifeInFrames
            lifeInFrames = lifeInFrames - 1;
            //if life in frames ever goes below one, delete self
            if (lifeInFrames <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        //if we have not yet reached our destination
        else
        {
            //move towards destination pos
            this.gameObject.GetComponent<RectTransform>().position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().position, destinationPos, .1f);
            //abstract the maxAcceptablePos
            Vector3 maxAcceptablePos = new Vector3(destinationPos.x + maxPosDeviation, destinationPos.y + maxPosDeviation, destinationPos.z + maxPosDeviation);
            //abstract the minAcceptablePos
            Vector3 minAcceptablePos = new Vector3(destinationPos.x - maxPosDeviation, destinationPos.y -  maxPosDeviation, destinationPos.z - maxPosDeviation);
            //and if we arrive within the acceptable deviation of destinationPos, turn on reachedDestinationPos
            if (this.gameObject.GetComponent<RectTransform>().position.x <= maxAcceptablePos.x && this.gameObject.GetComponent<RectTransform>().position.x >= minAcceptablePos.x 
                && this.gameObject.GetComponent<RectTransform>().position.y <= maxAcceptablePos.y && this.gameObject.GetComponent<RectTransform>().position.y >= minAcceptablePos.y
                && this.gameObject.GetComponent<RectTransform>().position.z <= maxAcceptablePos.z && this.gameObject.GetComponent<RectTransform>().position.z >= minAcceptablePos.z)
            {
                reachedDestinationPos = true;
            }
        }
    }
}
