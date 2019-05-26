/*
 *  This is similar to  player bullet class where here player get explode once collide with enemy bullet/rocket.
 *  
 */

using UnityEngine;

public class EnemyBulletManager : MonoBehaviour 
{
    PlayerManager playerClassObj;           //Player class object
    public Animator  playerExplodeAnim;     //player plane explode animation    
    GameObject player;
    GameManager gameManagerObj;             //Gamemanager class object

    //Find for player tag object.
    private void Start()
    {        
        gameManagerObj = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerClassObj = player.GetComponent<PlayerManager>();
    }
    private void Update()
    {
        Destroy(this.gameObject, 3);
        if (gameManagerObj.startGame)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerClassObj = player.GetComponent<PlayerManager>();
            gameManagerObj.currentPlayerDead = false;            
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if ( gameManagerObj.startGame && other.gameObject.tag == "Player" )
        {
            PlayerCollisionFunction(other.gameObject);
        }        
    }
        
    //This makes fales to player controller and destroye player from the scene.
    void PlayerCollisionFunction(GameObject planeObj)
    {        
        playerClassObj.playerControl = false;
        gameManagerObj.currentPlayerDead = false;
        Animator ani = Instantiate(playerExplodeAnim, planeObj.transform.position, planeObj.transform.localRotation);
        ani.SetTrigger("playerExplode");
        Destroy(this.gameObject);
        Destroy(ani.gameObject, 1);        
    }
}


