using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[RequireComponent(typeof(Enemigo))]
public class Bomba : MonoBehaviour
{
    private Enemigo enemigo;
    private bool impacto;

    void Start()
    {
        enemigo = GetComponent<Enemigo>();

        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }

    private void Inicio() {
        Daño();
        Controlador.Rutina(Instanciar<Controles>.Coger("Controles").duracionTurno, () => {
            Daño();
        });
    }
    private void Daño() {
        if (enemigo.Visible(true) && !impacto)
        {
            /// Animacion ***
            enemigo.Dañar(transform, 1f, ref impacto);

            GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
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
