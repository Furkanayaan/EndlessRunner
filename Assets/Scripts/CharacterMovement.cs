using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float sidewaysSpeed = 4f;
    public float turnSpeed = 90f;

    private bool isTurning = false;
    private Quaternion targetRotation;
    private Rigidbody rb;
    public int currentPlatformIndex;

    private void Start() {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sidewaysMovement = transform.right * horizontalInput * sidewaysSpeed * Time.deltaTime;
        
        Vector3 newPosition = rb.position + sidewaysMovement + forwardMovement;
        rb.MovePosition(newPosition);
        
        if (isTurning) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                isTurning = false;
            
        }

        ControlOfPlatform();
    }

    
    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("TurnPoint")) {
            Vector3 newDirection = other.transform.forward;
            targetRotation = Quaternion.LookRotation(newDirection);
            isTurning = true;
        }

        if (other.CompareTag("LastPoint")) {
            currentPlatformIndex++;
            
        }
    }

    public void ControlOfPlatform() {
        if (currentPlatformIndex != 1) return;
        if (Vector3.Distance(transform.position, LevelManager.I.currentPlatform(currentPlatformIndex).GetChild(1).position) < 15f) {
            LevelManager.I.SpawnPlatform();
            LevelManager.I.RemoveOldPlatform();
            currentPlatformIndex = 0;
        }
    }
}

