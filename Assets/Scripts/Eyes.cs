using UnityEngine;

public class Eyes : MonoBehaviour{
    [SerializeField]
    private Monster monster;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            monster.CheckSight();
        }
    }
}
