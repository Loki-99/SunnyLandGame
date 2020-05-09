using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedPanel : MonoBehaviour
{
    public GameObject diedPanel; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            diedPanel.SetActive(true);
        }
    }
}
