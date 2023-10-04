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
    [SerializeField] public float dañoCaida = 3f;

    public event Action InicioTurno;
    private Vector2 inicialPos;
    private bool caiendo;
    private Persona personaje;

    private void Awake()
    {
        Instanciar<Controles>.Añadir("Controles", this, gameObject);
        personaje = GetComponent<Persona>();
        if (modoRapido)
        {
            duracionTurno /= 2;
        }
    }
    private void Update()
    {
        if (caiendo)
        {
            Ray rayo = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(rayo, out hit, .6f) && !hit.collider.isTrigger) {
                GetComponent<Rigidbody>().isKinematic = true;
                caiendo = false;

                if (transform.position.y < -1)
                {
                    personaje.QuitarVida(100, false);
                }
            }
        }
        else
        {
            Movil();
            PC();
        }
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
            RaycastHit hit;

            if (Physics.Raycast(rayo, out hit, 1f) && !hit.collider.isTrigger)
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

                //--Comprobar si hay hierba--
                Collider[] colliders = Physics.OverlapSphere(transform.position, .4f);
                if (colliders.Some((e) =>  e.gameObject.layer == LayerMask.NameToLayer("Trigger") && e.tag != "Player" ))  {
                    gameObject.layer = LayerMask.NameToLayer("Trigger");
                }
                else if (gameObject.layer != LayerMask.NameToLayer("Default"))
                {
                    gameObject.layer = LayerMask.NameToLayer("Default");
                }

                //--Comprobar caida--
                Ray rayo = new Ray(transform.position, Vector3.down);
                if (!Physics.Raycast(rayo, out hit, .6f) || (Physics.Raycast(rayo, out hit, .6f) && hit.collider.isTrigger))
                {
                    caiendo = true;
                    GetComponent<Rigidbody>().isKinematic = false;
                }
            });
        }
    }
}
