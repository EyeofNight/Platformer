using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

    public GameObject canvas;
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            canvas.SetActive(true);
        }
	}

    public void Resume()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
    }

    public void Options()
    {
        //Placeholder
    }

    public void Exit()
    {
        Application.Quit();
    }
}
