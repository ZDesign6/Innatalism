using UnityEngine;

public class TubSpriteWater : MonoBehaviour
{
    //sprite renderer displaying the tub water sprite
    SpriteRenderer waterSprite;
    //color of the water- determined based off extremism
    Color waterColor;
    //ref to the game manager
    GameManagerBehavior gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
        
        waterSprite = GetComponent<SpriteRenderer>();
        
        float extremismProportion = (float)gameManager.babyExtremism + 8f / 16f;
        waterColor = Color.Lerp(gameManager.blobColorRGB, gameManager.cloneColorRGB, extremismProportion);
        waterColor.a = 0.5f;
        waterSprite.color = waterColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
