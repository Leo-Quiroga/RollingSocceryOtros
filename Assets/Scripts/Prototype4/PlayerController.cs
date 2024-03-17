using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private GameObject focalPoint;
    private Rigidbody playerRB;

    public bool hasPowerUp = false;
    
    private float powerUpStrength = 15;

    public GameObject powerUpIndicator;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerUpIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            powerUpIndicator.gameObject.SetActive(true);
            StartCoroutine("PowerupCountdownRoutine");
        }
        
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerUp = false;
        powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy")&& hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerUp set to " + hasPowerUp);
        }
    }

    
}
