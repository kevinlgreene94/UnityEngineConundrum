using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    
    // Start is called before the first frame update
   /* private void PlayerAttack(Enemy enemy)
    {
        if (enemy.attackable == true)
        {
            damageCoroutine = StartCoroutine(enemy.DamageCharacter(player.damageStrength, 1.0f));
        }
        
    }*/
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy != null)
        {
            if (enemy.attackable == true && player.isAttacking == true)
            {
                player.PlayerAttack(enemy);
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {   
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.attackable = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.attackable = false;
            player.PlayerStopAttack(enemy);
        }
    }
}
