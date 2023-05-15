using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HitPoints hitPoints;

    [HideInInspector]
    public Character character;
    
    public Image meterImage;
    public Text livesText;
    public Player player;
    public int playerLives = 5;
    float maxHitPoints;

    void Start()
    {
        if(character != null)
        {
            maxHitPoints = character.maxHitPoints;
        }
        
    }

    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;
            livesText.text = "Lives: " + playerLives;
        }
    }
}
