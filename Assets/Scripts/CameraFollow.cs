using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerScript.inKitchen) 
        {
            transform.position = new Vector3(0, 10.43f, -5.62f);
        }
        else
        {
            transform.position = player.transform.position + new Vector3(0, 10.43f, -5.62f);
        }
    }
}
