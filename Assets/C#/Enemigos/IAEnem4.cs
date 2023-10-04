using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[RequireComponent(typeof(Persona))]
[RequireComponent(typeof(Enemigo))]

public class IAEnem4 : IA
{
    [SerializeField] private int rangoVision = 2;
    private Enemigo enemigo;
    private Persona persona;
    private Transform player;

    private bool detectado;

    void Start()
    {
        Iniciar();

        enemigo = GetComponent<Enemigo>();
        persona = GetComponent<Persona>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        enemigo.Dañar(transform, .45f, ref impacto);
    }

    override protected void Inicio() {


        Controlador.Rutina(Instanciar<Controles>.Coger("Controles").duracionTurno * .01f, () => {

            //Comprueba si ha salido de su rango de vision
            if (!enemigo.VerPlayer(transform, rangoVision))
            {
                enemigo.AlterarAlerta(false);
                detectado = false;

                return;
            }

            //Golpear al perdsonaje
            Vector3 apuntar = Vector3.zero;
            if (enemigo.Visible(out apuntar))
            {
                persona.Chocar(apuntar);
            }


            //La IA del movimiento
            if (detectado)
            {
                //Decide la direccion
                Vector3 direccion = Vector3.zero;

                float x = transform.position.x - player.position.x;
                float z = transform.position.z - player.position.z;

                direccion = (Mathf.Abs(x) > Mathf.Abs(z)) 
                    ? new Vector3(x, 0, 0) 
                    :  new Vector3(0, 0, z);

                persona.Rota(direccion);

                //Si hay algo delante le coliciona
                Ray rayo = new Ray(transform.position, direccion * -1);
                RaycastHit hit;

                if (Physics.Raycast(rayo, out hit, 1f) && !hit.collider.isTrigger)
                {
                    persona.Chocar(direccion);
                    return;
                }

                //Mueve el personaje
                persona.Mueve(direccion * -1);
            }

            //Añade al jugador si esta dentro de su rango de vision
            if (enemigo.VerPlayer(transform, rangoVision))
            {
                enemigo.AlterarAlerta(true);
                detectado = true;
            }
        });
    }
}
