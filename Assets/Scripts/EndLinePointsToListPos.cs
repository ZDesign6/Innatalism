using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EndLinePointsToListPos : MonoBehaviour
{
    //GRAB ALL LINE RENDERER COMPONENTS, IN ORDER

    //FOR EACH COMPONENT
    //ITERATE OVER ITS POINTS
    //ASSIGNING EACH VECTOR3 INTO THE CORRESPONDING INDEX OF THE CORRESPONDING LIST

    // -- REFS --

    //list of all Line Renderers attached to this object, in order
    List<LineRenderer> lineRendererComponents = new List<LineRenderer>();
    //shortcut to this object's FloatingLetterManager
    EndFloatingLetterManager letterManager;
    //ref to singleton
    GameManagerBehavior gameManager;


    // -- INFO --

    //the arbitrary z at which to instantiate floating letters
    private int startingZPos = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
        //abstract the FloatingLetterPoints 
        GameObject parentObj = GameObject.Find("FloatingLetterPoints");
        //iterate over the child objects...
        for (int childIndex = 0; childIndex < parentObj.GetComponent<Transform>().childCount; childIndex = childIndex + 1)
        {
            //adding their LineRendererComponents to the list
            lineRendererComponents.Add(parentObj.GetComponent<Transform>().GetChild(childIndex).gameObject.GetComponent<LineRenderer>());
        }
        //assign ref to this obj's floating letter manager
        letterManager = this.gameObject.GetComponent<EndFloatingLetterManager>();
        //for each line renderer...
        for (int componentIndex = 0; componentIndex < lineRendererComponents.Count; componentIndex = componentIndex + 1)
        {
            float currentZpos = startingZPos;
            //iterate over all of its points (if there are no points this will short circuit and not attempt an assignment)
            for (int pointIndex = 0; pointIndex < lineRendererComponents[componentIndex].positionCount; pointIndex = pointIndex + 1)

            {

                //abstract the current point of the current linerenderer for easy ref
                Vector3 currentPoint = lineRendererComponents[componentIndex].GetPosition(pointIndex);
                //overwrite using our arbitray zPos
                currentPoint = new Vector3(currentPoint.x, currentPoint.y, currentZpos);

                //if we are looking at first LineRenderer...
                if (componentIndex == 0)
                {
                    //assign the current point to the corresponding index of posDialogue1FloatingLetterPos
                    letterManager.posDialogue1FloatingLetterPos[pointIndex] = currentPoint;
                }
                //if we are looking at second LineRenderer...
                else if (componentIndex == 1)
                {
                    //assign the current point to the corresponding index of posDialogue2FloatingLetterPos
                    letterManager.posDialogue2FloatingLetterPos[pointIndex] = currentPoint;
                }
                //if we are looking at third LineRenderer...
                else if (componentIndex == 2)
                {
                    //assign the current point to the corresponding index of posDialogue3FloatingLetterPos
                    letterManager.posDialogue3FloatingLetterPos[pointIndex] = currentPoint;
                }
                //if we are looking at fourth LineRenderer...
                else if (componentIndex == 3)
                {
                    //assign the current point to the corresponding index of posDialogue4FloatingLetterPos
                    letterManager.posDialogue4FloatingLetterPos[pointIndex] = currentPoint;

                }
                //if we are looking at fifth LineRenderer...
                else if (componentIndex == 4)
                {
                    //assign the current point to the corresponding index of negDialogue1FloatingLetterPos
                    letterManager.negDialogue1FloatingLetterPos[pointIndex] = currentPoint;
                }
                //if we are looking at sixth LineRenderer...
                else if (componentIndex == 5)
                {
                    //assign the current point to the corresponding index of negDialogue2FloatingLetterPos
                    letterManager.negDialogue2FloatingLetterPos[pointIndex] = currentPoint;

                }
                //if we are looking at seventh LineRenderer...
                else if (componentIndex == 6)
                {
                    //assign the current point to the corresponding index of negDialogue3FloatingLetterPos
                    letterManager.negDialogue3FloatingLetterPos[pointIndex] = currentPoint;

                }
                //if we are looking at eigth LineRenderer...
                else if (componentIndex == 7)
                {
                    //assign the current point to the corresponding index of negDialogue4FloatingLetterPos
                    letterManager.negDialogue4FloatingLetterPos[pointIndex] = currentPoint;

                }
                //finally, change the currentZPos depending on whether the day's previous change was pos or neg
                if (gameManager.positiveChange == true)
                {
                    currentZpos = currentZpos + 0.1f;
                }
                else
                {
                    currentZpos = currentZpos - 0.1f;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
