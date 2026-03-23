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
        //every frame, subtract one from lifeInFrames
        lifeInFrames = lifeInFrames - 1;
        //if life in frames ever goes below one, delete self
        if (lifeInFrames <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
