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

            if (!enemigo.VerPlayer(transform, rangoVision))
            {
                enemigo.AlterarAlerta(false);
                detectado = false;

                return;
            }

            Vector3 direccion = Vector3.zero;
            if (enemigo.Visible(out direccion))
            {
                persona.Rota(direccion);
                persona.Chocar(direccion);

                return;
            }

            if (detectado)
            {
                float x = transform.position.x - player.position.x;
                float z = transform.position.z - player.position.z;

                if (Mathf.Abs(x) > Mathf.Abs(z))
                {
                    direccion = new Vector3(x, 0, 0);

                    persona.Rota(direccion);
                    persona.Mueve(direccion * -1);
                }
                else
                {
                    direccion = new Vector3(0, 0, z);

                    persona.Rota(direccion);
                    persona.Mueve(direccion * -1);
                }
            }

            if (enemigo.VerPlayer(transform, rangoVision))
            {
                enemigo.AlterarAlerta(true);
                detectado = true;
            }
        });
    }
}
