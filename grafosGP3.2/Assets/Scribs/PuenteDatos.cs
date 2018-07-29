using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuenteDatos : MonoBehaviour {
    public MostrarTexto textoDisplay;
    public Text inputText;
    public Text botonEtiqueta;
    private string textoInfoA;
    public string TextoInfoA
    {
        get { return textoInfoA; }
        set
        {
            if (textoInfoA != value)
            {
                textoInfoA = value;
                botonEtiqueta.text = value;
            }
        }
    }


    public void TransferirDatos()
    {
        textoDisplay.ShowInfo(textoInfoA, inputText.text);

    }

}
