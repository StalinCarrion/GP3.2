﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRKeyboard.Utils
{
    public class KeyboardManager : MonoBehaviour
    {
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
        public ObtenerPredicado obtenerPredicado;
        public pruebas pru;
        //public GameObject prueba2;
        int opcion = 0;

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
        private void Awake()
        {
            //Debug.Log("Que hay aqui: "+Input);
            for (int i = 0; i < characters.childCount; i++)
            {
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
        public void Backspace()
        {
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
        public Material skyone;

        public void Enter()
        {
            //PAra el GEarVR
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.One))
            {

                //prueba.GetComponent<pruebas>().InitializeH();
                //pru.InitializeH();
                prueba.GetComponent<pruebas>().InitializeH();
                RenderSettings.skybox = skyone;
                StartCoroutine(obtenerPredicado.LecturaDatos());
                esferaEnvolvente.GetComponent<Renderer>().enabled = false;
            }
            //prueba.GetComponent<pruebas>().InitializeH();
            //pru.InitializeH();
            prueba.GetComponent<pruebas>().InitializeH();
            RenderSettings.skybox = skyone;
            StartCoroutine(obtenerPredicado.LecturaDatos());
            esferaEnvolvente.GetComponent<Renderer>().enabled = false;

        }


        public void Clear()
        {
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.One))
            {
                Input = "";
            }
        }

        public void CapsLock()
        {
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
        public void GenerateInput(string s)
        {

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

        private string ToLowerCase(string s)
        {

            return s.ToLower();


        }

        private string ToUpperCase(string s)
        {

            return s.ToUpper();

        }
        #endregion
    }
}