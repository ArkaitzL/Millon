using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

[RequireComponent(typeof(Persona))]
public class Controles : MonoBehaviour
{
    [SerializeField] private float deslizamientoMin = 50f;
    [SerializeField] public float duracionTurno;
    [SerializeField] public bool modoRapido;

    public event Action InicioTurno;
    private Vector2 inicialPos;


    private void Awake()
    {
        Instanciar<Controles>.Añadir("Controles", this, gameObject);
        if (modoRapido)
        {
            duracionTurno /= 2;
        }
    }
    private void Update()
    {
        Movil();
        PC();
    }

    private void Movil()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //INICIO
            if (touch.phase == TouchPhase.Began)
            {
                inicialPos = touch.position;
            }
            //FIN
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 finalPos = touch.position;
                Vector2 apuntado = finalPos - inicialPos;

                if (apuntado.magnitude < deslizamientoMin) return;
                apuntado.Normalize();

                if (apuntado.y > 0.5f)
                {
                    Turno(0);
                }
                else if (apuntado.y < -0.5f)
                {
                    Turno(1);
                }
                else if (apuntado.x > 0.5f)
                {
                    Turno(2);
                }
                else if (apuntado.x < -0.5f)
                {
                    Turno(3);
                }
            }
        }
    }

    private void PC()
    {
        bool[] teclas = {
            Input.GetKeyDown(KeyCode.W),
            Input.GetKeyDown(KeyCode.S),
            Input.GetKeyDown(KeyCode.D),
            Input.GetKeyDown(KeyCode.A)
        };
        teclas.ForEach((tecla, index) => {
            if (tecla)
            {
                Turno(index);
            }
        });
    }

    //SISTEMA DE TURNOS
    private void Turno(int i) 
    {
        Persona personaje = GetComponent<Persona>();
        Vector3[] direcciones = {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };


        if (Controlador.Esperando("Turno"))
        {
            Controlador.IniciarEspera("Turno", duracionTurno*2);

            //ANTES TURNO

            //--Activa el RB
            //GetComponent<Rigidbody>().isKinematic = true;

            //--Rota el personaje(No cuenta como turno)--
            personaje.Rota(direcciones[i]);

            //--Comporobar si se puede mover--
            Ray rayo = new Ray(transform.position, direcciones[i]);
            if (Physics.Raycast(rayo, 1f))
            {
                personaje.Chocar(direcciones[i]);
                return;
            }

            //COMIENZA TURNO

            //--Mover el personaje--
            personaje.Mueve(direcciones[i]);

            //ACABA TURNO

            //--Comprobar el estado despues de moverse-- (Llamar a una rutina con la duracion del turno)
            Controlador.Rutina(duracionTurno, () =>
            {
                //--Desactiva el RB
                //GetComponent<Rigidbody>().isKinematic = false;

                //--Mover lo demas--
                InicioTurno?.Invoke();

                //--Comprobar caida--
                Ray rayo = new Ray(transform.position, Vector3.down);
                if (!Physics.Raycast(rayo, 1f))
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    personaje.QuitarVida(100, false);
                }
            });
        }
    }
}
