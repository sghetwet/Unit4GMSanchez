using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject focalPoint;
    public float speed = 5f;
    private float powerUpStrength = 100.0f;
    public bool hasPowerUp = false;
    public GameObject PowerUpIndicator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        rb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        PowerUpIndicator.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Power"))
        {
            Destroy(other.gameObject);
            PowerUpIndicator.gameObject.SetActive(true);
            hasPowerUp = true;
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }
    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        PowerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with; " + collision.gameObject.name + " with powerup set to " + hasPowerUp);
        }
    }
}
