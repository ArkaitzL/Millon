using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemigo))]
public class Municion : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float radio;

    [HideInInspector] public Vector3 direccion;
    [HideInInspector] public int daño;

    private Enemigo cuerpo;
    private bool impacto;

    private void Start()
    {
        cuerpo = GetComponent<Enemigo>();
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;

        if (cuerpo.Dañar(transform, radio, ref impacto, false)) {
            Destroy(gameObject, .05f);
        }
    }
}
