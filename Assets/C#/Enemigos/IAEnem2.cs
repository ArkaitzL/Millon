using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemigo))]
public class IAEnem2 : MonoBehaviour
{
    private bool detectado;
    private Enemigo enemigo;

    void Start()
    {
        enemigo = GetComponent<Enemigo>();
        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }

    private void Inicio() 
    {

        //Si tiene un objetivo
        if (detectado)
        {
            Disparar();

            enemigo.AlterarAlerta(false);
            detectado = false;

            return;
        }

        //Si no tiene un objetivo
        if (enemigo.Visible())
        {
            enemigo.AlterarAlerta(true);
            detectado = true;
        }
    }

    private void Disparar() {
        GameObject municion = Resources.Load<GameObject>("P3_Municion");
        foreach (Vector3 direccion in enemigo.direcciones)
        {
            municion = Instantiate(municion, transform);
            municion.GetComponent<Municion>().direccion = direccion;
            municion.GetComponent<Municion>().daño = enemigo.daño;

            Destroy(municion, 5f);
        }
    }


    //DESTRUIR
    private void OnDestroy()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno -= Inicio;
    }

    private void OnDisable()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno -= Inicio;
    }
}
