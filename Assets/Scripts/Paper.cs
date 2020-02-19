using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{

    [SerializeField]
    private int number;

    [SerializeField]
    public string spellText;

    [SerializeField]
    private Door door;

    public void SendToDoor()
    {
	door.ReadPaper(number);    	
    }

}
