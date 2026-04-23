using UnityEngine;
using UnityEngine.SceneManagement;

public class BabySpriteChanger : MonoBehaviour
{
    // -- REFS --

    //ref to the singleton. Assigned during Start()
    GameManagerBehavior gameManager;
    //a two dimensional List that contains a List of all days, with each Day containing a List of each possible sprite for that day's possible extremisms.
    public Sprite[] babySprites = new Sprite[153];
    //a two dimensional List that contains a List of all days, with each Day containing a List of each possible material for that day's possible extremisms.
    public Material[] babyMats = new Material[153];
    //wobble shader
    public Shader wobbleShader;
    //current day index of sprite
    private int spriteIndex;

    public SpriteMask[] babyOutlineMask;
    public SpriteMask babyMask;
    public SpriteMask outlihebabyMask;
    public SpriteRenderer babySprite;
    public Animator babyOutlineAnimator;
    public Animator handAnim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;

        //-- SPRITE ASSIGNMENT --
        //convert currentDay into a base Index that represents what day's-worth of indices to check
        int baseIndex = (gameManager.currentDay - 1) * 17;
        //convert babyExtremism into an offset that represents which extremism index to check out of the day's-worth of indices
        int offsetIndex = (gameManager.babyExtremism + 8);
        //add the two together into a single day/extremism index to check
        spriteIndex = baseIndex + offsetIndex;
        //assign the Sprite equal to the Sprite at the spriteIndex
        babySprite.sprite = babySprites[spriteIndex];
        //assign the sprite material to default
        babySprite.material = babyMats[0];
        //outline sprite!
        for(int i = 0; i < 8; i ++)
        {
            babyOutlineMask[i].sprite = babySprites[spriteIndex];
        }

        babyMask.sprite = babySprites[spriteIndex];
        //and set this baby's pos equal to the pos of the background, so the two objects are centered on one another
        if (SceneManager.GetActiveScene().name == "NeutralEnding")
        {
            Debug.Log("DO NOT resize this baby!!!!");
        }
        else
        {
            this.gameObject.GetComponent<Transform>().position = GameObject.Find("Background").GetComponent<Transform>().position;
            //and set this baby's scale equal to the scale of the Background object
            this.gameObject.GetComponent<Transform>().localScale = GameObject.Find("Background").GetComponent<Transform>().localScale;
            //and then recalculate the PolygonCollider's points to match any new Sprite shapes
            this.gameObject.GetComponent<PolygonCollider2D>().CreateFromSprite(babySprite.sprite);
        }

    }

    private void OnMouseEnter()
    {
        
        try
        {
            //once mouse enters the collider, check if the baby is interactible. If it is, animate.
            if (this.gameObject.GetComponent<InteractibilityManager>().isInteractible == true)
            {
                AnimateBaby();
            }
        }
        //handling for edge cases where the sprite changer is being used without the baby being interactible:
        catch (System.NullReferenceException)
        {

        }

    }

    private void OnMouseExit()
    {
        StopBabyAnimation();
    }

    public void AnimateBaby()
    {
        //assign the wobble shader to the current sprite
        //TEMPORARILY REMOVED
        babySprite.gameObject.GetComponent<SpriteRenderer>().material = babyMats[spriteIndex];
        
    }

    public void StopBabyAnimation()
    {
        //if not currently listening
        if (!gameManager.currentlyListening)
        {
            //assign the wobble shader to not wiggle
            babySprite.gameObject.GetComponent<SpriteRenderer>().material = babyMats[0];
        }
    }
}
