using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class obtenerTextoTXT : MonoBehaviour {
    
	public Text word, prefix;
    string filePath = "Scribs/Data";
    public string dataSearch;
    private int count =1;
    private Dictionary<string, string> wordsContent = new Dictionary<string, string>();

    public void FileDataReader()
    {
        string applicationPath = string.Format("{0}/{1}.txt", Application.dataPath, filePath.Trim());
        string[] stringData = File.ReadAllLines(applicationPath);

        for (int i = 1; i < stringData.Length; i++)
        {
            wordsContent.Add(stringData[i].Split(separator: new char[] { ';' })[0], stringData[i].Split(separator: new char[] { ';' })[1]);
        }
    }

    public void SerachWord()
    {
        if (string.IsNullOrEmpty(dataSearch))
        {
            return;
        }
        if (wordsContent.ContainsKey (dataSearch))
        {
            word.text = dataSearch;
            prefix.text = wordsContent[dataSearch];
        }
    }
    private void Start()
    {
        FileDataReader();
    }
}
