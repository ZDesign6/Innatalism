using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterableTubBehavior : MonoBehaviour
{
    // -- REFERERENCES --

    //variable which holds the Collider with which we will check collision
    Collider2D colliderToCheck;
    //string representing the name of the scene to load
    public string sceneName;
    //ref to singleton
    GameManagerBehavior gameManager;

    private void Start()
    {
        //assign ref to the singleton
        gameManager = GameManagerBehavior.singleton;
        //feftch the first component that falls unyder the collider2D parent class
        colliderToCheck = this.gameObject.GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
    }
    private void OnMouseDown()
    {
        //And if we aren't currently listening, talking, or have already listened
        if (gameManager.currentlyListening == false && gameManager.currentlyTyping == false && gameManager.listeningComplete == false)
        {
            //load the specified scene
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("transition denied.");

        }
    }
}
