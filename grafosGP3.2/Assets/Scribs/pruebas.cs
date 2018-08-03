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
    ////variable para obtener Data.txt
    string filePath = "Scribs/Links";
    private Dictionary<string, List<string>> palabraContenedor = new Dictionary<string, List<string>>();   
    ////fin
    //Leo el documento .txt
    public void FileDataReader()
    {
        
        string applicationPath = string.Format("{0}/{1}.txt", Application.dataPath, filePath.Trim());
        string[] stringData = File.ReadAllLines(applicationPath);
        //string[] stringData2 = File.ReadAllLines(applicationPath);
        List<string> contenidoValor = new List<string>();
        Debug.Log("tamaño del string "+stringData.Length);
        for (int i = 1; i < stringData.Length; i++)
        {
            if (palabraContenedor.ContainsKey(stringData[i].Split(separator: new char[] { ';' })[0]))
            {

                palabraContenedor[stringData[i].Split(separator: new char[] { ';' })[0]].Add(stringData[i].Split(separator: new char[] { ';' })[1]);
            }
            else
            {
                
                contenidoValor.Add(stringData[i].Split(separator: new char[] { ';' })[1]);
                palabraContenedor.Add(stringData[i].Split(separator: new char[] { ';' })[0],contenidoValor);

            }
            
        }
    }

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
    //public int i;
    string obtener="";
    Coroutine cH;
    //public GameObject input;
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
        //Para leer el archivo
        FileDataReader();

        //obtengo el texto del inpput
        obtener = inputText.text;
        //lo transformo a String
        string textoO = obtener.Trim();
        //transformo en minusculas
        string a = textoO.ToLower();
        //La primera va a ser mayuscula
        string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(a);
        Debug.Log("LO QUE SE OBTIENE DEL INPUT "+ resul);
        //link de la consulta donde se sustraen los datos
        WWW www = new WWW("https://dbpedia.org/sparql?default-graph" +
            "-uri=http%3A%2F%2Fdbpedia.org&query=CONSTRUCT+%0D%0A%7B%3FuriPais+" +
            "%3Ftype+%3Fclass+.+%0D%0A%3FuriPais+%3Fetiqueta+%3FnombrePais+.+%7" +
            "D%0D%0AWHERE%7B%0D%0Avalues+%3Ftypev%7Brdf%3Atype%7D%0D%0Avalues" +
            "+%3Fetiquetav%7Brdfs%3Alabel%7D%0D%0Avalues+%3Fclass+%7Bdbo%3ACountry" +
            "%7D%0D%0A%3FuriPais+%3Ftype+%3Fclass.%0D%0A%3FuriPais+%3Fetiqueta+%3" +
            "FnombrePais.%0D%0A%3FuriPais+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2" +
            "Fowl%23sameAs%3E+%3Fo.%0D%0AFILTER+%28%3FnombrePais+%3D+%22" + resul + "%22%" +
            "40en%29%0D%0A%7D&format=application%2Fsparql-results%2Bjson&CXML" +
            "_redir_for_subjs=121&CXML_redir_for_hrefs=&timeout=30000&debug=on&run=+Run+Query+");
        //espera cuando se carge los datos
        yield return www;
        //para presentar en consola
        print(www.text);
        //leer los datos presentados
        JsonData data = JsonMapper.ToObject(www.text);

        //Para ver el nombre del objeto
        string strObjeto;
        string strPredicado;
        string strSujeto;
        int num = 28;
        //se declaran las palabras que se van a mostrar en el entorno
        string palabraSujeto1;
        string palabraSujeto2;
        string palabraPredicado1;
        string palabraPredicado2;
        string palabraObjeto1;
        string palabraObjeto2;
        string nuevoSujeto;
        string nuevoPredicado;
        string nuevoObjeto;

        for (int i = 0; i < 3; i++)
        {
            GameObject textGo = new GameObject("ObjetoSTALIN");
            GameObject textSujeto = new GameObject("SujetoSTALIN");
            GameObject textPredicado = new GameObject("PredicadoSTALI");
            //se crear una variable de Nombre
            Nombres nom = new Nombres();
            //se ingresa a cada variable el dato que se sustrae
            nom.Sujeto = data["results"]["bindings"][i]["s"]["value"].ToString();
            nom.TypeSujeto = data["results"]["bindings"][i]["s"]["type"].ToString();
            nom.Predicado = data["results"]["bindings"][i]["p"]["value"].ToString();
            nom.TypePredicado = data["results"]["bindings"][i]["p"]["type"].ToString();
            nom.Objeto = data["results"]["bindings"][i]["o"]["value"].ToString();
            nom.TypeObjeto = data["results"]["bindings"][i]["o"]["type"].ToString();
            nombre.Add(nom);
            Debug.Log("Cuanto "+nombre.Count);
            //aqui
            //Guardar los datos en los strings
            strObjeto = nom.Objeto;
            strSujeto = nom.Sujeto;
            strPredicado = nom.Predicado;
            //texbox1.text = ("Posicion del Objeto: " + i + " nombre objeto: " + strObjeto);
            if (nom.Sujeto != " " && nom.TypeSujeto != " " && nom.Objeto != " ")
            {
                //se crea la esfera sujeto
                origin = Instantiate(sphere, vectorSujeto, Quaternion.identity);
                origin.GetComponent<MeshRenderer>().material = MaSujeto;
                //origin.name = "origen"+i;
                origin.tag = "esferas";
                origin.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                var traza = origin.AddComponent<LineRenderer>();
                traza.startWidth = traza.endWidth = 0.04f;
                traza.useWorldSpace = true;
                traza.positionCount = 2;

                //textSujeto.tag = "";
                if (nom.TypeObjeto == "uri")
                {
                    destino = Instantiate(sphere, new Vector3(Random.Range(-37.500f, -40.500f), Random.Range(32.119f, 31.062f), -71.213f), Quaternion.identity);
                    destino.GetComponent<MeshRenderer>().material = MaObjeto;
                    destino.tag = "esferas";
                    destino.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);

                    palabraObjeto1 = ObtenerPrimeraPalabra(strObjeto, 28);
                    Debug.Log("Primera palabra Objeto " + palabraObjeto1);
                    palabraObjeto2 = ObtenerSegundaPalabra(strObjeto, 28);
                    Debug.Log("Segunda palabra Objeto " + palabraObjeto2);

                }
                else
                {
                    destino = Instantiate(cube, new Vector3(Random.Range(-37.500f, -40.500f), Random.Range(32.119f, 31.062f), -71.213f), Quaternion.identity);
                    destino.GetComponent<MeshRenderer>().material = MaObjLiteral;
                    destino.tag = "esferas";
                    destino.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);

                    palabraObjeto1 = ObtenerPrimeraPalabra(strObjeto, 1);
                    Debug.Log("Primera palabra Objeto " + palabraObjeto1);
                    palabraObjeto2 = ObtenerSegundaPalabra(strObjeto, 1);
                    Debug.Log("Segunda palabra Objeto " + palabraObjeto2);
                }

                traza.SetPosition(0, vectorSujeto);

                traza.SetPosition(1, destino.transform.position);
                //traza.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                traza.material = MaPredicado;
                //traza.endWidth = 4;
                //Debug.Log("Posicion del Objeto: " + i + " nombre objeto: " + strObjeto);
                //Para poner el texto de las esferas de objeto
                textGo.transform.position = destino.transform.position;
                //Para poner el texto de las esferas de objeto
                textSujeto.transform.position = origin.transform.position;
                textPredicado.transform.position = traza.GetPosition(1);

                TextMesh textMeshPredicado = textPredicado.AddComponent<TextMesh>();
                TextMesh textMesh = textGo.AddComponent<TextMesh>();
                TextMesh textMeshSujeto = textSujeto.AddComponent<TextMesh>();

                //se optiene la nueva palabra

                palabraSujeto1 = ObtenerPrimeraPalabra(strSujeto, num);
                Debug.Log("Primera palabra Sujeto " + palabraSujeto1);
                palabraSujeto2 = ObtenerSegundaPalabra(strSujeto, num);
                Debug.Log("Segunda palabra Sujeto " + palabraSujeto2);
                palabraPredicado1 = ObtenerPrimeraPalabra(strPredicado, num);
                Debug.Log("Primera palabra predicado " + palabraPredicado1);
                palabraPredicado2 = ObtenerSegundaPalabra(strPredicado, num);
                Debug.Log("Segunda palabra Predicado " + palabraPredicado2);



                nuevoSujeto = UnirPalabras(palabraSujeto1, palabraSujeto2);
                Debug.Log("Nuevo Sujeto " + nuevoSujeto);
                nuevoPredicado = UnirPalabras(palabraPredicado1, palabraPredicado2);
                Debug.Log("Nuevo predicado " + nuevoPredicado);
                nuevoObjeto = UnirPalabras(palabraObjeto1, palabraObjeto2);
                Debug.Log("Nuevo Objeto " + nuevoObjeto);
                //-39.13  31.5  -72.4
                //aqui cambio para comprobar si se envia el nuevo nombre
                textMesh.text = nuevoObjeto; //strObjeto;
                textMeshSujeto.text = resul; //strSujeto;
                //textMeshPredicado.text = nuevoPredicado;
                //muestra el texto obtenido de la consulta
                //textMesh.text = strObjeto;
                //textMeshSujeto.text = strSujeto;

                //textGo.Color = new Color(1, 0, 1, 0.5f); //violeta transparente al 50%   100%, 64.7%, 0%, 1
                textMesh.color = new Color(0, 255, 0, 1);
                textMeshSujeto.color = new Color(100, 64.7f, 0, 1);
                textMeshPredicado.color = new Color(1, 0, 0, 1);
                //los tag sirven para eliminar el texto cuando le de al boton enter
                textGo.tag = "esferas";
                textSujeto.tag = "esferas";
                textPredicado.tag = "esferas";

                textGo.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                textSujeto.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
                textPredicado.transform.localScale = new Vector3(0.1395633f, 0.1395634f, 0.1395634f);
            }
        }

    }

    public List<string> SerachWord(string dataSearch)
    {
        Debug.Log("palabra a buscar " + dataSearch);
        if (string.IsNullOrEmpty(dataSearch))
        {
            return new List<string>();
        }
        if (palabraContenedor.ContainsKey(dataSearch))
        {           
            return palabraContenedor[dataSearch];
        }
        else
        {
            return new List<string>();
        }
    }    
    public string ObtenerPrimeraPalabra(string palabra, int num)
    {   
        string Palabra = palabra.Substring(0, num);
        List<string> listaTEmporal = SerachWord(Palabra);
        string nPalabra = Palabra;
        if (listaTEmporal.Count> 0)
        {
            nPalabra = SerachWord(Palabra)[0];
            Debug.Log("nPalabra " + nPalabra);
        }
        return nPalabra;
    }
    public string ObtenerSegundaPalabra(string palabra, int num)
    {
        int longitud = palabra.Length;
        int tamanio = longitud - num;
        string nPalabra = palabra.Substring(num, tamanio);
        return nPalabra;
    }
    public string UnirPalabras(string palabra1, string palabra2)
    {
        string nuevaPalabra="";

        nuevaPalabra = palabra1 + palabra2;

        return nuevaPalabra;
    }
} 