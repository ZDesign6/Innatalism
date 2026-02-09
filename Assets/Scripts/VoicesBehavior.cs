using System.Collections.Generic;
using UnityEngine;

public class VoicesBehavior : MonoBehaviour
{
    /*THIS SCRIPT LIVES ON A "VOICES" OBJECT, AND ACTS A "LIVING" SOUNDBANK FOR ALL VOICES THAT SHOULD BE PLAYED IN THE GIVEN SCENE.
     IT IS RESPONSIBLE FOR MANAGING AND PLAYING ALL VOICE-RELATED SOUNDS. ITS FUNCTIONS ALLOW OTHER SCRIPTS TO USE ITS BEHAVIORS.*/

    // -- REFS --
    //this is a ref to the active DialogueTypingBehavior. Since there is only one in a scene, it will assign itself to this var when it finds this script.
    public DialogueTypingBehavior typingScript;
    //ref to the singleton
    GameManagerBehavior gameManager;

    // -- PHONEMES --
    //these Lists represent which chars map to which phonemes. Used by the charToPhoneme fct to determine which phoneme index to return.
    List<char> ahChars = new List<char>()
    {
        //lowercase
        'a','d','g','j','m','p','s','v','y',
        //caps
        'A','D','G','J','M','P','S','V','Y',
        //symbols
        '.','?',';','/','(',
    };
    List<char> ehChars = new List<char>()
    {
        //lowercase
        'b','e','h','k','n','q','t','w','z',
        //caps
        'B','E','H','K','N','Q','T','W','Z',
        //symbols
        ',','\'',':','\\',')'
    };
    List<char> ohChars = new List<char>()
    {
        //lowercase
        'c','f','i','l','o','r','u','x',
        //caps
        'C','F','I','L','O','R','U','X',
        //symbols
        ' ', '!','\"','&','-',
    };
    // -- SOUNDS --
    //The Audio Source on the Voices obj
    AudioSource audioSource;
    //variables which store adjustable minimum and maximum ranges for pitch randomization during playback for man-related voices
    [SerializeField][Range(.5f, 1.5f)] private float manMinPitch, manMaxPitch;
    //variables which store adjustable minimum and maximum ranges for pitch randomization during playback for baby-related voices
    [SerializeField][Range(.5f, 1.5f)] private float babyMinPitch, babyMaxPitch;


    // NOTE: ALL SOUNDS ARE STORED IN THIS ORDER: 'AH' AT INDEX 0, 'EH' AT INDEX 1, 'OH' AT INDEX 2

    // -- MAN SOUNDS --
    //List of all the positive sounds that the man can make
    public List<AudioClip> manPositiveNoises = new List<AudioClip>(3);
    //list of all the negative sounds that the man can make
    public List<AudioClip> manNegativeNoises = new List<AudioClip>(3);
    // -- BABY SOUNDS --
    //list of pos sounds at -8 extremism
    public List<AudioClip> babyNeg8PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -8 extremism
    public List<AudioClip> babyNeg8NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -7 extremism
    public List<AudioClip> babyNeg7PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -7 extremism
    public List<AudioClip> babyNeg7NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -6 extremism
    public List<AudioClip> babyNeg6PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -6 extremism
    public List<AudioClip> babyNeg6NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -5 extremism
    public List<AudioClip> babyNeg5PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -5 extremism
    public List<AudioClip> babyNeg5NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -4 extremism
    public List<AudioClip> babyNeg4PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -4 extremism
    public List<AudioClip> babyNeg4NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -3 extremism
    public List<AudioClip> babyNeg3PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -3 extremism
    public List<AudioClip> babyNeg3NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -2 extremism
    public List<AudioClip> babyNeg2PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -2 extremism
    public List<AudioClip> babyNeg2NegNoises = new List<AudioClip>(3);
    //list of pos sounds at -1 extremism
    public List<AudioClip> babyNeg1PosNoises = new List<AudioClip>(3);
    //list of neg sounds at -1 extremism
    public List<AudioClip> babyNeg1NegNoises = new List<AudioClip>(3);
    //list of positive sounds at 0 extremism
    public List<AudioClip> baby0PosNoises = new List<AudioClip>(3);
    //list of neg sounds at 0 extremism
    public List<AudioClip> baby0NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +1 extremism 
    public List<AudioClip> baby1PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +1 extremism
    public List<AudioClip> baby1NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +2 extremism
    public List<AudioClip> baby2PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +2 extremism
    public List<AudioClip> baby2NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +3 extremism
    public List<AudioClip> baby3PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +3 extremism
    public List<AudioClip> baby3NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +4 extremism
    public List<AudioClip> baby4PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +4 extremism
    public List<AudioClip> baby4NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +5 extremism
    public List<AudioClip> baby5PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +5 extremism
    public List<AudioClip> baby5NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +6 extremism
    public List<AudioClip> baby6PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +6 extremism
    public List<AudioClip> baby6NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +7 extremism
    public List<AudioClip> baby7PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +7 extremism
    public List<AudioClip> baby7NegNoises = new List<AudioClip>(3);
    //list of pos sounds at +8 extremism
    public List<AudioClip> baby8PosNoises = new List<AudioClip>(3);
    //list of neg sounds at +8 extremism
    public List<AudioClip> baby8NegNoises = new List<AudioClip>(3);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the obj's audio source
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //this fct parses a phoneme out of a given char, in the form of an index used to retrieve that phoneme from one of our Sound Lists. Returns 0 for 'ah' chars, 1 for 'eh' chars, 2 'oh' chars.
    int charToPhonemeIndex(char charToParse)
    {
        //if the ahChars contains the given char
        if (ahChars.Contains(charToParse)) 
        {
            //return an 'ah' Index
            return 0;
        }
        //if the ehChars contains the given char
        else if (ehChars.Contains(charToParse)) 
        {
            //return an 'eh' Index
            return 1;
        }
        //if the ohChars contains the given char
        else if (ohChars.Contains(charToParse))
        {
            //return an 'oh' Index
            return 2;
        }
        //we should never reach here
        else
        {
            //return some bullshit so it throws errors
            return 99;
        }
    }

