using UnityEngine;

public class Key : MonoBehaviour{
    [SerializeField]
    private Door door;
    public void PickUp(){
        //door.Unlock();
        Destroy(gameObject);
    }
}