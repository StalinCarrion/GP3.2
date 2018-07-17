using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;


public class ImportarPaises : MonoBehaviour {

    

    //public InputField texbox1;
    public GameObject origin;
    public GameObject destino;
    public GameObject texto;
    //---------------------
    public Vector3 vectorSujeto;
    public Transform TransforEsfera;
    
    //---------------------
    //Objeto donde se guarda la esfera
    public GameObject sphere;
    //para generar el minimo y maximo en la posicion random
    public int min, max;
    //Creacino de la lista que contendra todo
    public List<Nombres> nombre = new List<Nombres>();
    public int i;
    public Text inputText;
    //KeyboardManager objTeclado = new KeyboardManager();
    IEnumerator Start()
    {
        //-------------------------
        vectorSujeto = TransforEsfera.position;
        //-------------------------

        //get { return inputText.text; }
        Debug.Log("ESTO ES TEXTOOO " + texto);
        int nEsferas = 5;
        //link de la consulta donde se sustraen los datos
        WWW www = new WWW("http://es-la.dbpedia.org/sparql?default-graph-uri" +
            "=&query=select+%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2FEcuador%" +
            "3E+%3Fp+%3Fo+where+%7B%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2FEcuador%3E" +
            "+%3Fp+%3Fo%7D+LIMIT+" + 5 + "&format=application%2Fsparql-results%2Bjson&timeout=0&debug=on");
        //espera cuando se carge los datos
        yield return www;
        //para presentar en consola
        print(www.text);
        //leer los datos presentados
        JsonData data = JsonMapper.ToObject(www.text);
        //var pRed = GeneratedPosition();

        for (int i = 0; i < nEsferas; i++)
        {
            GameObject textGo = new GameObject("Objeto");
            GameObject textSujeto = new GameObject("Sujeto");

            //var pBlue = GeneratedPosition();
            //se crear una variable de Nombre
            Nombres nom = new Nombres();
            //se ingresa a cada variable el dato que se sustrae
            nom.Sujeto = data["results"]["bindings"][i]["callret-0"]["value"].ToString();
            nom.TypeSujeto = data["results"]["bindings"][i]["callret-0"]["type"].ToString();
            nom.Predicado = data["results"]["bindings"][i]["p"]["value"].ToString();
            nom.TypePredicado = data["results"]["bindings"][i]["p"]["type"].ToString();
            nom.Objeto = data["results"]["bindings"][i]["o"]["value"].ToString();
            nom.TypeObjeto = data["results"]["bindings"][i]["o"]["type"].ToString();
            nombre.Add(nom);
            //Para ver el nombre del objeto
            string strObjeto;
            string strSujeto;
            strObjeto = nom.Objeto;
            strSujeto = nom.Sujeto;

            //texbox1.text = ("Posicion del Objeto: " + i + " nombre objeto: " + strObjeto);
            if (nom.Sujeto != " " && nom.TypeSujeto == "uri" && nom.Objeto != " " && nom.TypePredicado == "uri")
            {
                
                origin = Instantiate(sphere, vectorSujeto, Quaternion.identity);
                origin.GetComponent<Renderer>().material.color = Color.red;

                var traza = origin.AddComponent<LineRenderer>();
                traza.startWidth = traza.endWidth = .2f;
                traza.useWorldSpace = true;

                traza.positionCount = 2;
                traza.SetPosition(0, vectorSujeto);
                destino = Instantiate(sphere, new Vector3(Random.Range(-30, -46), Random.Range(30, 37), -66.31f), Quaternion.identity);
                destino.GetComponent<Renderer>().material.color = Color.blue;
                //var po = origin.transform.position;
                //var de = destino.transform.position;
                traza.SetPosition(1,destino.transform.position);
                //Debug.DrawLine(po,de, Color.black);
                Debug.Log("Posicion del Objeto: " + i + " nombre objeto: " + strObjeto);
                //Para poner el texto de las esferas de objeto
                textGo.transform.position = destino.transform.position;
                //Para poner el texto de las esferas de objeto
                textSujeto.transform.position = origin.transform.position;

                TextMesh textMesh = textGo.AddComponent<TextMesh>();
                TextMesh textMeshSujeto = textSujeto.AddComponent<TextMesh>();

                textMesh.text = strObjeto;
                textMeshSujeto.text = strSujeto;
                //textGo.Color = new Color(1, 0, 1, 0.5f); //violeta transparente al 50%   100%, 64.7%, 0%, 1
                textMesh.color = new Color(0, 255, 0, 1);
                textMeshSujeto.color = new Color(100, 64.7f, 0, 1);
            }
        }

    }
}
