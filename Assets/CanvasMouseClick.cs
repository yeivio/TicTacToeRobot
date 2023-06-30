using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CanvasMouseClick : MonoBehaviour {
    [SerializeField] private Sprite circleImage,crossImage;
    [SerializeField] private Image boxImage;
    [SerializeField] private Button boxButton;
    [SerializeField] private int posX, posY;
    [SerializeField] private AudioSource audioSourcePop;

    public event Action<int, int, Vector3> interaccionTablero;

    public static float MAQUINA_PITCH = 0.65f;
    public static float PLAYER_PITCH = 1;

    private static bool turno = true; //True = Jugador | False = Maquina

    public void Start()
    {
        turno = true;
    }

    public void BoxPressed()
    {
        if (!turno)
            return;
        turno = !turno;
        audioSourcePop.pitch = PLAYER_PITCH;
        audioSourcePop.Play();
        boxImage.sprite = circleImage;
        boxButton.interactable = false;
        if (interaccionTablero != null)
            interaccionTablero(this.posX, this.posY, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
    }

    public void simulateBoxPressed()
    {
        audioSourcePop.pitch = MAQUINA_PITCH;
        audioSourcePop.Play();
        boxImage.sprite = crossImage;
        boxButton.interactable = false;
        turno = !turno;
    }

    public void disableBox()
    {
        boxButton.interactable = false;
    }

    public int getPositionX()
    {
        return posX;
    }

    public int getPositionY()
    {
        return posY;
    }

}
