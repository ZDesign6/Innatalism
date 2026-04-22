using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    // -- REFS --

    //ref to the singleton
    GameManagerBehavior gameManager;
    
    //list of hand holding baby sprites
    public List<Sprite> handHoldingClaySprites;
    //list of hand squishing baby sprites
    public List<Sprite> handSquishingClaySprites;

    //actual sprites to display, determined based on extremism on start
    Sprite currentHoldingSprite;
    Sprite currentSquishingSprite;

    SpriteRenderer handSR;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
        //get sprite renderer component off the hand
        handSR = GetComponent<SpriteRenderer>();
        //set sprite according to extremism
        ChangeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //this fct changes the Hand's Sprite based on the Baby's current extremism
    void ChangeSprite()
    {
        if (gameManager.babyExtremism == 4)
        {
            currentSquishingSprite = handSquishingClaySprites[4];
            currentHoldingSprite = handHoldingClaySprites[4];
        }
        else if (gameManager.babyExtremism == 2)
        {
            currentSquishingSprite = handSquishingClaySprites[3];
            currentHoldingSprite = handHoldingClaySprites[3];
        }
        else if (gameManager.babyExtremism == 0)
        {
            currentSquishingSprite = handSquishingClaySprites[2];
            currentHoldingSprite = handHoldingClaySprites[2];
        }
        else if (gameManager.babyExtremism == -2)
        {
            currentSquishingSprite = handSquishingClaySprites[1];
            currentHoldingSprite = handHoldingClaySprites[1];
        }
        else if (gameManager.babyExtremism == -4)
        {
            currentSquishingSprite = handSquishingClaySprites[0];
            currentHoldingSprite = handHoldingClaySprites[0];
        }
        
    }

    //called from end typing behavior to switch sprite to the baby held
    public void SquishBaby()
    {
        handSR.sprite = currentSquishingSprite;
    }

    //called from end typing behavior to switch sprite to the baby squish
    public void HoldBaby()
    {
        handSR.sprite = currentHoldingSprite;
    }
    
    
}
