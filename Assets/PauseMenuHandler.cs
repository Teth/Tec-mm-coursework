using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    Canvas c;
    void Start()
    {
        c = GetComponent<Canvas>();
        c.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            c.enabled = !c.enabled;
            if(c.enabled == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void Load()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("preLoadLevel");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("menu");
    }

    public void Settings()
    {

    }
}