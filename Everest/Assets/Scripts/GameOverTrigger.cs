using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private GameObject gameOverTriggerUI;

    private void Start()
    {
        gameOverTriggerUI = GameObject.FindGameObjectWithTag("GameOverUI");
        gameOverTriggerUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameOverTriggerUI.SetActive(true);
        Destroy(gameObject);
    }


}
