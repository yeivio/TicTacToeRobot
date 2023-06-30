/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

private struct info{
    public int[] siguienteMov;
    public int[,] tablero;
    public int valor;
    public int numMov;
};
private class backup
{
    public static int JUGADOR = 1;
    public static int MAQUINA = 9;
    private static int DEFAULT_MAX_VALUE = int.MinValue;
    private static int DEFAULT_MIN_VALUE = int.MaxValue;
    public static int CONTADOR_FINALES = 0;
    public static int CONTADOR_ACTUALIZACION = 0;

    //StreamWriter writer = new StreamWriter("Assets/MyFile.txt", true);

    public int[] decisionMinimax(int[,] tablero){
        info aux = new info();
        aux.tablero = new int[3,3];
        //aux.tablero = (int[,]) tablero.Clone(); // Copiar el tablero inicial
        for(int i = 0; i<3;i++){
            for(int j = 0; j<3;j++){
                aux.tablero[i,j] = tablero[i,j]; //Copiar el tablero inicial
            }
        }

        
        aux.valor = 0;
        aux.numMov = 0;
        aux.siguienteMov = new int[2]; 
        aux.siguienteMov[0] = DEFAULT_MAX_VALUE;
        aux.siguienteMov[1] = DEFAULT_MAX_VALUE;

        aux = max_valor(aux, DEFAULT_MAX_VALUE, DEFAULT_MIN_VALUE);
        //Debug.Log("resultados:" + CONTADOR_FINALES);
        //Debug.Log("resultados2:" + CONTADOR_ACTUALIZACION);
        //Debug.Log("a:" + aux.siguienteMov[0] + aux.siguienteMov[1]);

        //writer.WriteLine("Resultado:" + aux.siguienteMov[0] + aux.siguienteMov[1]);
        //writer.Close();
        return aux.siguienteMov;
    }


info max_valor(info dato, int alpha, int beta){  
    if(comprobarGanador(dato.tablero,JUGADOR)){ //Gana las cruces
        //dato.valor = -10 - dato.numMov;
        dato.valor = -1;
        CONTADOR_FINALES++;

            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
        return dato;
    }
    if(comprobarGanador(dato.tablero,MAQUINA)){
        //dato.valor = 10 - dato.numMov;
        dato.valor = 1;
        CONTADOR_FINALES++;
            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
            return dato;
    }

    if(!hayCasillasVacias(dato.tablero)){
        dato.valor = 0;
        CONTADOR_FINALES++;
            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
            return dato;
    }

    info aux;
    info resultado = dato;

    resultado.valor = DEFAULT_MAX_VALUE;
        resultado.siguienteMov = new int[2]; //En c# hay que hacer array copia
        resultado.siguienteMov[0] = dato.siguienteMov[0];
        resultado.siguienteMov[1] = dato.siguienteMov[1];
    bool poda = false; //Activar poda

    for(int i = 0; i<3 && !poda;i++){
        for(int j = 0; j<3 && !poda;j++){
            if(dato.tablero[i,j] == 0){ //Casilla libre
                dato.tablero[i,j] = MAQUINA;
                dato.numMov++;
                aux = min_valor(dato, alpha, beta);
                    //writer.WriteLine("antesif:"+aux.valor+resultado.valor);
                    //writer.WriteLine("auxif:"+resultado.siguienteMov[0]+resultado.siguienteMov[1]);

                    if (resultado.siguienteMov[0] == DEFAULT_MAX_VALUE ||aux.valor > resultado.valor){ 
                    resultado.siguienteMov[0] = i;
                    resultado.siguienteMov[1] = j;
                    resultado.valor = aux.valor;   
                    CONTADOR_ACTUALIZACION++;
                        //writer.WriteLine("Actualizo:" + i + j + "," + resultado.valor);
                        //Debug.Log("Actualizo:" + i + j + "," + resultado.valor);
                    
                }

                dato.numMov--;
                dato.tablero[i,j] = 0;

                //Actualizar poda
                if(resultado.valor >= beta){
                    poda = true;
                        //writer.WriteLine("se poda");
                        //writer.WriteLine("poda:" + resultado.valor + alpha);
                    }
                    alpha = Math.Max(alpha, resultado.valor);
                    //writer.WriteLine("viejoalpha:" + resultado.valor +","+ alpha + ",nuevo" + alpha);
                }
            }
    }
        //writer.WriteLine("se devuelve:" + resultado.siguienteMov[0]+ resultado.siguienteMov[1]);
        return resultado;
}

info min_valor(info dato, int alpha, int beta){
    if(comprobarGanador(dato.tablero,JUGADOR)){
        //dato.valor = -10 - dato.numMov;
        dato.valor = -1;
        CONTADOR_FINALES++;
            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
            return dato;
    }
    if(comprobarGanador(dato.tablero,MAQUINA)){
        //dato.valor = 10 - dato.numMov;
        dato.valor = 1;
        CONTADOR_FINALES++;
            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
            return dato;
    }
    if(!hayCasillasVacias(dato.tablero)){
        dato.valor = 0;
        CONTADOR_FINALES++;
            //writer.WriteLine("Res:");
            //imprimirResultados(dato);
            return dato;
    }
    info aux;
    info resultado = dato;
    resultado.valor = DEFAULT_MIN_VALUE;
        resultado.siguienteMov = new int[2];
        resultado.siguienteMov[0] = dato.siguienteMov[0];
        resultado.siguienteMov[1] = dato.siguienteMov[1];

        bool poda = false;

    for(int i = 0; i<3 && !poda;i++){
        for(int j = 0; j<3 && !poda;j++){
            if(dato.tablero[i,j] == 0){ //Casilla libre
                dato.tablero[i,j] = JUGADOR;
                dato.numMov++;
                aux = max_valor(dato, alpha, beta);
                    //writer.WriteLine("antesif:" + aux.valor + resultado.valor);
                    //writer.WriteLine("auxif:" + resultado.siguienteMov[0] + resultado.siguienteMov[1]);
                    if (resultado.siguienteMov[0] == DEFAULT_MAX_VALUE ||aux.valor < resultado.valor){ 
                    resultado.siguienteMov[0] = i;
                    resultado.siguienteMov[1] = j;
                    resultado.valor = aux.valor;
                    CONTADOR_ACTUALIZACION++;
                        //writer.WriteLine("Actualizo:" + i + j + "," + resultado.valor);
                        //Debug.Log("Actualizo:" + i + j + "," + resultado.valor);
                    }

                dato.numMov--;
                dato.tablero[i,j] = 0;   

                //actualizar poda
                if(resultado.valor <= alpha){
                    poda = true;
                        //writer.WriteLine("se poda");
                        //writer.WriteLine("poda:" + resultado.valor + alpha);
                    }
                    beta = Math.Min(beta, resultado.valor);
                    //writer.WriteLine("viejobeta:" + resultado.valor + "," + beta + ",nuevo" + beta);

                }
            }
    }
        //writer.WriteLine("se devuelve:" + resultado.siguienteMov[0] + resultado.siguienteMov[1]);
        return resultado;
    
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


bool hayCasillasVacias(int[,] tablero){
    for(int i = 0; i<3;i++){
        for(int j = 0; j<3;j++){
            if(tablero[i,j] == 0)
                return true;
        }
     
    }
    return false;
}

void imprimirResultados(info dato){
        //aux += "------------------------";
        //Debug.Log("------------------------");
        //Debug.Log("------------------------");

        //writer.WriteLine("Valor:" + dato.valor);
        //Debug.Log("Valor:" + dato.valor);
        imprimirTablero(dato.tablero);
    //Debug.Log("------------------------");
}

void imprimirTablero(int[,] tablero){
        //writer.WriteLine(tablero[0, 0] + "," + tablero[0, 1] + "," + tablero[0, 2]);
        //writer.WriteLine(tablero[1, 0] + "," + tablero[1, 1] + "," + tablero[1, 2]);
        //writer.WriteLine(tablero[2, 0] + "," + tablero[2, 1] + "," + tablero[2, 2]);
        //writer.WriteLine("-------------------------");
        Debug.Log("Tablero:"+ tablero[0,0]+","+tablero[0,1]+","+tablero[0,2]); 
    Debug.Log("Tablero:"+ tablero[1,0]+","+tablero[1,1]+","+tablero[1,2]); 
    Debug.Log("Tablero:"+ tablero[2,0]+","+tablero[2,1]+","+tablero[2,2]); 
    Debug.Log("-------------------------");
}

bool confirmarTablero(int[,] tablero){
    return (tablero[0,0] == 1 && tablero[0,1] == 0 && tablero[0,2] == 9
        && tablero[1,0] == 0 && tablero[1,1] == 0 && tablero[1,2] == 0 
        &&tablero[2,0] == 0 && tablero[2,1] == 0 && tablero[2,2] == 0);
}

}*/