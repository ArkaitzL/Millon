using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[RequireComponent(typeof(Enemigo))]
[RequireComponent(typeof(Persona))]
public class IAEnem3 : MonoBehaviour
{
    [SerializeField] private Direccion direccionActual;
    private Enemigo enemigo;
    private Persona persona;

    private bool detectado;
    private bool impacto;

    void Start()
    {
        enemigo = GetComponent<Enemigo>();
        persona = GetComponent<Persona>();

        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }
    private void Update()
    {
        /// *** No funciona
        enemigo.Dañar(transform, .4f, ref impacto);
    }

    private void Inicio() 
    {
        impacto = false;

        //Si tiene un objetivo
        if (detectado)
        {
            Estampar();

            enemigo.AlterarAlerta(false);
            detectado = false;

            return;
        }

        //Si no tiene un objetivo
        Vector3 miDireccion;
        if (enemigo.Visible(out miDireccion))
        {
            //Rotar
            persona.Rota(miDireccion);
            direccionActual.Set(miDireccion);

            enemigo.AlterarAlerta(true);
            detectado = true;
        }
    }

    private void Estampar() 
    {
        ///*** Mirar tambien si hay algo adelante: Un personaje, un bloque...
        ///     Si es una persona se mueve hasta la persona
        ///     Si no uno antes como con el suelo

        Vector3 posicionInicial = transform.position.Y(-1f);
        int distancia = 1;

        float distanciaMax = 250f;

        while (distancia < distanciaMax)
        {
            // Lanza un rayo desde la posición actual
            Collider[] colliders = Physics.OverlapSphere(posicionInicial + (direccionActual.Get() * distancia), 0.1f);

            if (colliders.Length > 0)
            {
                distancia++;
            }
            else
            {
                // Si no chocamos con ningún collider, hemos llegado al final
                persona.Mueve(direccionActual.Get(), distancia-2);
                break;
            }
        }

    }

    //DESTRUIR
    public void OnDestroy()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno -= Inicio;
    }
}
