using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        //Placeholder
    }

    public void Quit()
    {
        Application.Quit();
    }

}
