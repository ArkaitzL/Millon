using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemigo))]
[RequireComponent(typeof(Persona))]
public class IAMovFijo : MonoBehaviour
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
        
    }
}
