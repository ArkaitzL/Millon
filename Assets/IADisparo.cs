using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemigo))]
public class IADisparo : MonoBehaviour
{
    private bool detectado;
    private Enemigo cuerpo;

    void Start()
    {
        cuerpo = GetComponent<Enemigo>();
        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }

    private void Inicio() 
    {

        //Si tiene un objetivo
        if (detectado)
        {
            Disparar();

            cuerpo.AlterarAlerta(false);
            detectado = false;

            return;
        }

        //Si no tiene un objetivo
        if (cuerpo.Visible())
        {
            cuerpo.AlterarAlerta(true);
            detectado = true;
        }
    }

    private void Disparar() {
        GameObject municion = Resources.Load<GameObject>("P3_Municion");
        foreach (Vector3 direccion in cuerpo.direcciones)
        {
            municion = Instantiate(municion, transform);
            municion.GetComponent<Municion>().direccion = direccion;
            municion.GetComponent<Municion>().daño = cuerpo.daño;

            //Destroy(municion, 7.5f);
        }
    }


    //DESTRUIR
    public void OnDestroy()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno -= Inicio;
    }
}
