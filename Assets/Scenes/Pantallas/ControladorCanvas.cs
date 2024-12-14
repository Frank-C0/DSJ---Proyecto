using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCanvas : MonoBehaviour
{
    private bool juegoPausado = false; // Estado del juego (pausado o no)
    private bool juegoTerminado = false;
    [SerializeField]
    private GameObject pantallaPausa, pantallaPerdiste, pantallaGanaste;

    void Update()
    {
        // Detectar la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!juegoTerminado){
                if (juegoPausado)
                {
                    ReanudarJuego(); // Si está pausado, reanudar
                }
                else
                {
                    Pausar(); // Si está en ejecución, pausar
                }
            }            
        }

        //Solo para Pruebas
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            TerminarJuego();
        }*/ 
    }

    public void Pausar()
    {
        pantallaPausa.SetActive(true);
        Time.timeScale = 0f; // Detener el tiempo
        juegoPausado = true;
        Debug.Log("Juego pausado");
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1f; // Reanudar el tiempo
        pantallaPausa.SetActive(false);
        juegoPausado = false;
        Debug.Log("Juego reanudado");
    }

    public void TerminarJuego()
    {
        Time.timeScale = 0f;
        juegoTerminado= true;
        pantallaPerdiste.SetActive(true);
        Debug.Log("Juego Terminado");
    }

    public void GanarJuego(){        
        Time.timeScale = 0f;
        juegoTerminado= true;
        pantallaGanaste.SetActive(true);
        Debug.Log("Juego Terminado");
    }

    public void CargarEscena(int escena){
        SceneManager.LoadScene(escena);
    }

    public void PasarEscena(){
        int nescena= SceneManager.GetActiveScene().buildIndex;
        if(nescena==3){
            SceneManager.LoadScene(0);
        }else{
            SceneManager.LoadScene(nescena+1);
        }
    }

    public void RecargarEscena(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegresarMenu(){
        SceneManager.LoadScene(0);
    }
}
