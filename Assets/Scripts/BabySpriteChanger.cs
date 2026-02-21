using UnityEngine;
using System.Collections.Generic;
using System;

public class BabySpriteChanger : MonoBehaviour
{
    // -- REFS --

    //ref to the singleton. Assigned during Start()
    GameManagerBehavior gameManager;
    //a two dimensional List that contains a List of all days, with each Day containing a List of each possible sprite for that day's possible extremisms.
    public Sprite[] babySprites = new Sprite[153];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;

        //-- SPRITE ASSIGNEMENT --

        //convert currentDay into a base Index that represents what day's-worth of indices to check
        int baseIndex = (gameManager.currentDay - 1) * 17;
        //convert babyExtremism into an offset that represents which extremism index to check out of the day's-worth of indices
        int offsetIndex = (gameManager.babyExtremism + 8);
        //add the two together into a single day/extremism index to check
        int spriteIndex = baseIndex + offsetIndex;
        //assign the Sprite equal to the Sprite at the spriteIndex
        this.gameObject.GetComponent<SpriteRenderer>().sprite = babySprites[spriteIndex];
        //and then recalculate the PolygonCollider's points to match any new Sprite shapes
        this.gameObject.GetComponent<PolygonCollider2D>().CreateFromSprite(this.gameObject.GetComponent<SpriteRenderer>().sprite);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
