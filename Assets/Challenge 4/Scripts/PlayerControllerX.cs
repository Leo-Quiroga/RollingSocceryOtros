using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    public GameObject goalKeeper;

    public bool hasPowerup;
    public bool hasGoalkeeper = false;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    public ParticleSystem speedParticles;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if(hasPowerup == true || hasGoalkeeper == true)
        {
            StartCoroutine("PowerupCooldown");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PowerUpSpeed();
        }

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);

            StartCoroutine("PowerupCooldown");
        }
        if (other.CompareTag("PowerUp2"))
        {
            hasGoalkeeper = true;
            Destroy(other.gameObject);
            //powerUpIndicator.gameObject.SetActive(true);
            goalKeeper.gameObject.SetActive(true);

            StartCoroutine("PowerupCooldown");
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        hasGoalkeeper = false;
        powerupIndicator.SetActive(false);
        goalKeeper.gameObject.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position  ; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }

    private void PowerUpSpeed()
    {
        playerRb.AddForce(focalPoint.transform.forward * 10, ForceMode.Impulse);
        speedParticles.Play();
    }



}
