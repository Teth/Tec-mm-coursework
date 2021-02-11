using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var saver = GameObject.FindGameObjectsWithTag("SaveObject")[0].GetComponent<SaveDataScript>();
        SceneManager.LoadScene(saver.LoadData().levelName);
    }

    

}