    //this fct plays a "positive" sound clip. The exact clip it plays is determined by the last input character.
    public void PlayPositiveManSound(char charToParse)
    {
        //stop any previous clip to prevent overlap
        audioSource.Stop();
        //randomize the pitch within the manMinPitch and manMaxPitch
        audioSource.pitch = Random.Range(manMinPitch, manMaxPitch);
        //convert parsed char into a phoneme index
        int phonemeIndex = charToPhonemeIndex(charToParse);
        //then, play the phoneme from the Soundbank
        audioSource.PlayOneShot(manPositiveNoises[phonemeIndex]);
        Debug.Log("Played a Positive Man Sound");
    }
    //this fct plays a "negative" sound clip THe exact clip it palhys is determined by the last input character.
    public void PlayNegativeManSound(char charToParse)
    {
        //stop any previous clip to prevent overlap
        audioSource.Stop();
        //randomize the pitch within the manMinPitch and manMaxPitch
        audioSource.pitch = Random.Range(manMinPitch, manMaxPitch);
        //convert the parsed char into a phoneme index
        int phonemeIndex = charToPhonemeIndex(charToParse);
        //then, play the phoneme from the Soundbank
        audioSource.PlayOneShot(manPositiveNoises[phonemeIndex]);
        Debug.Log("Played a Negative Man Sound");
    }
    //this fct plays a "positive" baby sound clip. The exact clip it plays is determined by the character currently being parsed from the player's response, and by the baby's current Extremism level.
    //should only ever be called during Listening
    public void PlayPositiveBabySound(char charToParse)
    {
        //stop any previous clip to prevent overlap
        audioSource.Stop();
        //randomize the pitch within the manMinPitch and manMaxPitch
        audioSource.pitch = Random.Range(manMinPitch, manMaxPitch);
        //convert the parsed char into a phoneme index
        int phonemeIndex = charToPhonemeIndex(charToParse);
        //then, accordingly with the baby's extremism...
        switch (gameManager.babyExtremism)
        {
            //play the phoneme from the appropriate Soundbank
            case -8:
                audioSource.PlayOneShot(babyNeg8PosNoises[phonemeIndex]);
                break;
            case -7:
                audioSource.PlayOneShot(babyNeg7PosNoises[phonemeIndex]);
                break;
            case -6:
                audioSource.PlayOneShot(babyNeg6PosNoises[phonemeIndex]);
                break;
            case -5:
                audioSource.PlayOneShot(babyNeg5PosNoises[phonemeIndex]);
                break;
            case -4:
                audioSource.PlayOneShot(babyNeg4PosNoises[phonemeIndex]);
                break;
            case -3:
                audioSource.PlayOneShot(babyNeg3PosNoises[phonemeIndex]);
                break;
            case -2:
                audioSource.PlayOneShot(babyNeg2PosNoises[phonemeIndex]);
                break;
            case -1:
                audioSource.PlayOneShot(babyNeg1PosNoises[phonemeIndex]);
                Debug.Log("Played a Positive Baby Sound for extremism -1");

                break;
            case 0:
                audioSource.PlayOneShot(baby0PosNoises[phonemeIndex]);
                Debug.Log("Played a Positive Baby Sound for extremism 0");
                break;
            case 1:
                audioSource.PlayOneShot(baby1PosNoises[phonemeIndex]);
                Debug.Log("Played a Positive Baby Sound for extremism 1");

                break;
            case 2:
                audioSource.PlayOneShot(baby2PosNoises[phonemeIndex]);
                break;
            case 3:
                audioSource.PlayOneShot(baby3PosNoises[phonemeIndex]);
                break;
            case 4:
                audioSource.PlayOneShot(baby4PosNoises[phonemeIndex]);
                break;
            case 5:
                audioSource.PlayOneShot(baby5PosNoises[phonemeIndex]);
                break;
            case 6:
                audioSource.PlayOneShot(baby6PosNoises[phonemeIndex]);
                break;
            case 7:
                audioSource.PlayOneShot(baby7PosNoises[phonemeIndex]);
                break;
            case 8:
                audioSource.PlayOneShot(baby8PosNoises[phonemeIndex]);
                break;
        }
    }
    public void PlayNegativeBabySound(char charToParse)
    {
        //stop any previous clip to prevent overlap
        audioSource.Stop();
        //randomize the pitch within the manMinPitch and manMaxPitch
        audioSource.pitch = Random.Range(manMinPitch, manMaxPitch);
        //convert the parsed char into a phoneme index
        int phonemeIndex = charToPhonemeIndex(charToParse);
        //then, accordingly with the baby's extremism...
        switch (gameManager.babyExtremism)
        {
            //play the phoneme from the appropriate Soundbank
            case -8:
                audioSource.PlayOneShot(babyNeg8NegNoises[phonemeIndex]);
                break;
            case -7:
                audioSource.PlayOneShot(babyNeg7NegNoises[phonemeIndex]);
                break;
            case -6:
                audioSource.PlayOneShot(babyNeg6NegNoises[phonemeIndex]);
                break;
            case -5:
                audioSource.PlayOneShot(babyNeg5NegNoises[phonemeIndex]);
                break;
            case -4:
                audioSource.PlayOneShot(babyNeg4NegNoises[phonemeIndex]);
                break;
            case -3:
                audioSource.PlayOneShot(babyNeg3NegNoises[phonemeIndex]);
                break;
            case -2:
                audioSource.PlayOneShot(babyNeg2NegNoises[phonemeIndex]);
                break;
            case -1:
                audioSource.PlayOneShot(babyNeg1NegNoises[phonemeIndex]);
                Debug.Log("Played a Negative Baby Sound for extremism -1");

                break;
            case 0:
                audioSource.PlayOneShot(baby0PosNoises[phonemeIndex]);
                Debug.Log("Played a Negative Baby Sound for extremism 0");
                break;
            case 1:
                audioSource.PlayOneShot(baby1NegNoises[phonemeIndex]);
                Debug.Log("Played a Negative Baby Sound for extremism 1");

                break;
            case 2:
                audioSource.PlayOneShot(baby2NegNoises[phonemeIndex]);
                break;
            case 3:
                audioSource.PlayOneShot(baby3NegNoises[phonemeIndex]);
                break;
            case 4:
                audioSource.PlayOneShot(baby4NegNoises[phonemeIndex]);
                break;
            case 5:
                audioSource.PlayOneShot(baby5NegNoises[phonemeIndex]);
                break;
            case 6:
                audioSource.PlayOneShot(baby6NegNoises[phonemeIndex]);
                break;
            case 7:
                audioSource.PlayOneShot(baby7NegNoises[phonemeIndex]);
                break;
            case 8:
                audioSource.PlayOneShot(baby8NegNoises[phonemeIndex]);
                break;
        }
    }
    /*This fct iterates over all the chars typed by the player in response to a dialogue.
     * It plays a pos or neg baby sound for each, according to if the char matched the intended char.
     * The set of clips */
}
