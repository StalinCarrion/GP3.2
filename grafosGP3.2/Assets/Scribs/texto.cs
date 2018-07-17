using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class texto : MonoBehaviour {
    //public Text sparql;

    public Text palabras;
    
    // Use this for initialization
    void Start () {
        string pal1= "Stalin";
        string pal2="como estas";

        palabras.text = "Hola <color=#ff0000ff>" + pal1+ "</color> , yo bien <color=#ff0000ff> " + pal2+ "</color>";


    //    public Text tx;
    //// Use this for initialization
    //void Start()
    //{
    //    tx.text = "<color=#ff0000ff>Hola</color> <color=#0000ffff>Como estas</color>";
    //}


    //sparql.text = "select distinc ? p count(*) As ?morep ? "+ pal1 + " \nwhere" +
    //    "{[] rdf:type dbo:Country ; ?p [] . \n?p rdfs:label ?etiqueta" +
    //    "\nFILTER (lang(?etiqueta) = \"en\")" +
    //    "\n} GROUP BY ?p ?etiqueta" +
    //    "\nORDER BY DESC(?"+pal2+ ")";
}
}
