using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventoryPrefab;
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Instantiate(inventoryPrefab);

        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        healthBar.hitPoints.value = startingHitPoints;
    }

    // Update is called once per frame
    void Update()
    {

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
        if (healthBar.hitPoints.value < maxHitPoints)
        {
            healthBar.hitPoints.value = healthBar.hitPoints.value + amount;
            print("Adjusted HP by: " + amount + ". New value: " + healthBar.hitPoints.value);
            return true;
        }
        return false;
    }
}
