using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    static List<GameObject> ammoPool;
    public int poolSize;
    public float weaponVelocity;
    Vector3 target;
    Player player;

    void Awake()
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }
        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.isTouchingGround == true)
        {
            FireAmmo();
            var t = Task.Run(async delegate
            {
                player.isShooting = true;
                await Task.Delay(800);
                return player.isShooting = false;
            });
        }
    }
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }
    void FireAmmo()
    {
        if(player.facingLeft == true)
        {
            target = new Vector3(transform.position.x - 20, transform.position.y + 2, 0);
        }
        else
        {
            target = new Vector3(transform.position.x + 20, transform.position.y + 2, 0);
        }
        
        GameObject ammo = SpawnAmmo(transform.position);
        if(ammo != null)
        {
            if(target.x < transform.position.x)
            {
                ammo.transform.rotation = Quaternion.Euler(0, 180, 0);
                player.facingLeft = true;
            }
            else
            {
                ammo.transform.rotation = Quaternion.identity;
                player.facingLeft = false;
            }
            if (ammo != null)
            {
                Arc arcScript = ammo.GetComponent<Arc>();
                float travelDuration = 1.0f / weaponVelocity;
                StartCoroutine(arcScript.TravelArc(target, travelDuration));
            }
        }
        

    }
    void OnDestroy()
    {
        ammoPool = null;
        
    }
}
