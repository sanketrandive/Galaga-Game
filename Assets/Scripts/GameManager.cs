/*
 *This is main script where many object has depedency of this script class.
 *In this script has score class to initiallize score system.
 *Also look for player dead or alive.
 *Game Start and Stop functionallity.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    //private
    GameObject playerPrefab;
    List<GameObject> playerNumberList = new List<GameObject>();                 //Total player into the list
    Vector2 playerPos = new Vector2(6, -3.6f);                                  //Current player Position
    Vector2 playerNextPos = new Vector2(0, -4.7f);                              //Player next default position

    //public
    public int numberOfPlayer = 1;                                              //EDITABLE number of player plane into the game.
    public bool startGame = false;                                              //to check in every plane script 
    public bool currentPlayerDead = false;                                      //Check if player alive or dead
    public bool gameStop = false;                                               //Games get stop after this true
    public Text playerNumberText;                                               //Unity UI class object to show text on screen
    public Text totalScore, firstScore, secondScore, thirdScore, gameScreenTest;//Unity UI to store player score 

    //Class Objects -- Here all 3 plane script classes object defined --
    BlueEnemyManager blueEnemyClassObj;             
    RedEnemyManager redEnemyClassObj;
    GreenEnemyManager greenEnemyClassObj;
    
    //To initiallize first this calls.
    void Awake () {
        playerNumberText.text = "Total Plane :   " +numberOfPlayer;
        playerPrefab = (GameObject)Resources.Load("Prefabs/PlayerPlane", typeof(GameObject));       //This call prefabs from the resources automatically.
	}

    //Find the script class object for all plane object.
    private void Start()
    {
        
        gameScreenTest.text = " GALAGA \n\n Press  Spacebar  to  start game.";          //This defines the text into UI.
        blueEnemyClassObj = GameObject.FindObjectOfType<BlueEnemyManager>();
        redEnemyClassObj = GameObject.FindObjectOfType<RedEnemyManager>();
        greenEnemyClassObj = GameObject.FindObjectOfType<GreenEnemyManager>();
    }

   //Here all functionallity of the gameplay and player initiallize.
    void Update () {
        
        totalScore.text = ScoreSystemClass.totalScore.ToString();                   //store realtime score into UI
        //This check if number of player less than 0 then stop game and make game over.
        if (numberOfPlayer <= 0)
        {
            startGame = false;            
            gameScreenTest.text = "Game  Over \n\nPress Spacebar to Restart.";
            gameStop = true;
        }
        //Here checks if SPACE key enter then gamesplay should start.
        if (Input.GetKeyDown(KeyCode.Space) && !startGame && !gameStop)
        {
            startGame = true;
            PlayerSpwan();                              //Player spwan into the scene with defined numbers.
            gameScreenTest.text = " ";
        }      
        //whenever game start player next spawn function called and countinously check for is all other players alive or not.
        if (startGame)
        {
            StartCoroutine (SpwanNextPlayer());         //Function inherited
            if (blueEnemyClassObj.blueAlive)            //If true then look for enemy list.
            {
                if (blueEnemyClassObj.bluePlaneList.Count <= 0 && redEnemyClassObj.redPlaneList.Count <= 0 && greenEnemyClassObj.greenPlaneList.Count <= 0)
                    SceneManager.LoadScene(0);          //Condition satisfy then restart the level.
            }
        }       
        //On game over this give chance to restart game.
        if (Input.GetKeyDown(KeyCode.Space) && gameStop )
            SceneManager.LoadScene(0);                  //Restart the level.

        //Switch statement to check how many player died and store their score into the UI.
        switch (numberOfPlayer)
        {
            case 1:
                thirdScore.text = "3 UP  : " + ScoreSystemClass.totalScore.ToString();
                break;
            case 2:
                secondScore.text = "2 UP  : " + ScoreSystemClass.totalScore.ToString();
                break;
            case 3:
                firstScore.text = "1 UP  : " + ScoreSystemClass.totalScore.ToString();
                break;
            default:
                thirdScore.text = "3 UP : 00";
                secondScore.text = "2 UP : 00 ";
                firstScore.text = "1 UP  : 00";
                totalScore.text = "000";
                ScoreSystemClass.totalScore = 00;
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Application.Quit();
        }
            
        
    }

    //This spwan player object into the list with number of set amount. 
    void PlayerSpwan()
    {
        for (int i = 0; i < numberOfPlayer; i++)
        {
            GameObject go = Instantiate(playerPrefab, playerPos, Quaternion.identity);          //Instantiate the Player prefabs and add into the list.
            playerNumberList.Add(go);
            playerNumberList[0].tag = "Player";                                                 //Set tag to active player plane into the scene.
            playerNumberList[0].transform.position = playerNextPos;                             //Get default position into the scene.
            StartCoroutine(PlayerControllerTurnOn());                                           //Call function to get player control.
        }
    }
    //This get next player once previous one die. This look for next number into the linked list.
    IEnumerator SpwanNextPlayer()
    {        
        var tempPlayer = playerNumberList[0].GetComponent<PlayerManager>();
        //Temp variable to store current player object and remove and get replaced with next one.
        if (tempPlayer.playerDead)              //if this satisfy then player get removed and replaced bu next one.
        {
            numberOfPlayer--;
            if (playerNumberList.Count > 1)
            {
                tempPlayer.playerDead = false;
                currentPlayerDead = true;
                tempPlayer.enabled = false;
                yield return new WaitForSeconds(1);
                //playernumberList[0].tag = "Untagged";
                Destroy(playerNumberList[0]);
                playerNumberList.Remove(playerNumberList[0]);
                playerNumberList[0].tag = "Player";
                yield return new WaitForSeconds(2);                 //This hold delay to reactivate control to next player plane.
                playerNumberList[0].transform.position = playerNextPos;
                playerNumberList[0].GetComponent<PlayerManager>().enabled = true;                
                tempPlayer.playerControl = true;                               
                playerNumberText.text = "Total Plane :   " + numberOfPlayer;   
            }            
        }
    }

    //This Hold Player controller on start game.
    IEnumerator PlayerControllerTurnOn()
    {
        yield return new WaitForSeconds(4.5f);
        playerNumberList[0].GetComponent<PlayerManager>().enabled = true;
    }
}

// This is seperate class for the score system
public class ScoreSystemClass { 
    public static int totalScore=0;                       //store total number of score.
    public static void ScoreFunction(int planeScore)
    {
        totalScore += planeScore;
        //Debug.Log("totalScore " + totalScore);
    }
}