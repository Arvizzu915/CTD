using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool generalCameraOn;

    GameObject player;
    PlayerMovement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        generalCameraOn = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(generalCameraOn);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!generalCameraOn)
            {
                generalCameraOn = true;
            }
            else
            {
                generalCameraOn = false;
            }
        }

        if (playerScript.inKitchen && !generalCameraOn) 
        {
            transform.position = new Vector3(0, 10.43f, -5.62f);
        }
        
        if (!playerScript.inKitchen && !generalCameraOn)
        {
            transform.position = player.transform.position + new Vector3(0, 10.43f, -5.62f);
        }

        if (generalCameraOn)
        {
            transform.position = new Vector3(0, 29, -13.41f);
        }

    }
}
