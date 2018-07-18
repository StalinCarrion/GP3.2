using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;
using System.Globalization;

public class pruebas : MonoBehaviour
{
    string testPalabra="";
    //Obtengo la posicion que quiero para el sujeto
    public Vector3 vectorSujeto;
    public Transform TransforEsfera;
    //Fin
    //Inserto los objetos que quiero mostrar al ejecutar
    public GameObject origin;
    public GameObject destino;
    //public GameObject texto;
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
        //obtengo el texto del inpput
        obtener = inputText.text;
        //lo transformo a String
        string textoO = obtener.Trim();
        //transformo en minusculas
        string a = textoO.ToLower();
        //La primera va a ser mayuscula
        string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(a);
        Debug.Log("LO QUE SE OBTIENE DEL INPUT "+ resul);
        int nEsferas = 7;
        //link de la consulta donde se sustraen los datos
        WWW www = new WWW("http://es-la.dbpedia.org/sparql?default-graph-uri" +
            "=&query=select+%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2F"+ resul + "%" +
            "3E+%3Fp+%3Fo+where+%7B%3Chttp%3A%2F%2Fes-la.dbpedia.org%2Fresource%2F"+ resul + "%3E" +
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
            string strPredicado;
            string strSujeto;
            //se declaran las palabras que se van a mostrar en el entorno
            string palabraSujeto;
            string palabtaPredicado;
            string palabraObjeto;
            //Guardar los datos en los strings
            strObjeto = nom.Objeto;
            strSujeto = nom.Sujeto;
            strPredicado = nom.Predicado;
            

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
                //Debug.Log("Posicion del Objeto: " + i + " nombre objeto: " + strObjeto);
                //Para poner el texto de las esferas de objeto
                textGo.transform.position = destino.transform.position;
                //Para poner el texto de las esferas de objeto
                textSujeto.transform.position = origin.transform.position;
                

                TextMesh textMesh = textGo.AddComponent<TextMesh>();
                TextMesh textMeshSujeto = textSujeto.AddComponent<TextMesh>();
                //se optiene la nueva palabra

                palabraSujeto = ObtenerURL(strSujeto);
                palabtaPredicado = ObtenerURL(strPredicado);
                //palabraObjeto = ObtenerURL(strObjeto);


                Debug.Log("strSujeto " + strSujeto);
                Debug.Log("Nuevo Sujeto " + palabraSujeto);

                Debug.Log("strPredicado" + strPredicado);
                Debug.Log("Nuevo Predicado " + palabtaPredicado);

                Debug.Log("strOBJETO " + strObjeto);
                //Debug.Log("NuevoObjeto " + palabraObjeto);


                //-39.13  31.5  -72.4



                //aqui cambio para comprobar si se envia el nuevo nombre
                textMesh.text = palabtaPredicado;
                textMeshSujeto.text = palabraSujeto;
                //muestra el texto obtenido de la consulta
                //textMesh.text = strObjeto;
                //textMeshSujeto.text = strSujeto;

                //textGo.Color = new Color(1, 0, 1, 0.5f); //violeta transparente al 50%   100%, 64.7%, 0%, 1
                textMesh.color = new Color(0, 255, 0, 1);
                textMeshSujeto.color = new Color(100, 64.7f, 0, 1);
                //los tag sirven para eliminar el texto cuando le de al boton enter
                textGo.tag = "esferas";
                textSujeto.tag = "esferas";

                textGo.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                textSujeto.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
            }
        }

    }

    public string ObtenerURL(string palabra)
    {
        int longitud = palabra.Length;
        string nPalabra1 = palabra.Substring(0, 33);
        int tamanio = longitud - 33;
        string nPalabra2 = palabra.Substring(33, tamanio);
        string nPalabra;
        nPalabra1 = "stalin";

        nPalabra = nPalabra1 + nPalabra2;
        //Debug.Log("palabra "+palabra.Substring(42, longitud));
        return nPalabra;
    }

    public void Start()
    {
        //
        testPalabra = "http://www.ontologydesignpatterns.org/ont/d0.owl#Location";
        string nPalabra = "";
        int longitud = testPalabra.Length;
        nPalabra = ObtenerURL(testPalabra);
        //Debug.Log("long " + longitud);
        Debug.Log("aaaaaa " + nPalabra);
        //
        //string a ="123456789";
        //string n = a.Substring(0, 5);
        //Debug.Log("n "+n);
        //int b = a.Length - 5;
        //string c = n + a.Substring(5,b);
        //Debug.Log("nueva palabra "+c);

    }


} 