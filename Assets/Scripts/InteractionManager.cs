using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractionManager : MonoBehaviour{

    [SerializeField]
    private RigidbodyFirstPersonController playerController;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Image handImage;

    [SerializeField]
    private PaperPanel paperPanel;
    
    [SerializeField]
    private PasswordPanel passwordPanel;
    
    [SerializeField]
    private float interactDistance;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Light lamp;

    [SerializeField]
    private AudioSource audioAnomalZone;

    [SerializeField]
    private AudioSource audioTakeMedKit;

    private Collider currentColliderAnomalZone;

    // пароль от сейфа введен правильно	
    private bool isSafePasswordCorrect;
    // дверца сейфа разблокирована	
    private bool isSafeDoorUnlock;

    private void Start()
    {
        // отключаем руку
        handImage.gameObject.SetActive(false);
        // отключаем панель с запиской
        paperPanel.SetPanelActive(false);
	// коллайдер текущей аномальной зоны присваиваем null
	currentColliderAnomalZone = null;
	// пароль сейфа введен правильно
	isSafePasswordCorrect = false;
        // дверца сейфа разблокирована	
        isSafeDoorUnlock = false;
    }

   
    private void Update()
    {
       Safe safe;
       // проверка нахождения игрока с камерой внутри сферического коллайдера аномальных зон (tag="AnomalZone")
       if (currentColliderAnomalZone != null)
       {
	  // аномальная зона - да
	  if (currentColliderAnomalZone.bounds.Contains(transform.position))
  	  {
         	  // проиграть звук при нахождении игрока в аномальной зоне
		  if (!audioAnomalZone.isPlaying)
		  {
        		audioAnomalZone.Play();
		  }

        	  // выключить фонарь
        	  lamp.gameObject.GetComponent<Light>().intensity = 0;

 	  }

	  // аномальная зона - нет
	  else
	  {
        	  // остановить проигрывание звука при выходе игрока из аномальной зоны
		  if (audioAnomalZone.isPlaying)
		  {
			audioAnomalZone.Stop();
		  }

		  // включить фонарь
        	  lamp.gameObject.GetComponent<Light>().intensity = 100;

	  }
        }

        Ray ray = new Ray (transform.position, transform.forward);

        RaycastHit raycastHit;
        if(Physics.Raycast(ray, out raycastHit, interactDistance, layerMask))
	{
            // если смотрю на аномальную зону
            if(raycastHit.transform.tag == "AnomalZone")
            {
		    // встретилась новая аномальная зона, нужно сохранить коллайдер этой аномальной зоны в currentColliderAnomalZone
		    // руку отображать не надо при входе в аномальную зону
                    currentColliderAnomalZone = raycastHit.collider;
            }
	    else
	    {
                // если смотрю на аптечку
		// руку отображать не надо при виде аптечки
                if(raycastHit.transform.tag == "MedKit"){
                    // пополнить "жизни" у игрока
                    player.AddLifes(10);
                    audioTakeMedKit.Play();
                    // уничтожить аптечку
                    Destroy(raycastHit.transform.gameObject);

	        }	  
		else

		{

	    	    //  отображение руки для сейфа, двери, заметки
	            // если рука не отображается
        	    if(!handImage.gameObject.activeSelf)
            		{
                	// показать картинку руки
			handImage.enabled = true;
                	handImage.gameObject.SetActive(true);
            		}
    
    	            // если смотрю на сейф
                    if(raycastHit.transform.tag == "Safe" && isSafePasswordCorrect)
		    {
                    	safe = raycastHit.transform.GetComponent<Safe>();
                    	if(safe != null)
		    	{
				// переменная isSafeDoorUnlock нужна,
				// чтобы при первом виде на сейф, где введен правильный пароль (isSafePasswordCorrect = true) - дверца открылась,
				// в дальнейшем при втором(и далее...) видах на сейф (isSafePasswordCorrect = true):
				//  дверца может быть в состоянии открыта/закрыта (управляется нажатием E при изображении руки)
				if(!isSafeDoorUnlock)
				{
					safe.UnlockOpenDoor();
					isSafeDoorUnlock = true;
				}
		     	}
                    }	  
		}
	     }

            // если нажата клавиша Е
            if(Input.GetKeyDown(KeyCode.E)){
                // если смотрю на сейф
    		if(raycastHit.transform.tag == "Safe"){
                    safe = raycastHit.transform.GetComponent<Safe>();
                    if(safe != null)
		    {
	  		if (isSafePasswordCorrect)
			{
				// дверь сейфа открыть/закрыть
				safe.ChangeDoorState();
			}
			else
			{
				// включаем панель ввода пароля сейфа
			        passwordPanel.SetPanelActive(true);
		                //отключаем игрока
        		        playerController.enabled = false;
	                	handImage.enabled = false;

			}

                    }
                }
                // если смотрю на записку
                if(raycastHit.transform.tag == "Paper"){
                    Paper paper = raycastHit.transform.GetComponent<Paper>();
                    if(paper != null)
		    {
		    // сообщить двери какая заметка прочтена (по номеру)
		    paper.SendToDoor();
                    // включаем панель, передаем текст, который у каждой Paper свой
		    paperPanel.SetSpellText(paper.spellText);
		    paperPanel.SetPanelActive(true);
                    // отключаем игрока
                    playerController.enabled = false;

                    }
                }
                // если смотрю на дверь
                if(raycastHit.transform.tag == "Door"){
                    // проиграть звук закрытой двери
                    Door door = raycastHit.transform.GetComponent<Door>();
                    if(door != null)
		    {
                        door.ChangeDoorState();
                    }
                }
            }

	    // если пароль от сейфа введен правильно
	    if(passwordPanel.PasswordCorrect())
	    {
			isSafePasswordCorrect = true;			
	    }

            if(Input.GetKeyDown(KeyCode.Escape))
		{
                // выключаем панель ввода пароля сейфа
                passwordPanel.SetPanelActive(false);
                // выключаем панель прочтения записки
                paperPanel.SetPanelActive(false);
                // включаем игрока
                playerController.enabled = true;
		// включить руку
	        handImage.enabled = true;

                }
	    else
	    {
            	if(isSafePasswordCorrect)
		{
                	// выключаем панель ввода пароля сейфа
                	passwordPanel.SetPanelActive(false);
                	// включаем игрока
                	playerController.enabled = true;
	        }
	    }
        }
	else
	{
            //выключаем картинку руки
            handImage.gameObject.SetActive(false);
         
        }
    }
}
