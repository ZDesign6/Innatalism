using UnityEngine;
using System.Collections.Generic;

public class DrippingController : MonoBehaviour
{
	//ref to game manager singleton
	GameManagerBehavior gameManager;
	
	//The Audio Source on the drip sfx obj
    AudioSource audioSource;
	
	//array of drip audio clips
	public AudioClip[] drips;
	
	int dripToPlay;
	
	int currentDay;
	
	int dripTimer, dripTimeMax, dripTimeMin;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the obj's audio source
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //assign ref to singleton
        gameManager = GameManagerBehavior.singleton;
		
		//drips = new AudioClip[9];
		currentDay = gameManager.currentDay;
		
		switch (currentDay)
		{
			case 4:
				dripTimeMax = 350;
				dripTimeMin = 250;
				break;
			case 5:
				dripTimeMax = 250;
				dripTimeMin = 170;
				break;
			case 6:
				dripTimeMax = 170;
				dripTimeMin = 100;
				break;
			case 7:
				dripTimeMax = 100;
				dripTimeMin = 60;
				break;
			case 8:
				dripTimeMax = 60;
				dripTimeMin = 30;
				break;
			case 9:
				dripTimeMax = 30;
				dripTimeMin = 10;
				break;
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (dripTimer <= 0)
		{
			dripToPlay = Random.Range(0,9);
			audioSource.PlayOneShot(drips[dripToPlay]);
			dripTimer = Random.Range(dripTimeMin,dripTimeMax);
		}
        dripTimer--;
    }
}
