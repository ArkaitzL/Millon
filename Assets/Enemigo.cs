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
    public bool Visible() 
    {
        foreach (Vector3 direccion in direcciones)
        {
            Ray rayo = new Ray(transform.position, direccion);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayo, out hitInfo, rango))
            {
                if (hitInfo.collider.CompareTag("Player")) {
                    return true;
                }
            }
        }
        return false;
    }

}
