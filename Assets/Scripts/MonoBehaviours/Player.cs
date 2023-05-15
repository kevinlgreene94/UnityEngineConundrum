using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventoryPrefab;
    Inventory inventory;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isShooting = false;
    public bool facingLeft = false;
    public bool isDead = false;
    public bool isTouchingGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public int damageStrength;
    
    Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }
    // Start is called before the first frame update
    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }
    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
        healthBarPrefab.playerLives -= 1;


    }
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value <= float.Epsilon)
            {
                isDead = true;
                StartCoroutine(PlayerDeath());
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
    public IEnumerator PlayerDeath()
    {
        while(isDead == true)
        {
            yield return new WaitForSeconds(1);
            KillCharacter();
        }
    }
    public IEnumerator PlayerAttacking()
    {
        while (isAttacking == true)
        {
            yield return new WaitForSeconds(1.35f);
            isAttacking = false;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            StartCoroutine(PlayerAttacking());

        }
        if (Input.GetKey(KeyCode.E))
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
        

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            isDead = true;
            StartCoroutine(PlayerDeath());
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
            if (hitObject != null)
            {
                bool shouldDisappear = false;
                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    case Item.ItemType.LIFE:
                        shouldDisappear = AdjustLives(hitObject.quantity);
                        break;
                    default:
                        break;
                }
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }

            }

        }


    }
    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            healthBarPrefab.playerLives = healthBarPrefab.playerLives + amount;
            return true;
        }
        return false;
    }
    public bool AdjustLives(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            return true;
        }
        return false;
    }
    public void PlayerAttack(Enemy enemy)
    {
            if (isAttacking == true)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(damageStrength, 1.0f));
            }
    }
    public void PlayerStopAttack(Enemy enemy)
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
}
