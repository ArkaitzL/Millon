using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Ascensor : MonoBehaviour
{
    [SerializeField] AnimationCurve animacion;
    [SerializeField] float rango = .25f;
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
            Ray rayo = new Ray(transform.position.Y(.5f), transform.up);
            RaycastHit hitInfo;

            if (activo)
            {
                if (!Physics.Raycast(rayo, out hitInfo, rango))
                {
                    Mover(0);
                    activo = false;
                }
                return;
            }

            if (Physics.Raycast(rayo, out hitInfo, rango))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    Transform obj = hitInfo.collider.transform;
                    obj.SetParent(transform.GetChild(0));

                    Controlador.Rutina(duracion, () => {
                        obj.SetParent(null);
                    });

                    Mover(1, obj);
                    activo = true;
                }
            }
        });
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
