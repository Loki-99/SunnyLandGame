using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public void GemDeath()
    {
        FindObjectOfType<Movement>().GemCount();
        Destroy(gameObject);
    }
}
