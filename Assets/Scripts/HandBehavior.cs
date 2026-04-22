using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    // -- REFS --

    //ref to the singleton
    GameManagerBehavior gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
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

        }
        else if (gameManager.babyExtremism == 2)
        {

        }
        else if (gameManager.babyExtremism == 0)
        {

        }
        else if (gameManager.babyExtremism == -2)
        {

        }
        else if (gameManager.babyExtremism == -4)
        {

        }
    }
}
