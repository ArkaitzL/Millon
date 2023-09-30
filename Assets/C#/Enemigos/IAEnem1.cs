using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[RequireComponent(typeof(Enemigo))]
[RequireComponent(typeof(Persona))]
public class IAEnem1 : MonoBehaviour
{
    [SerializeField] Ruta[] ruta;
    [SerializeField] private Direccion direccionActual;

    private Enemigo enemigo;
    private Persona persona;

    private int rutaNum, pasosNum;
    private bool impacto;

    void Start()
    {
        enemigo = GetComponent<Enemigo>();
        persona = GetComponent<Persona>();

        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }

    private void Inicio()
    {
        //Reiniciar impacto
        impacto = false;


        //Golpera al enemigo si esta delante
        Controlador.Rutina(Instanciar<Controles>.Coger("Controles").duracionTurno, () => {
            if (enemigo.Visible())
            {
                persona.Chocar(direccionActual.Get());
                return;
            }
        });

        //Girar o moverse siguindeo la ruta
        if (ruta[rutaNum].distancia == pasosNum)
        {
            /// *** FUTURO: Poder elegir si va pa atras o sigue
            pasosNum = 0;
            if (rutaNum == ruta.Length-1) rutaNum = 0;
            else rutaNum++;
        }

        Vector3 direccion = ruta[rutaNum].direccion.Get();
        if (direccion != direccionActual.Get())
        {
            direccionActual.Set(direccion);
            enemigo.direcciones = new Vector3[] { direccion };
            persona.Rota(direccion);
        }
        persona.Mueve(direccion);
        pasosNum++;

        ///*** FUTURO: Moverse hasta el destino, usa lo mismo pero pasando las casillas que tiene hasta el destino en lugar de 1 usando la propiedad incremento

        //Golpera al enemigo si esta delante
        Controlador.Rutina(Instanciar<Controles>.Coger("Controles").duracionTurno, () => {
            if (enemigo.Visible())
            {
                persona.Chocar(direccion);
                return;
            }
        });
    }

    private void Update()
    {
        enemigo.Dañar(transform, .32f, ref impacto);
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

[Serializable]
public class Ruta {
    public int distancia;
    public Direccion direccion;
}
