/*
 * This is player bullet controller script. This used to check if bullet collide with enemy or not.
 * Also add define the score to total score.
 */ 
using UnityEngine;

using inheritanceScoreClass = ScoreSystemClass;     //This inheritance of the score system class
public class PlayerBulletManager : MonoBehaviour 
{
    
    public Animator enemyExplodeAnim, playerExplodeAnim;    //Animator for player and enemy explosion       
    //THis is a class object of enemy plane
    BlueEnemyManager blueEnemyClassObj;
    RedEnemyManager redEnemyClassObj;
    GreenEnemyManager greenEnemyClassObj;

    private void Start()
    {
        blueEnemyClassObj = GameObject.FindObjectOfType<BlueEnemyManager>();
        redEnemyClassObj = GameObject.FindObjectOfType<RedEnemyManager>();
        greenEnemyClassObj = GameObject.FindObjectOfType<GreenEnemyManager>();
    }
    private void Update()
    {
        Destroy(this.gameObject, 3);      //this destroy bullet object if bullet in the screen.  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BluePlane")
        {
            inheritanceScoreClass.ScoreFunction(100);
            EnemyCollisionFunction(other.gameObject);
        }

        if (other.gameObject.tag == "RedPlane")
        {
            inheritanceScoreClass.ScoreFunction(200);
            EnemyCollisionFunction(other.gameObject);
        }
        if (other.gameObject.tag == "GreenPlane")
        {
            inheritanceScoreClass.ScoreFunction(300);
            EnemyCollisionFunction(other.gameObject);
        }
        
    }
    
    //This has functionallity to remove enemy object from the list and add their score.
    void EnemyCollisionFunction(GameObject planeObj)
    {
        blueEnemyClassObj.bluePlaneList.Remove(planeObj);
        redEnemyClassObj.redPlaneList.Remove(planeObj);
        greenEnemyClassObj.greenPlaneList.Remove(planeObj);
        Destroy(planeObj);       
        Animator ani = Instantiate(enemyExplodeAnim, planeObj.transform.position, planeObj.transform.localRotation);
        ani.SetTrigger("enemyExplode");
        Destroy(this.gameObject);
        Destroy(ani.gameObject,1);
    }
    
}


