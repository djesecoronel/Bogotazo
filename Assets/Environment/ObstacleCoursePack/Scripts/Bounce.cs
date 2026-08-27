using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float force = 100f; // Sube este valor a 50 o 100 si lo notas muy flojo por la escala del cubo
    public float stunTime = 0.3f;

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            if (collision.gameObject.tag == "player")
            {
                collision.gameObject.GetComponent<PlayerController>().HitPlayer(Vector3.up * force, stunTime);
                return; // Evita que se repita la llamada en el mismo frame
            }
        }
    }
}