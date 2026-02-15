using UnityEngine;

public class MouseCursorBehavior : MonoBehaviour
{
    //game manager
    private GameManagerBehavior gameManager;
    private Transform mouseTransform;
    void Start()
    {
        gameManager = GameManagerBehavior.singleton;
        mouseTransform = this.gameObject.GetComponent<Transform>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseTransform.position = gameManager.mouseInWorldSpace;
    }
}
