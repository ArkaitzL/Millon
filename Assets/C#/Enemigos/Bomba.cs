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
        Da�o();
        Controlador.Rutina(Instanciar<Controles>.Coger("Controles").duracionTurno, () => {
            Da�o();
        });
    }
    private void Da�o() {
        if (enemigo.Visible(true) && !impacto)
        {
            /// Animacion ***
            enemigo.Da�ar(transform, 1f, ref impacto);

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
