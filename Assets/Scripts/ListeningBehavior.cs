using UnityEngine;

public class ListeningBehavior : MonoBehaviour
{
    // -- PARSING-RELATED --
    //this tracks what index we are parsing when going through playerResponses during a Listening segment.
    int parsingIndex = 0;
    //the base delay in frames between audio playback.
    int basePlaybackDelay = 30;
    //the max percentage variation in playback delay
    float maxPercentVariation = .25f;
    //tracks how many frames are left until the next playback. Starts at 0, set after a playback.
    int currentDelay = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if currently listening
            //and currentDelay is 0
                //parse the bool at parsingIndex to determine if the currentChar was correct or not
                //then parse the char at parsingIndex
                    //if correct, pass the parsed char through to VoicesBehavior PlayPositiveBabySound
                    //else pass the parsed char through to VoicesBehavior PlayNegativeBabySound
                    //finally...
                        //increase the parsing index to prep for next loop
                            //then, if the index is not equal to the COUNT of playerResponse
                                //set currentDelay to a variation of basePLaybackDelay
                            //else, mark currentlyListening in the gameManager as false
                            //and mark listeningComplete in the gameManager as true
    }
}
