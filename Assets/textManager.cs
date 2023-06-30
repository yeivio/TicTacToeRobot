using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textManager : MonoBehaviour
{
    public GameObject textoUI;

    // Start is called before the first frame update
    void Start()
    {
        this.textoUI.GetComponent<TextMeshProUGUI>().enabled = false;

    }

   public void setText(string texto){
    this.textoUI.GetComponent<TextMeshProUGUI>().text = texto;
   }

    public void disableText()
    {
        this.textoUI.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void enableText()
    {
        this.textoUI.GetComponent<TextMeshProUGUI>().enabled = true;
    }
}
