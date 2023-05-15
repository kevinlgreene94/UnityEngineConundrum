using UnityEngine;

public class Barrel : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageInflicted;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D)
        {
            EnemyWall enemy = collision.gameObject.GetComponent<EnemyWall>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
        }
    }
}
