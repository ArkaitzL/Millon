using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

public class Persona : MonoBehaviour
{
    
    [SerializeField] private AnimationCurve animacion;
    [SerializeField] private Particula[] particulas;
    [SerializeField] private int casillas;
    [SerializeField] private int vida = 1;

    public void Mueve(Vector3 direccion, int incremento = 0)
    {
        float duracion = Instanciar<Controles>.Coger("Controles").duracionTurno;

        Controlador.Mover(transform, new Movimiento(
            duracion,
            transform.position + (direccion.normalized * (casillas + incremento)),
            animacion
        ));
    }

    public void Rota(Vector3 direccion) 
    {
        float duracion = Instanciar<Controles>.Coger("Controles").duracionTurno;

        Controlador.Rotar(transform, new Rotacion(
            duracion,
            Quaternion.LookRotation(direccion),
            animacion
        ));
    }

    public void Chocar(Vector3 direccion)
    {
        float duracion = Instanciar<Controles>.Coger("Controles").duracionTurno;
        Vector3 origen = transform.position;

        Controlador.Mover(transform, new Movimiento(
             duracion/2,
             transform.position + (direccion.normalized * casillas/5 ),
             animacion
         ));

        Controlador.Rutina((duracion/2), () => {
            Controlador.Mover(transform, new Movimiento(
               duracion / 2,
               origen,
               animacion
           ));
        });

        transform.position = origen;
    }

    public void QuitarVida(int cantidad, bool animacion = true) 
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            vida = 0;

            /// *** Desactiva todos los scripts que podria tener ***

            if (tag == "Player")
            {
                Debug.Log("GAMEOVER!!");
            }
            if (animacion)
            {
                Particulas();
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        /// *** Cambiar el UI ***
    }

    public void Particulas()
    {

        particulas.ForEach((p) =>
        {
            for (int i = 0; i < p.cantidad; i++)
            {
                float x = UnityEngine.Random.Range(0, 0.6f);
                float y = UnityEngine.Random.Range(0, 0.6f);
                float z = UnityEngine.Random.Range(0, 0.6f);

                Transform particula = Instantiate(Resources.Load<GameObject>("Particula").transform, transform.position + new Vector3(x, y, z), Quaternion.identity);
                particula.GetComponent<Renderer>().material.color = p.color;
                particula.SetParent(transform);

                Destroy(particula.gameObject, UnityEngine.Random.Range(3F, 5F));
            }
        });
    }

    [Serializable]
    public class Particula
    {
        public Color32 color;
        public int cantidad;
    }
}
