/*
 *This script has functionallity of explode blue plane object by colliding player plane.
 */


using UnityEngine;

public class BlueEnemyExplode : MonoBehaviour {

    //Private    
    //Classes object which initialize for inheritance functions and methods. 
    PlayerManager playerClassObj;
    GameObject player;
    GameManager gameManagerObj;

    //Public
    //Animator controller for both explosion prefabs.
    public Animator enemyExplodeAnim, playerExplodeAnim;

    
    void Start () {
        //Here player class could be search for PLAYER tag object and that parse to player class object.
        player = GameObject.FindGameObjectWithTag("Player");
        playerClassObj = player.GetComponent<PlayerManager>();
        gameManagerObj = FindObjectOfType<GameManager>();           //Find GameManager script object.
        
    }
	
	
	void Update () {
        //This check condition is Game start yet or not. This help to find current player object in realtime.
        if (gameManagerObj.startGame )
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerClassObj = player.GetComponent<PlayerManager>();
            gameManagerObj.currentPlayerDead = false;               //this desable flag to check condition 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Basic looking for Player object collision.
        if (other.gameObject.tag == "Player")
        {
            PlayerCollisionFunction(other.gameObject);
        }
    }

    //This function has ability to make instance of explode object prefabs and play their animation on player collide.
    void PlayerCollisionFunction(GameObject planeObj)
    {
        playerClassObj.playerControl = false;
        gameManagerObj.currentPlayerDead = false;
        Animator plrEx = Instantiate(playerExplodeAnim, planeObj.transform.position, planeObj.transform.localRotation);
        Animator enmyEx = Instantiate(enemyExplodeAnim, planeObj.transform.position, planeObj.transform.localRotation);
        enmyEx.SetTrigger("enemyExplode");
        Destroy(plrEx.gameObject, 1);
        plrEx.SetTrigger("playerExplode");
        Destroy(enmyEx.gameObject, 1);      //here instance of object gets destroy.
    }
}
