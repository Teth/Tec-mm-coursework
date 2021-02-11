using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wingame : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text text;
    void Start()
    {
        //text = GetComponent<TMPro>();
        StartCoroutine(showScreen());
    }

    IEnumerator showScreen()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(LoadYourAsyncScene());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("menu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

