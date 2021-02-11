using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
    private Player player;
    string savePath;
    // Start is called before the first frame update
    void Awake()
    {
        savePath = Application.persistentDataPath + "/gamesave.save";
    }

    // Update is called once per frame
    public void SaveData(SaveFile s)
    {
        var save = s;

        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
        Debug.Log(savePath);

        Debug.Log("Data Saved");
    }

    public SaveFile LoadData()
    {
        if (File.Exists(savePath))
        {
            SaveFile save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (SaveFile)binaryFormatter.Deserialize(fileStream);
            }
            Debug.Log(savePath);

            Debug.Log("Data Loaded");

            return save;

        }
        else
        {
            Debug.Log(savePath);

            Debug.LogWarning("Save file doesn't exist.");
            return null;
        }
    }
}
