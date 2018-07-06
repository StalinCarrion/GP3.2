/***
 * Author: Yunhan Li 
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace VRKeyboard.Utils {
    public class KeyboardManager : MonoBehaviour {

        //List<string> words = new List<string>();
        //string myText = "";
        //public InputField textAutocomplete;
        //PAra desaparecer la esfera
        public GameObject esferaEnvolvente;

        
        #region Public Variables
        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        public bool isUppercase = false;
        public int maxInputLength;
        
        [Header("UI Elements")]
        public Text inputText;
        public GameObject prueba;
        //public GameObject prueba2;
        int opcion =0 ;
        
        [Header("Essentials")]
        public Transform characters;
        #endregion

        #region Private Variables
        //pruebas objpruebas = new pruebas();
        public string Input
        {
            get { return inputText.text; }
            set { inputText.text = value; }
        }
        private Dictionary<GameObject, Text> keysDictionary = new Dictionary<GameObject, Text>();

        private bool capslockFlag;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake() {
            //Debug.Log("Que hay aqui: "+Input);
            for (int i = 0; i < characters.childCount; i++) {
                GameObject key = characters.GetChild(i).gameObject;
                Text _text = key.GetComponentInChildren<Text>();
                keysDictionary.Add(key, _text);

                key.GetComponent<Button>().onClick.AddListener(() => {
                    GenerateInput(_text.text);
                });
            }

            capslockFlag = isUppercase;
            CapsLock();
        }
        #endregion

        #region Public Methods
        public void Backspace() {
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.One))
            {
                if (Input.Length > 0)
                {
                    Input = Input.Remove(Input.Length - 1);
                }
                else
                {
                    return;
                }
            }

            
        }
        
        public void Enter()
        {
            //PAra el GEarVR
            //OVRInput.Update();
            //if (OVRInput.Get(OVRInput.Button.One))
            //{
            //    prueba.GetComponent<pruebas>().InitializeH();
            //    esferaEnvolvente.GetComponent<Renderer>().enabled = false;
            //    //    words.Add(inputText.text);
            //    //Debug.Log("Que se guarda: " + words);
            //}
            prueba.GetComponent<pruebas>().InitializeH();
            ////esferaEnvolvente.GetComponent<Renderer>().enabled = false;

        }


        public void Clear() {
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.One))
            {
                Input = "";
            }          
        }

        public void CapsLock() {
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.One))
            {
                //if (Input.Length > 0)
                //{
                //    Input = Input.Remove(Input.Length - 1);
                //}
                //else
                //{
                //    return;
                //}

                if (capslockFlag)
                {
                    foreach (var pair in keysDictionary)
                    {
                        pair.Value.text = ToUpperCase(pair.Value.text);
                    }
                }
                else
                {
                    foreach (var pair in keysDictionary)
                    {
                        pair.Value.text = ToLowerCase(pair.Value.text);
                    }
                }
                capslockFlag = !capslockFlag;
            }            
        }
        #endregion

        #region Private Methods
        public void GenerateInput(string s) {

            OVRInput.Update();

            if (OVRInput.Get(OVRInput.Button.One))
            {
                if (Input.Length > maxInputLength)
                {
                    return;
                }
                Input += s;
            }
        }

        private string ToLowerCase(string s) {
            
                return s.ToLower();
            
            
        }

        private string ToUpperCase(string s) {
            
                return s.ToUpper();

        }
        #endregion
        // ObtenerText

        //private void OnGUI()
        //{
        //    string oldString = myText;
        //    //myText = GUI.TextField(newRect(10, 10, 200, 20), myText);
        //    myText = GUI.TextField(new Rect(10, 10, 200, 20), myText);
        //    if (!string.IsNullOrEmpty(myText) && myText.Length > oldString.Length)
        //    {
        //        List<string> found = words.FindAll(w => w.StartsWith(myText));
        //        if (found.Count > 0)
        //        {
        //            //myText = found[0];
        //            textAutocomplete.text = found[0];
        //            print(found.Count);
        //        }
        //    }
        //}
    }
}