using System.Collections.Generic;
using UnityEngine;

public class VoicesBehavior : MonoBehaviour
{
    /*THIS SCRIPT LIVES ON A "VOICES" OBJECT, AND ACTS A "LIVING" SOUNDBANK FOR ALL VOICES THAT SHOULD BE PLAYED IN THE GIVEN SCENE*/

    // NOTE: ALL SOUNDS ARE STORED IN THIS ORDER: 'AH' AT INDEX 0, 'EH' AT INDEX 1, 'OH' AT INDEX 2

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
