using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_map : MonoBehaviour
{
    public Transform left;
    public Transform right;

    private float leftX, rightX;
    // Start is called before the first frame update
    void Start()
    {
        leftX = left.position.x;
        rightX = right.position.x;

        Destroy(left.gameObject);
        Destroy(right.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    { }
}
