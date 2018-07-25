using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class texto : MonoBehaviour {
    //public Text sparql;

    public Text palabras;
    
    // Use this for initialization
    void Start () {
        string pal1= "Ecuador";
        string pal2="Ecuador";

        //palabras.text = "Hola <color=#ff0000ff>" + pal1+ "</color> , yo bien <color=#ff0000ff> " + pal2+ "</color>";
        //palabras.text = "Select distinct * where{\n ";



        //    public Text tx;
        //// Use this for initialization
        //void Start()
        //{
        //    tx.text = "<color=#ff0000ff>Hola</color> <color=#0000ffff>Como estas</color>";
        //}


        palabras.text = "select distinc ? p count(*) As ?morep ? <color=#ff0000ff>" + pal1 + "</color> \nwhere" +
        "{[] rdf:type dbo:Country ; ?p [] . \n?p rdfs:label ?etiqueta" +
        "\nFILTER (lang(?etiqueta) = \"en\")" +
        "\n} GROUP BY ?p ?etiqueta" +
        "\nORDER BY DESC(?"+pal2+ ")";
}
}
