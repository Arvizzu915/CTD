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
        Debug.Log(playerScript.inKitchen);

        if (playerScript.inKitchen) 
        {
            transform.position = new Vector3(0, 11.35f, -7.69f);
        }
        else
        {
            transform.position = player.transform.position + new Vector3(0, 10.35f, -8);
        }
    }
}
