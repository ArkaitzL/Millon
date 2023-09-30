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
        enemigo.Dañar(transform, .32f, ref impacto);
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

        Vector3 posicionInicial = transform.position.Y(-1f);
        int distancia = 1;

        float distanciaMax = 250f;

        while (distancia < distanciaMax)
        {
            // Lanza un rayo desde la posición actual
            int exceptTrigger = ~LayerMask.GetMask("Trigger");

            Collider[] suelo = Physics.OverlapSphere(posicionInicial + (direccionActual.Get() * distancia), 0.1f);
            Collider[] superficie = Physics.OverlapSphere(transform.position + (direccionActual.Get() * distancia), 0.1f, exceptTrigger);


            bool condicion = superficie.Length == 0;
            if (!condicion)
            {
                if (superficie.Some((col) => col.CompareTag("Player")))
                {
                    condicion = true;
                }
            }

            if (suelo.Length > 0 && condicion)
            {
                distancia++;
            }
            else
            {
                // Si no chocamos con ningún collider, hemos llegado al final
                persona.Mueve(direccionActual.Get(), distancia - 2);
                break;
            }

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
