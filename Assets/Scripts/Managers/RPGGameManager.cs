using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RPGGameManager : MonoBehaviour
{
    public SpawnPoint playerSpawnPoint;
    public static RPGGameManager sharedInstance = null;
    public RPGCameraManager cameraManager;
    GameObject player;
    public HealthBar healthBar;
    public Player playerObject;

    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            // We only ever want one instance to exist, so destroy the other existing object
            Destroy(gameObject);
        }
        else
        {
            // If this is the only instance, then assign the sharedInstance variable to the current object.
            sharedInstance = this;
        }
    }

    void Start()
    {
        // Consolidate all the logic to setup a scene inside a single method. 
        // This makes it easier to call again in the future, in places other than the Start() method.
        

    }
    void Update()
    {
        if(player == null)
        {
            SpawnPlayer();
            
        }
        
    }

   

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null && healthBar.playerLives > 0)
        {
            player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;
        }
        if(healthBar.playerLives == 0)
        {
            SceneManager.LoadScene("GameOver");
            healthBar.playerLives = 5;
            playerObject.isDead = true;
        }
    }
    

}
