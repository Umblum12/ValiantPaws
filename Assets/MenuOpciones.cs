using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpciones : MonoBehaviour
{
    public void CargarMenu()
    {
        Debug.Log("xddd");
        SceneManager.LoadScene("MenuScene");
    }

    public void SalirMenu()
    {         Debug.Log("Saliendo del menu");
        SceneManager.LoadScene("SampleScene");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}
