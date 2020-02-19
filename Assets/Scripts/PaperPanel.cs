using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperPanel : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Text yourPaperText;

    private void Start()
    {
        // отключаем панель на старте игры
        root.gameObject.SetActive(false);
    }
    
    public void SetPanelActive(bool state)
    {
        root.gameObject.SetActive(state);
    }

    public void SetSpellText(string spellText)
    {
        yourPaperText.text = spellText; 
    }

}
