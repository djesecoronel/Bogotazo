using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObs : MonoBehaviour
{
    public float distance = 5f; 
    public float speed = 3f;
    public float offset = 0f; 

    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        
        // Nos aseguramos de que el Rigidbody NO sea cinemático para que aplique fuerza física de empuje
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            // Congelamos las rotaciones para que la pared no ruede ni se caiga
            rb.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Calculamos la posición deseada en el tiempo usando transform.right
        float movement = Mathf.Sin(Time.time * speed) * (distance * 0.5f) + offset;
        Vector3 targetPos = startPos + (transform.right * movement);

        // Calculamos la velocidad exacta necesaria para llegar a la posición objetivo
        Vector3 velocity = (targetPos - rb.position) / Time.fixedDeltaTime;
        
        // Aplicamos la velocidad para que el motor de físicas empuje al jugador con fuerza
        rb.linearVelocity = velocity;
    }
}