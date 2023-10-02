using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Ascensor : MonoBehaviour
{
    [SerializeField] AnimationCurve animacion;
    [SerializeField] float rango = .5f;
    private bool activo;
    private float duracion;

    void Start()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
        duracion = Instanciar<Controles>.Coger("Controles").duracionTurno;
        duracion -= duracion * 0.1f;
    }

    private void Inicio()
    {
        Controlador.Rutina(duracion * 0.05f, () => {
            Transform obj = null;

            if (activo)
            {
                //if (!Detectar())
                //{
                //    Mover(0);
                //    activo = false;
                //}
                Mover(0);
                activo = false;
                return;
            }

            if (Detectar(out obj))
            {
                obj.SetParent(transform.GetChild(0));

                Controlador.Rutina(duracion, () => {
                    obj.SetParent(null);
                });

                Mover(1, obj);
                activo = true;
            }
        });
    }

    private bool Detectar(out Transform obj) 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position.Y(1), .25f);
        foreach (Collider col in colliders)
        {
            if (col.tag == "Player")
            {
                obj = col.transform;
                return true;
            }
        }

        obj = null;
        return false;
    }

    private void Mover(int destino, Transform obj = null) {

        Controlador.Mover(transform.GetChild(0), new Movimiento(
            duracion,
            new Vector3(0, destino, 0),
            animacion
        ));
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
