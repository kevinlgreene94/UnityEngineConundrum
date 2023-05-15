using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : MonoBehaviour
{
    public Player player;
    PolygonCollider2D blockArea;
    // Start is called before the first frame update
    void Start()
    {
        blockArea = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isBlocking == true)
        {
            blockArea.enabled = true;
        }
        else
        {
            blockArea.enabled = false;
        }
    }
}
