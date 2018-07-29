using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;
using System.Globalization;

public class ObtenerPredicado : MonoBehaviour {
    //Creacino de la lista que contendra todo
    //public List<GameObject> listaBotones;
    public List<PuenteDatos> botones = new List<PuenteDatos>(3);
    string strPredicado="";
    string tagBoton="";
    public List<Predicados> ListaPredicados = new List<Predicados>();
    public Text inputText;
    string obtener = "";
    string Input
    {
        get { return inputText.text; }
        set { inputText.text = value; }
    }

    //CorutinaPredicado
    public IEnumerator LecturaDatos()
    {
        //obtengo el texto del inpput
        obtener = inputText.text;
        //lo transformo a String
        string textoO = obtener.Trim();
        //transformo en minusculas
        string a = textoO.ToLower();
        //La primera va a ser mayuscula
        string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(a);

        WWW www = new WWW("https://dbpedia.org/sparql?default-graph-uri=http%3A%2F%2Fdbpedia." +
            "org&query=SELECT+%3Fp%2C+count%28%3Fp%29+as+%3Fcuenta%0D%0AWHERE%7B%0D%0A%3FuriPais+" +
            "rdf%3Atype+dbo%3ACountry.%0D%0A%3FuriPais+rdfs%3Alabel+%3FnombrePais.%0D%0A%3FuriPais+" +
            "%3Fp+%3Fo.%0D%0AFILTER+%28%3FnombrePais%3D%22" + resul + "%22%40en%29%0D%0A%7D%0D%0AORDER+BY+" +
            "DESC+%28%3Fcuenta%29%0D%0ALIMIT+5&format=application%2Fsparql-results%2Bjson&CXML_" +
            "redir_for_subjs=121&CXML_redir_for_hrefs=&timeout=30000&debug=on&run=+Run+Query+");
        //espera cuando se carge los datos
        yield return www;
        //para presentar en consola
        print(www.text);
        //leer los datos presentados
        JsonData data = JsonMapper.ToObject(www.text);
        GameObject[] objeto;
        for (int i = 0; i < 3; i++)
        {
            //se crear una variable de Nombre
            Predicados predicado = new Predicados();
            //se ingresa a cada variable el dato que se sustrae
            predicado.predicado = data["results"]["bindings"][i]["p"]["value"].ToString();
            ListaPredicados.Add(predicado);
            print("predicados " + predicado.predicado);
            strPredicado = predicado.predicado;
            // MANDO EL TEXTO al boton


            botones[i].TextoInfoA = strPredicado;
            
            
        }
    }
}
