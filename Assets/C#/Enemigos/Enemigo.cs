using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Enemigo : MonoBehaviour
{
    [Header("Alertas")]
    private GameObject alerta;

    [Header("Visibilidad")]
    [SerializeField] private int rango;
    [SerializeField] public Vector3[] direcciones;

    [Header("Otros")]
    [SerializeField] public int daño;

    //Alertas
    private void Start()
    {
        GameObject alerta = Resources.Load<GameObject>("Exclamacion");
        this.alerta = Instantiate(alerta, transform);

        //this.alerta.SetActive(false);
    }

    public void AlterarAlerta(bool estado)
    {
        Animator animator = alerta.GetComponent<Animator>();

        //alerta.SetActive(estado);
        animator.speed = 6f;
        animator.SetBool("grande", estado);
    }

    //Visibilidad
    public bool Visible(out Vector3 miDireccion) 
    {
        foreach (Vector3 direccion in direcciones)
        {
            Ray rayo = new Ray(transform.position, direccion);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayo, out hitInfo, rango))
            {
                if (hitInfo.collider.CompareTag("Player")) {
                    miDireccion = direccion;
                    return true;
                }
            }
        }
        miDireccion = Vector3.zero;
        return false;
    }
    public bool Visible() {
        Vector3 nulo;
        return Visible(out nulo);
    }


    public bool Dañar(Transform trans, float radio, ref bool impacto, bool personaDetecta = true) {
        if (!impacto)
        {
            Collider[] colliders = Physics.OverlapSphere(trans.position, radio);

            foreach (Collider col in colliders)
            {
                if (gameObject.tag != col.tag)
                {
                    Persona persona = col.GetComponent<Persona>();
                    if (persona != null)
                    {
                        persona.QuitarVida(daño);
                        impacto = true;
                        return true;
                    }
                }     
            }

            ///***BUG SE AUTODESTRUIE
            //if (!personaDetecta && colliders.Length > 0 && !colliders.Some((col) => gameObject.tag != col.tag))
            //{
            //    impacto = true;
            //    return true;
            //}
        }
        return false;
    }

}
