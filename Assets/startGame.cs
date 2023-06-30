using UnityEngine;
using System;
using System.Collections;

public class startGame : MonoBehaviour
{
    public GameObject cruz, circulo, placeMarker;

    public CanvasMouseClick[,] objetosCasillas = new CanvasMouseClick[3, 3];
    private bool turnoJugador;

    private botIA botIA;
    public int[,] tablero;

    public static int JUGADOR = 1;
    public static int MAQUINA = 9;

    public bool tableroLleno = false;

    private textManager textoUI;

 

    // Start is called before the first frame update
    void Start()
    {
        foreach (CanvasMouseClick canvasMouseClick in FindObjectsOfType<CanvasMouseClick>())
        {
            canvasMouseClick.interaccionTablero += this.interaccion;
            objetosCasillas[canvasMouseClick.getPositionX(), canvasMouseClick.getPositionY()] = canvasMouseClick;
        }

        //Inicio del juego
        turnoJugador = true;
        tablero = new int[3, 3];
        botIA = new botIA();
        textoUI = this.GetComponent<textManager>();
   
        for(int i = 0; i<3;i++){
            for(int j = 0; j<3;j++){
                tablero[i,j] = 0;
            }
        }
    }

    public void interaccion(int x, int y, Vector3 posicion)
    {
        if (turnoJugador)
            marcarCasilla(x, y, posicion);
    }
    private void marcarCasilla(int x, int y, Vector3 posicion)
    {
        int[] auxResultado;
        if (turnoJugador)
        {
            tablero[x, y] = JUGADOR;
            auxResultado = botIA.decisionMinimax(tablero);
            turnoJugador = !turnoJugador;
            stopGame(); //Se comprueba si se ha acabado el juego ya
            if (!tableroLleno)
                marcarCasilla(Convert.ToInt32(auxResultado[0]), Convert.ToInt32(auxResultado[1]),
                    objetosCasillas[Convert.ToInt32(auxResultado[0]), Convert.ToInt32(auxResultado[1])].transform.position);
        }
        else
        {//Maquina

            StartCoroutine(pausaMaquina(x,y)); //Pausa
            
        }

    }

    IEnumerator pausaMaquina(int x, int y)
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        objetosCasillas[x, y].GetComponent<CanvasMouseClick>().simulateBoxPressed(); //Hay que simular el click de la máquina
        tablero[x, y] = MAQUINA; //Mejorar
        turnoJugador = !turnoJugador;
        stopGame(); //Se comprueba si se ha acabado el juego ya
    }

    bool comprobarGanador(int[,] tablero, int jugador)
    {
        // Comprobación de filas
        for (int i = 0; i < 3; i++)
        {
            if (tablero[i, 0] == jugador && tablero[i, 1] == jugador && tablero[i, 2] == jugador)
            {
                return true;
            }
        }

        // Comprobación de columnas
        for (int j = 0; j < 3; j++)
        {
            if (tablero[0, j] == jugador && tablero[1, j] == jugador && tablero[2, j] == jugador)
            {
                return true;
            }
        }

        // Comprobación de diagonales
        if (tablero[0, 0] == jugador && tablero[1, 1] == jugador && tablero[2, 2] == jugador)
        {
            return true;
        }

        if (tablero[0, 2] == jugador && tablero[1, 1] == jugador && tablero[2, 0] == jugador)
        {
            return true;
        }
        return false; // Si no se cumple ninguna de las condiciones anteriores, no hay ganador.
    }


    bool hayCasillasVacias(int[,] tablero)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tablero[i, j] == 0)
                {
                    return true;
                }
            }

        }
        return false;
    }
    public void stopGame()
    {
        if (!hayCasillasVacias(tablero))
        {
            textoUI.enableText();
            tableroLleno = true;
            textoUI.setText("EMPATE");
        }
        if (comprobarGanador(tablero, JUGADOR))
        { //Gana las cruces
            textoUI.enableText();
            tableroLleno = true;
            textoUI.setText("VICTORIA");
        }
        if (comprobarGanador(tablero, MAQUINA))
        {
            textoUI.enableText();
            tableroLleno = true;
            textoUI.setText("DERROTA");
        }

        if(tableroLleno)
            foreach (CanvasMouseClick objeto in objetosCasillas)
                if (objeto.GetComponent<CanvasMouseClick>() != null)
                    objeto.GetComponent<CanvasMouseClick>().disableBox();
        
    }
}
