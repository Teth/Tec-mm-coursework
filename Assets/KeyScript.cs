using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyScript : MonoBehaviour
{
    public string nextScene;
    public Vector2 spawn;
    // Start is called before the first frame update
    SaveDataScript saver;
    Player player;
    void Start()
    {
        saver = GameObject.FindGameObjectsWithTag("SaveObject")[0].GetComponent<SaveDataScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            SaveFile s = new SaveFile
            {
                stats = player.stats,
                playerClass = player.playerClass,
                xp = player.xp,
                lvl = player.level,
                currentSkill = player.skillPrimary ? player.skillPrimary.GetType() : null,
                currentWeapon = WeaponClass.getWeaponClassFromWeapon(player.weapon),
                x_WaypointLocation = spawn.x,
                y_WaypointLocation = spawn.y,
                levelName = nextScene
            };
            saver.SaveData(s);
            SceneManager.LoadScene("preLoadLevel");
        }
    }
}
