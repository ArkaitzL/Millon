using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Direccion
{
    public enum Opcion
    {
        Adelante,
        Atras,
        Izquierda,
        Derecha
    }

    public Opcion seleccion;

    public Vector3 Get()
    {
        switch (seleccion)
        {
            case Direccion.Opcion.Adelante:
                return Vector3.forward;
            case Direccion.Opcion.Atras:
                return Vector3.back;
            case Direccion.Opcion.Izquierda:
                return Vector3.left;
            case Direccion.Opcion.Derecha:
                return Vector3.right;
        }

        return Vector3.zero;
    }

    public void Set(Opcion seleccion) {
        this.seleccion = seleccion;
    }
    public void Set(Vector3 seleccion)
    {
        if (Vector3.forward == seleccion)
        {
            this.seleccion = Opcion.Adelante;
            return;
        }
        if (Vector3.back == seleccion)
        {
            this.seleccion = Opcion.Atras;
            return;
        }
        if (Vector3.left == seleccion)
        {
            this.seleccion = Opcion.Izquierda;
            return;
        }
        if (Vector3.right == seleccion)
        {
            this.seleccion = Opcion.Derecha;
            return;
        }
    }
}