using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public HealthBar healthBar;
    public Player playerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Level1");
            playerObject.isDead = false;
            if (healthBar.playerLives != 5)
            {
                healthBar.playerLives = 5;
            }
        }
    }
}
