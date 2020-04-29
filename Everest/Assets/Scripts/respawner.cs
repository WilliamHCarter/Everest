using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawner : MonoBehaviour
{
    private Vector2 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = new Vector2(-30,-10); //starting spawn point
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            die();
        }
        else if (collision.gameObject.tag == "SavePoint")
        {
            respawnPoint = collision.gameObject.transform.position;
        }
    }
    private void die()
    {
        Debug.Log("You Die");
        transform.position = respawnPoint;
    }

}
