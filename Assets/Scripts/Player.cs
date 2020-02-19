using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Text yourLifeText;
    [SerializeField]
    private Text gameOverText;
    // жив ли игрок
    public bool isAlive;
    private int lifes;

    private void Start()
    {
	// отключить текст "Game Over!"
	gameOverText.enabled = false;
    	lifes = 100;
	LifeTextUpdate();
    }
    
    // обновить текст "Lifes" на экране
    private void LifeTextUpdate()
    {
	yourLifeText.text = "Lifes: " + lifes;
        
    }
    // при прохождении аптечки +10
    public void AddLifes(int lifesAdd)
    {
	lifes += lifesAdd;  
	LifeTextUpdate();
    }
    // при обнаружении игрока монстром -10
    public void TakeAwayLifes(int lifesTakeAway)
    {
	lifes -= lifesTakeAway;

	if (lifes < 0)
	{
		lifes = 0;
	}

	LifeTextUpdate();

	// если кол-во "жизней" = 0
	if (lifes == 0)
	{	
		// вывести текст "Game Over!"
		gameOverText.enabled = true;
		// остановить игру
		Time.timeScale = 0;
		Application.Quit();
	}
    }

}
