using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //So you can use SceneManager

public class restartButton : MonoBehaviour
{
    public void reiniciarNivel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Load scene called Game
    }
}
