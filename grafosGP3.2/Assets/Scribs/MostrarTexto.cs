using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarTexto : MonoBehaviour {

    public Text palabras;

    // Use this for initialization
    public void ShowInfo(string pal1, string pal2)
    {
        palabras.text = "CONSTRUCT \n{?uriPais ?type ?class . \n?uriPais ?etiqueta ?nombrePais . }" +
            "\nWHERE{ \nvalues ?typev{rdf:type} \nvalues ?etiquetav{rdfs:label} \nvalues ?class {dbo:Country}" +
            "\n?uriPais ?type ?class. \n?uriPais ?etiqueta ?nombrePais. \n?uriPais <<color=#FF0000>" + pal1 + "</color>> <color=#800080>?o</color>." +
            "\nFILTER (?nombrePais = \"<color=#DCFE00>" + pal2 + "</color>\"@en) \n}";
    }
}
