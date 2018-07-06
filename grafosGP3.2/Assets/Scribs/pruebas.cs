using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;

public class pruebas : MonoBehaviour
{
    //Obtengo la posicion que quiero para el sujeto
    public Vector3 vectorSujeto;
    public Transform TransforEsfera;
    //Fin
    //Inserto los objetos que quiero mostrar al ejecutar
    public GameObject origin;
    public GameObject destino;
    public GameObject texto;
    //materiales que van a ir en los objetos
    public Material MaObjeto;
    public Material MaSujeto;
    public Material MaObjLiteral;
    public Material MaPredicado;

    //fin
    //Objeto donde se guarda la esfera y el cubo
    public GameObject sphere;
    public GameObject cube;

    
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
        //obtengo la posicion del sujeto que va hacer fija
        vectorSujeto = TransforEsfera.position;
        //fin

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
            "+%3Fp+%3Fo%7D+LIMIT+" + 7 + "&format=application%2Fsparql-results%2Bjson&timeout=0&debug=on");
        //espera cuando se carge los datos
        yield return www;
        //para presentar en consola
        print(www.text);
        //leer los datos presentados
        JsonData data = JsonMapper.ToObject(www.text);

        for (int i = 0; i < nEsferas; i++)
        {
            GameObject textGo = new GameObject("Objeto");
            GameObject textSujeto = new GameObject("Sujeto");

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
            if (nom.Sujeto != " " && nom.TypeSujeto != " " && nom.Objeto != " " )
            {
                //se crea la esfera sujeto
                origin = Instantiate(sphere, vectorSujeto, Quaternion.identity);
                origin.GetComponent<MeshRenderer>().material = MaSujeto;
                //origin.name = "origen"+i;
                origin.tag = "esferas";
                origin.transform.localScale= new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                var traza = origin.AddComponent<LineRenderer>();
                traza.startWidth = traza.endWidth = 0.04f;
                traza.useWorldSpace = true;
                traza.positionCount = 2;
                
                //textSujeto.tag = "";
                if (nom.TypeObjeto=="uri")
                {
                    destino = Instantiate(sphere, new Vector3(Random.Range(-37.500f, -40.500f), Random.Range(32.119f, 31.062f), -71.213f), Quaternion.identity);
                    destino.GetComponent<MeshRenderer>().material = MaObjeto;                 
                    destino.tag = "esferas";
                    destino.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);

                }
                else
                {
                    destino = Instantiate(cube, new Vector3(Random.Range(-37.500f, -40.500f), Random.Range(32.119f, 31.062f), -71.213f), Quaternion.identity);
                    destino.GetComponent<MeshRenderer>().material = MaObjLiteral;
                    destino.tag = "esferas";
                    destino.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                }

                traza.SetPosition(0, vectorSujeto);

                //traza.startWidth = 0.06f;

                traza.SetPosition(1, destino.transform.position);
                //traza.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                traza.material = MaPredicado;
                //traza.endWidth = 4;
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
                textGo.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                textSujeto.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
            }
        }

    }
   
   
 
} 