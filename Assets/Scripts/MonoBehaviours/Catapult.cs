using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public GameObject ammoPrefab;
    public static List<GameObject> barrelPool;
    public int poolSize;
    public float weaponVelocity;
    Animator animator;
    bool isFired = false;
    bool canFire = false;
    Player player;

    void Awake()
    {
        if (barrelPool == null)
        {
            barrelPool = new List<GameObject>();
        }
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            barrelPool.Add(ammoObject);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(player != null)
        {
            if (player.isAttacking == true && canFire == true)
            {
                FireBarrel();
                animator.SetBool("isFired", !isFired);
            }
        }
        
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            canFire = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isFired", isFired);
            canFire = false;
        }
    }
    void FireBarrel()
    {
        Vector3 target = new Vector3(57, 1, 0);
        Vector3 spawnBarrel = new Vector3(-7.05f, 5.91f, 8.0020f);
        GameObject ammo = SpawnAmmo(spawnBarrel);
        if (ammo != null)
        {
            BarrelArc arcScript = ammo.GetComponent<BarrelArc>();
            float travelDuration = 1.0f / weaponVelocity;
            StartCoroutine(arcScript.TravelArc(target, travelDuration));
        }


    }
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in barrelPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }
    void OnDestroy()
    {
        barrelPool = null;

    }
}
