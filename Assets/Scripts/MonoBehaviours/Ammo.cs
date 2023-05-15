using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageInflicted;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision is BoxCollider2D)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
        }
    }
}
