using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionBehavior : MonoBehaviour
{
    //the string of the NAME of the scene the script will transition to ^o^
    public String sceneName;
    
    //this function loads the seccene that the scnne name is on the top of the yes on the editor thits its the one there
    //also handles transition aesthetics
    public void TransitionTo()
    {
        //cool animation here
        SceneManager.LoadScene(sceneName);
    }
}
