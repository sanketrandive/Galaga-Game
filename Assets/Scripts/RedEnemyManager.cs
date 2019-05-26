/*
 *This script has red plane working functionallity. This make instance of red plane of group in a list and start transforming on their position.
 * In certain time this plane can attack on player plane.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyManager : MonoBehaviour
{
    //Private
    GameObject player;
    bool stopInstanceRedPlane, redPlaneTransform;                                       //flag to check condision in realtime.
    Vector2 curr = new Vector2(0, 6);                                                   //This value provide initial posion of the plane.
    Vector2 next = new Vector2(0, 0);                                                   //Value can help to transform plane to zero postion into the screen.
    float speedOfEnemy;                                                                 //Here enemy speed perform with deltaTime method. NOT EDITABLE
    float redAttackTimer, redAttackSpeed, redPlaneDistance;                             //This helps to get Attack speed, distance and attack timer variable NOT EDITABLE 
    List<GameObject> rocketNumberList = new List<GameObject>();                         //This stores rockets into the list.
    int randomRedPlaneNumber;                                                           //This generates random number between the list.
    bool checkBulletDrop = false;                                                       //Condtion to check is bullet dropped or not.
    int numberOfRedPlane = 12;                                                         //This is EDITABLE but need to check postion for every change.

    //Public
    public int totalRocketFire = 2;                                                     //Set how many rockets required to drop on player plane EDITABLE
    public List<GameObject> redPlaneList = new List<GameObject>();                      //This is list to store red planes.
    public GameObject redPlanePrefab;
    public GameObject bulletObjectPrefab;                                               //Get Enemy bullet object prefab.

    //Class Objects
    GameManager gameManagerObj;

    //Find Player object with tag
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerObj = FindObjectOfType<GameManager>();
    }

    void Update() {
        //Here boolean check for is Game start yet.
        if (gameManagerObj.startGame )
        {
            player = GameObject.FindGameObjectWithTag("Player");                     //This perform every frame to find Player object in scene.
            //This is main condition to get attack red plane to Player.
            if (redPlaneList.Count >= 0 && !gameManagerObj.currentPlayerDead)
            {
                RedPlaneAttack();                                               //Function inherited.
            }
            //This check How many of instance of objects required into the scene.
            if ( !stopInstanceRedPlane)
            {
                StartCoroutine(SpwanPlane(numberOfRedPlane, curr));            //Plane Instantiate into the scene.   
                //print("number");          
            }
            //Plane get initial position which setted into the vector variable above..
            if (redPlaneTransform)
            {
                speedOfEnemy += Time.deltaTime;                                     //start timer to get smooth tranform speed.
                ///print("zal dusara");
                for (int i = 0; i < redPlaneList.Count; i++)
                {
                    next = new Vector2(0, 2);                                        //this define row and column of position.
                    next.x = Mathf.Clamp(transform.position.x + 0.5f + i, -4.3f, 4.3f);//This defines the area for the tranformation.
                    redPlaneList[i].transform.position = Vector2.Lerp(curr, next, 1 * speedOfEnemy);//The half of the planes get first row position.
                }
                for (int j = 6; j < redPlaneList.Count; j++)
                {
                    next = new Vector2(0, 3);
                    next.x = Mathf.Clamp(transform.position.x + 0.5f + j - 6, -4.3f, 4.3f);
                    redPlaneList[j].transform.position = Vector2.Lerp(curr, next, 1 * speedOfEnemy);//Here, The half of the planes get second row position.
                }
                if (speedOfEnemy >= 1 && speedOfEnemy <= 1.2)                        //if within speed condition gets false to stop transformation of the plane.
                {
                    redPlaneTransform = false;
                    speedOfEnemy = 0;
                }
            }
            
        }
    }

    //This function has Instantiation functionallity to set position. This has 2 overload method.
    IEnumerator SpwanPlane( int redValue, Vector2 spwanPos) {
        //This stops when i equals to red value and instance specific objects.
        for (int i = 0; i < redValue; i++)
        {
            GameObject goR = Instantiate(redPlanePrefab, spwanPos, transform.localRotation) as GameObject;  //Instantiation process.
            redPlaneList.Add(goR);                                                                          //Add into the list which defined.
            goR.transform.parent = gameObject.transform;                                                    //this moved to parent object into the hierarchy
           //Condision to stop instantiation process.
            if (i <= redValue)
                stopInstanceRedPlane = true;
            else
                stopInstanceRedPlane = false;
        }
        //This enumerator start tranformation of the object after 2 seconds.
        yield return new WaitForSeconds(2);
        redPlaneTransform = true;     
    }

    //Attck functionallity to Player with certain delay. 
    void RedPlaneAttack()
    {
        redAttackTimer += Time.deltaTime;                                                                //Timer to attack in certain time.
        if (redAttackTimer >= 5 && redAttackTimer <= 8f)                                                 //Within every 5-8 time this enemy attack to player. EDITABLE. 
        {
            try     //Because of List of planes sometime gets remove or empty so this TRY n CATCH stop throwing exceptions.
            {
                redAttackSpeed += Time.deltaTime / 50;                                               //Speed of plane while attacking
                redPlaneDistance = Vector2.Distance(redPlaneList[randomRedPlaneNumber].transform.position, player.transform.position);  //Distance get between player and current red enemy.
                redPlaneList[randomRedPlaneNumber].transform.position = Vector2.Lerp(redPlaneList[randomRedPlaneNumber].transform.position, player.transform.position, redAttackSpeed);
                //Here start attacking to player position using Lerp method.
                //Whenever Distance lessthan 3.5 this stops attacking and following to player postion and add self gravity.
                if (redPlaneDistance < 3.5f)
                {
                    EnemyBombAttack();                                                  //Function inheritance
                    redPlaneList[randomRedPlaneNumber].transform.parent = null;         //Removed from parent object.
                    redPlaneList[randomRedPlaneNumber].GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    redPlaneList.Remove(redPlaneList[randomRedPlaneNumber]);            //This remove current gameobject from the list.
                    redAttackSpeed = 0;
                    //print("oooooooooo");
                    redAttackTimer = 0;
                    randomRedPlaneNumber = UnityEngine.Random.Range(0, redPlaneList.Count);  //This generate next plane number into the list.      
                    rocketNumberList.Clear();                                               //This clears the rocket numbers list to avoid exceptions.
                }
            }
            catch (Exception e)
            {
                //print(e);
            }
        }
        //This condition help to check if timer reset hasnt worked then here by default set it to 0.
        if (redAttackTimer > 12)
        {
            randomRedPlaneNumber = 0;       //Plane Number within list = 0
            redAttackTimer = 0;
        }
    }

    //The red plan has ability to drop rockets on player plane and this function has control to rocket object.
    void EnemyBombAttack()
    {
        //this makes number of rocket instance and instantiate rocket objects. 
        for (int i = 0; i < totalRocketFire; i++)
        {
            if (rocketNumberList.Count < totalRocketFire)
            {
                //Creates Go gameobect and make instance of the rocket prefabs.
                GameObject gO = Instantiate(bulletObjectPrefab, redPlaneList[randomRedPlaneNumber].transform.position, redPlaneList[randomRedPlaneNumber].transform.localRotation);
                //Here to get some normal deley between rockets this condion writen.
                if (!checkBulletDrop)
                {
                    checkBulletDrop = true;
                    gO.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
                }
                else
                {
                    checkBulletDrop = false;
                    gO.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                }
                rocketNumberList.Add(gO);                   //Here instance rocket object added into rocket list.
            }
        }
    }

    
}
