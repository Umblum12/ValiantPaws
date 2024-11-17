using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class IniciarJuego : MonoBehaviour
{
    public void CargarNivel1()
    {
        Debug.Log("FUnciona we");
        SceneManager.LoadScene("Nivel 1"); 
    }
}