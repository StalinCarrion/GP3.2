using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;




public class QR : MonoBehaviour {
    private WebCamTexture camTexture;
    private Rect screenRect;

    //public string texto;
    // Use this for initialization
    void Start () {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }

        if (OVRInput.GetDown(OVRInput.Button.DpadDown))
        {
            SceneManager.LoadScene("Demo");

        }
    }

    void OnGUI()
    {
        //Texture2D myQR = generateQR("prueba");

        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if (result != null)
            {
                Debug.Log("DECODE TEXT FROM QR:"+result.Text);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
            //throw;
        }
        
        //if (GUI.Button(new Rect(300, 300, 256, 256), myQR, GUIStyle.none)) { }
    }

    private static Color32[] Encode(string textForEncoding,
    int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    static Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
    

}

