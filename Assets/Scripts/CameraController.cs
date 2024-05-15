using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float xOffset = 0;
    public float yOffset = 2.5f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + xOffset, player.position.y + yOffset, transform.position.z);
    }
}
