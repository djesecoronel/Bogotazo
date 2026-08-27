using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f; // Limit in degrees of the movement
    public bool randomStart = false; // If you want to modify the start position
    public float pushForce = 50f; // Fuerza aumentada para tumbar al jugador de un golpe
    private float random = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if(randomStart)
            random = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = limit * Mathf.Sin((Time.time * speed) + random);
			transform.localRotation = Quaternion.Euler(angle, 0, 0);    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta si choca contra el jugador u otro objeto con Rigidbody
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb != null)
        {
            // Calcula la dirección del impacto y añade un impulso fuerte
            Vector3 pushDir = (collision.transform.position - transform.position).normalized;
            pushDir.y = 1f; // Impulso vertical fuerte para asegurar que te tumbe o eleve
            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
}