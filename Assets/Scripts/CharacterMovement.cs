using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public static CharacterMovement I;
    public float forwardSpeed = 6f;
    public float sidewaysSpeed = 6f;
    public float turnSpeed = 90f;
    public float jumpForce = 6f;

    private bool bTurning = false;
    private Quaternion targetRotation;
    private Rigidbody rb;
    private int currentPlatformIndex;
    private int levelCount;
    private bool bGrounded = true;
    private bool bDie = false;
    private int goldCount = 0;

    private void Start() {
        I = this;
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        levelCount = 1;
    }

    void Update() {
        if(bDie) return;
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sidewaysMovement = transform.right * horizontalInput * sidewaysSpeed * Time.deltaTime;
        
        Vector3 newPosition = rb.position + sidewaysMovement + forwardMovement;
        rb.MovePosition(newPosition);
        if (Input.GetKeyDown(KeyCode.Space) && bGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            bGrounded = false;
        }
        
        if (bTurning) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                bTurning = false;
            
        }

        ControlOfPlatform();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name.Contains("Ground")) {
            bGrounded = true;
        }
        if (other.gameObject.CompareTag("Obstacle")) {
            bDie = true;
            UIManager.I.youDie.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("LastPoint")) {
            currentPlatformIndex++;
            levelCount++;

        }

        if (other.CompareTag("Gold")) {
            goldCount++;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            other.gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerExit(Collider other) {
        //Changing the character's rotation based on the platform.
        if (other.CompareTag("TurnPoint")) {
            Vector3 newDirection = other.transform.forward;
            targetRotation = Quaternion.LookRotation(newDirection);
            bTurning = true;
        }
    }

    public int CurrentLevel() {
        return levelCount;
    }

    public int CurrentGold() {
        return goldCount;
    }

    //Maintaining a total of 3 active platforms in the scene.
    public void ControlOfPlatform() {
        if (currentPlatformIndex != 1) return;
        if (Vector3.Distance(transform.position, LevelManager.I.currentPlatform(currentPlatformIndex).GetChild(1).position) < 15f) {
            LevelManager.I.SpawnPlatform();
            LevelManager.I.RemoveOldPlatform();
            currentPlatformIndex = 0;
        }
    }
}

