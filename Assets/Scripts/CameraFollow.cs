using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    void LateUpdate()
    {
        Vector3 newPosition = player.position + player.TransformDirection(offset);
        transform.position = newPosition;
        
        Vector3 cameraRotation = new Vector3(26f, player.eulerAngles.y, 0);
        transform.eulerAngles = cameraRotation;
    }
}
