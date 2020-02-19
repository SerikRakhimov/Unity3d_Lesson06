using UnityEngine;

public class Door : MonoBehaviour{
    // логическая переменная для проверки заблокирована ли дверь
    private bool isLocked = true;
    // логическая переменная для проверки открыта ли дверь
    private bool isOpen;
    // угол, в котором дверь открыта полностью
    private float doorOpenAngle = 90f;
    // угол, в котором дверь закрыта полностью
    private float doorClosedAngle = 0f;
    // насколько плавно будет открываться дверь
    private float smooth = 2f;
    // логическая переменная для проверки прочтения 1-ой записки
    private bool isReadPaper1 = false;
    // логическая переменная для проверки прочтения 2-ой записки
    private bool isReadPaper2 = false;
    // логическая переменная для проверки прочтения 3-ой записки
    private bool isReadPaper3 = false;

    private void Update(){
        // если дверь заблокирована
        if(isLocked){
            //выйти из метода
            return;
        }
        
        // если дверь открыта
        if(isOpen){
            // создаем переменную для хранения координат вращения
            Quaternion targetRotationOpen = Quaternion.Euler(0, doorOpenAngle, 0);
            // вращаем дверь в нужный угол
            transform.parent.localRotation = Quaternion.Slerp(
                transform.parent.localRotation, 
                targetRotationOpen,
                smooth * Time.deltaTime
            );
        }else{
            // создаем переменную для хранения координат вращения
            Quaternion targetRotationClosed = Quaternion.Euler(0, doorClosedAngle, 0);
            // вращаем дверь в нужный угол
            transform.parent.localRotation = Quaternion.Slerp(
                transform.parent.localRotation, 
                targetRotationClosed,
                smooth * Time.deltaTime
            );
        }
    }

    public void ReadPaper(int number)
    {

	if (number == 1)
	{
	   isReadPaper1 = true;
	}
	if (number == 2)
	{
	   isReadPaper2 = true;
	}
	if (number == 3)
	{
	   isReadPaper3 = true;
	}

	// разблокировать дверь, если прочтены все три записки
	if(isReadPaper1 && isReadPaper2 && isReadPaper3)
	{
		isLocked = false;
    	}
    }

    public void ChangeDoorState(){
        isOpen = !isOpen;
    }
}
