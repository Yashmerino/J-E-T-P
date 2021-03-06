using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    // Components
    public PlayerController gamePlayer;
    public TextMeshProUGUI scoreText;
    public HeartSystem uiHeartSystem;

    // Vars
    public float respawnDelay = 0.5f;
    public int fruits = 0;

    // Audio
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with "PlayerController" script
        gamePlayer = FindObjectOfType<PlayerController>();
        uiHeartSystem = FindObjectOfType<HeartSystem>();

        // Play music
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Respawns the player at last checkpoint
    public void Respawn()
    {
        StartCoroutine("RespawnCoroutine");
    }

    public IEnumerator RespawnCoroutine()
    {
        // Losing score on death
        LostFruits();

        // Freeze rigid body so you can't move it
        (gamePlayer.GetComponent<Rigidbody2D>()).constraints = RigidbodyConstraints2D.FreezeAll;
        // Play disappearing animation
        gamePlayer.playerAnim.Play("Disappearing");

        // Wait for 2 seconds before doing other stuff
        yield return new WaitForSeconds(respawnDelay);

        // Change player position to respawn point
        gamePlayer.transform.position = gamePlayer.respawnPoint;

        // Play appearing animation
        gamePlayer.playerAnim.Play("Appearing");
        // Set the player active again
        gamePlayer.gameObject.SetActive(true);
        // Reset rigidbody constraints
        (gamePlayer.GetComponent<Rigidbody2D>()).constraints = RigidbodyConstraints2D.FreezeRotation;
        // Restart hearts
        uiHeartSystem.RestartHearts();
    }

    // Add fruits when a fruit is collected
    public void AddFruits(int numberOfFruits)
    {
        fruits += numberOfFruits;
        // Change text to current amount of collected fruits
        scoreText.text = "Fruits: " + fruits;
    }

    public void LostFruits()
    {
        // If there are more than 2 fruits then remove 2 fruits
        if(fruits >= 2)
            fruits -= 2;
        else // If there are less than 2 fruits then set number of fruits to 0
            fruits = 0;

        // Change text to current amount of collected fruits
        scoreText.text = "Fruits: " + fruits;
    }
}
