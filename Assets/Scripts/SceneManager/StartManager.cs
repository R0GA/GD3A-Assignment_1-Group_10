using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //On the start button's on click event handler in the inspector, the scene to be loaded needs to be passed in to this funtion
    public void LoadnextScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
