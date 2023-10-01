using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IA : MonoBehaviour
{
    internal bool impacto;

    protected void Iniciar()
    {
        Instanciar<Controles>.Coger("Controles").InicioTurno += Inicio;
    }

    protected virtual void Inicio() { }

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
