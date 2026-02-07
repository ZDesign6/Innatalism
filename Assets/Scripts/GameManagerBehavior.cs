using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerBehavior : MonoBehaviour
{
    //this static variable holds a single persistent instance of the GameManagerBehavior script
    public static GameManagerBehavior singleton;

    // -- MOUSE INFORMATION --

    //stores the computed location of the mouse in world space
    public Vector2 mouseInWorldSpace;
    //awake runs as soon as the object the script is on is instantiated
    private void Awake()
    {
        //If there is no singleton stored...
        if (singleton == null) 
        {
            //mark this instance as the singleton
            singleton = this;
        }
        //else, if there is a singleton...
        else
        {
            //and it is NOT this instance
            if (singleton != this)
            {
                //destroy this entire gameManger gameObject
                Destroy(singleton);
            }
        }
        //If we made it this far without being destroyed, then this object is carries the singleton. Mark it as Don'tDestroy
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //every frame, compute the mouse's world pos and store it
        mouseInWorldSpace = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
