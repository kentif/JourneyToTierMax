using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Respawn 
    public Transform respawnPoint; // Keeps track of where the player we respawn at 

    // Player stats 
    public float playerLife = 3;   // How much health the player currently has 
    public int currentCoins = 0;   // How many coins has the player collected 
    private float playerMaxHealth = 3; // What is the max health the player can have 

    // Rigidbody 
    private Rigidbody2D rigidbody2D; // Controls player speed 

    // Tags 
    private const string deathTag = "Death";
    private const string healthTag = "Health";
    private const string coinTag = "Coin";
    private const string respawnTag = "Respawn";
    private const string finishTag = "Finish";

    // UI 
    public Image heartImage;            // Update the Heart Image of the player 
    public TextMeshProUGUI coinText;    // Update the text showing coins collected 
    public GameObject CoinParent;       // Parent we check to see how many coins are in the level 
    public int maxCoins = 50;

    // Auido 
    public AudioSource deathSFX;  // Death sound effect 
    public AudioSource coinSFX;   // Coin pick up sound effect 

    void Start()
    {
        // Connect to the rigidbody 
        rigidbody2D = GetComponent<Rigidbody2D>();
        // Looks at the Coin Parent Game Object and check how many children it has, thats' the number
        // Of coins that are in the level, each time we destroy a coin the childCount would lower 
        // So we save the inforomation at the start of the game 
        // Updates the UI to show the proper values of the level 
        maxCoins = CoinParent.transform.childCount;
        coinText.text = "TIER" + currentCoins;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag){
            // Coin tag, dicates what happens after player touches coins 
            case coinTag:
                {
                    // Plays Sound Effect 
                    //coinSFX.Play();
                    // Increase the value of coins by 1
                    currentCoins = currentCoins + 10;
                    // Updates the UI 
                    maxCoins = CoinParent.transform.childCount;

                    coinText.text = "TIER " + currentCoins;
                    if (currentCoins >= 50)
                    {
                        coinText.text = "TIER MAX";

                    }
                    // Destroys the coins
                    Destroy(collision.gameObject);
                    break;
                }
            // Death tag, dicates what happens after player touches something that could kill them
            case deathTag:
                {
                    // Play Sound Effect
                    //deathSFX.Play();
                    // Make the speed zero 
                    rigidbody2D.velocity = Vector2.zero;
                    // Moves the player to the respawn point 
                    transform.position = respawnPoint.position;
                    // Take away players life 
                    playerLife--;
                    // Updates the UI 
                    //heartImage.fillAmount = playerLife / playerMaxHealth;
                    // If the player has lost all of their lives reset the level 
                    if (playerLife <= 0)
                    {
                        // Gets the name of the level we're currently in 
                        string currentLevel = SceneManager.GetActiveScene().name;
                        // Reload that level 
                        SceneManager.LoadScene(currentLevel);
                    }
                    break;
                }
            // Health tag, dicates what happens after player touches a heart
            case healthTag:
                {
                    // Checks if the player is full on health, if they are nothing happens 
                    if(playerLife < playerMaxHealth)
                    {
                        // If the player is missing health we increase their life
                        playerLife++;
                        // Update the UI to show new health 
                        //heartImage.fillAmount = playerLife / playerMaxHealth;
                        // Destroy the health object 
                        Destroy(collision.gameObject);
                    }
                    break;
            }
            // Respawn tag, dicates what happens after player touches a respawnpoint 
            case respawnTag:
                {
                    // We look for a child object called RespawnPoint, and we copy it's position
                    // This will be the new spot the player respawns from 
                    respawnPoint.position = collision.transform.Find("RespawnPoint").position;
                    break;
                }
            // Finish tag, dicates what happens after player touches a respawnpoint 
            case finishTag:
                {
                    // Gets the name of the next level from the end game object 
                    string nextLevel = collision.gameObject.GetComponent<GameEnd>().nextLevel;
                    // Loads us into that next level 
                    SceneManager.LoadScene(nextLevel);
                    break;
                }
        }
    }

}
