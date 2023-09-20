using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float radio;

    [HideInInspector] public Vector3 direccion;
    [HideInInspector] public int daño;

    private bool impacto;

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;


        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider col in colliders)
        {
            Persona persona = col.GetComponent<Persona>();
            if (persona != null && !impacto)
            {
                persona.QuitarVida(daño);
                impacto = true;
                Destroy(gameObject, .1f);
            }
        }
    }
}
