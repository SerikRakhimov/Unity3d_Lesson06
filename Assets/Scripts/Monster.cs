using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour{

    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private NavMeshAgent navMesh;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform eyes;

    [Header("Camera Settings")]
    [SerializeField]
    private Transform deathCamera;

    [SerializeField]
    private Transform deathCameraPoint;

    [SerializeField]
    private AudioSource audioScream;

    // текущее состояние персонажа
    private string state = "idle";
    // жив ли персонаж
    private bool isAlive = true;
    private float waitTime = 0f;

    // режим повышенной встревоженности
    private bool highAlertness = false;

    // уровень встревоженности
    private float alertnessLevel = 20f;

    private void Start(){
        navMesh.speed = 1f;
        animator.speed = 1f;
        deathCamera.gameObject.SetActive(false);
    }

    private void Update(){
        if(!isAlive){
            // делать что-то...
            return;
        }
        // передаем в аниматор скорость персонажа
        animator.SetFloat("velocity", navMesh.velocity.magnitude);
        if(state == "idle"){
            GoToRandomPoint();
        }

        if(state == "walk"){
            StopWalking();
        }

        if(state == "search"){
            SearchForPlayer();
        }

        if(state == "chase"){
            ChaseForPlayer();
        }

        if(state == "hunt"){
            HuntForPlayer();
        }

        if(state == "kill"){
            SetDeathCamera();
        }
    }

    // метод для проверки поля зрения
    public void CheckSight(){
        if(!isAlive){
            return;
        }

        RaycastHit hit;
        if(Physics.Linecast(eyes.position, playerTransform.position, out hit)){
            if(hit.collider.tag == "Player"){
                if(state == "kill"){
                    return;
                }
                // обновляем состояние
                state = "chase";
                navMesh.speed = 2f;
                animator.speed = 2f;
	        // проиграть звук при обнаружении игрока
        	audioScream.Play();
		// отнять "жизни" у игрока
		player.TakeAwayLifes(10);
            }
        }
    }

    private void GoToRandomPoint(){
        // генерируем рандомную координату
        Vector3 randomPosition = Random.insideUnitSphere * 20f;
        // создаем переменную для хранения информации
        NavMeshHit navMeshHit;
        // ставим точку на mesh
        NavMesh.SamplePosition(transform.position + randomPosition, out navMeshHit, 20f, NavMesh.AllAreas);
        
        // если находится в режиме повышенной встревоженности
        if(highAlertness){
            // ставим точку на mesh рядом с игроком
             NavMesh.SamplePosition(playerTransform.transform.position + randomPosition, out navMeshHit, 20f, NavMesh.AllAreas);
             // постепенно понижать уровень тревоги, если не нашел игрока
             alertnessLevel -= 5f;
             // если уровень тревоги минимальный
             if(alertnessLevel <= 0){
                 // выйти из режима повышенной тревоги
                 highAlertness = false;
                 // сбросить скорость
                 navMesh.speed = 1f;
                 animator.speed = 1f;
             }
        }
        
        // отправляем персонажа к сгенерированной точке
        navMesh.SetDestination(navMeshHit.position);
        // обновляем состояние
        state = "walk";
    }

    private void SearchForPlayer(){
        // если время ожидания больше 0
        if(waitTime > 0){
            // отнимаем время каждую секунду
            waitTime -= Time.deltaTime;
            // вращаем персонажа на месте
            transform.Rotate(0, 120f * Time.deltaTime, 0);
        }else{ // иначе
            // переходим в состояние idle
            state = "idle";
        }
    }

    private void StopWalking(){
        // если оставшееся расстояние меньше чем конечное и не просчитывается новый маршрут
        if(navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending){
            // обновляем состояние
            state = "search";
            waitTime = 5f;
        }
    }

    private void ChaseForPlayer(){
        navMesh.SetDestination(playerTransform.position);
        // получаем расстояние между персонажем и игроком
        float distance = Vector3.Distance(transform.position, playerTransform.transform.position);
        // если игрок оторвался
        if(distance > 10f){
            // искать игрока
            state = "hunt";
        }else if(navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending){
            Player playerController = playerTransform.GetComponent<Player>();
            if(playerController.isAlive){
                state = "kill";
                KillPlayer();
            }
        }
    }

    private void HuntForPlayer(){
        if(navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending){
            // обновляем state
            state = "search";
            // указываем время ожидания
            waitTime = 5f;
            // включаем уровень встревоженности
            highAlertness = true;
            // устанавливаем уровень тревоги
            alertnessLevel = 20f;
            // проверяем зону видимости
            CheckSight();
        }
    }

    private void KillPlayer(){
        // запускаем анимацию
        animator.SetTrigger("Kill");
        // обновляем значение переменной isAlive и игрока
        playerTransform.GetComponent<Player>().isAlive = false;
        // отключаем управление игрока
        playerTransform.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        // включаем объект DeathCamera
        deathCamera.gameObject.SetActive(true);
        // помещаем deatchCamera в ту позицию, где была камера игрока
        deathCamera.position = Camera.main.transform.position;
        deathCamera.rotation = Camera.main.transform.rotation;
        // отключаем камеру игрока
        Camera.main.gameObject.SetActive(false);
        // перезапустить игру
        Invoke("RestartGame", 2f);
    }

    private void RestartGame(){
        // перезапускаем уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetDeathCamera(){
        // изменяем позицию
        deathCamera.transform.position = Vector3.Slerp(
            deathCamera.position, 
            deathCameraPoint.position,
            10f * Time.deltaTime);
        // изменяем поворот
        deathCamera.transform.rotation = Quaternion.Slerp(
            deathCamera.rotation, 
            deathCameraPoint.rotation,
            10f * Time.deltaTime
        );
        // замедлим анимацию монстра
        animator.speed = 0.4f;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}