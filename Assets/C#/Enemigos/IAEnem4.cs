using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Persona))]
[RequireComponent(typeof(Enemigo))]

public class IAEnem4 : IA
{
    private Enemigo enemigo;
    private Persona persona;

    void Start()
    {
        Iniciar();

        enemigo = GetComponent<Enemigo>();
        persona = GetComponent<Persona>();
    }

    void Update()
    {
        enemigo.Dañar(transform, .45f, ref impacto);
    }

    override protected void Inicio() {

        if (enemigo.Visible())
        {

            enemigo.AlterarAlerta(true);
        }
        else {
            enemigo.AlterarAlerta(false);
        }
    }
}
