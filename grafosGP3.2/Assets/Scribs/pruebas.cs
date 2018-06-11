using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;

public class pruebas : MonoBehaviour
{
    public GameObject origin;
    public GameObject destino;
    public GameObject texto;
    //Objeto donde se guarda la esfera
    public GameObject sphere;
    //para generar el minimo y maximo en la posicion random
    public int min, max;
    //Creacino de la lista que contendra todo
    public List<Nombres> nombre = new List<Nombres>();
    public Text inputText;
    public int i;
    string obtener="";
    Coroutine cH;
    public GameObject input;
    string Input
    {
        get { return inputText.text; }
        set { inputText.text = value; }
    }

    public void InitializeH()
    {

        if (cH != null)
        {
            StopCoroutine(cH);
            GameObject[] todo;
            todo = GameObject.FindGameObjectsWithTag("esferas");
            for (int i = 0; i < todo.Length; i++)
            {
                Destroy(todo[i].gameObject);
            }
            Object.Destroy(origin);
            Object.Destroy(destino);
            Debug.Log("Aqui paro la corutina");
        }
        cH = StartCoroutine(ieH());
        Debug.Log("cH empezo como corutina");
    }
    public void StopH()
    {
        if (cH != null)
        {
            StopCoroutine(cH);
        }
    }
    private IEnumerator ieH()
    {
        obtener = inputText.text;
        Debug.Log("Que hay: "+obtener);
        string textoO = obtener.Trim();   
        int nEsferas = 7;
        //link de la consulta donde se sustraen los datos
        WWW www = new WWW("http://es-la.dbpedia.org/sparql?default-graph-uri" +
            "=&query=select+%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2F"+textoO+ "%" +
            "3E+%3Fp+%3Fo+where+%7B%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2F"+textoO+ "%3E" +
            "+%3Fp+%3Fo%7D+LIMIT+" + 20 + "&format=application%2Fsparql-results%2Bjson&timeout=0&debug=on");
        //espera cuando se carge los datos
        yield return www;
        //para presentar en consola
        print(www.text);
        //leer los datos presentados
        JsonData data = JsonMapper.ToObject(www.text);
        var pRed = GeneratedPosition();

        for (int i = 0; i < nEsferas; i++)
        {
            GameObject textGo = new GameObject("Objeto");
            GameObject textSujeto = new GameObject("Sujeto");

            var pBlue = GeneratedPosition();
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
                origin = Instantiate(sphere, pRed, Quaternion.identity);
                origin.GetComponent<Renderer>().material.color = Color.red;
                //origin.name = "origen"+i;
                origin.tag = "esferas";

                var traza = origin.AddComponent<LineRenderer>();
                traza.startWidth = traza.endWidth = .2f;
                traza.useWorldSpace = true;

                traza.positionCount = 2;
                traza.SetPosition(0, pRed);
                destino = Instantiate(sphere, pBlue, Quaternion.identity);
                destino.GetComponent<Renderer>().material.color = Color.blue;
                //destino.name = "destino" + i;
                destino.tag = "esferas";

                traza.SetPosition(1, pBlue);
                
                
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
                textGo.tag = "esferas";
                textSujeto.tag = "esferas";
                //textSujeto.tag = "";
            }
        }

    }
   
    Vector3 GeneratedPosition()
    {
        int x, y, z;
        x = Random.Range(min, max);
        y = Random.Range(min, max);
        z = Random.Range(min, max);
        return new Vector3(x, y, z);

    }
 
} 