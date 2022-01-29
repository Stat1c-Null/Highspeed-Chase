using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    [Header("Car Values")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float speedGain = 0.2f;
    [SerializeField] private float turnSpeed = 200f;
    public float health = 50f;
    public float maxHealth = 100f;
    public float healthRestore = 50f;
    public float collisionDamage = 25f;
    public float gas = 100f;
    public float maxGas = 100f;
    public float gasRestore = 20f;
    private float gasConsume;
    [Header("UI Element")]
    [SerializeField] private Image healthFrontUI = null;
    [SerializeField] private Image gasFrontUI = null;

    private int steerValue;

    // Update is called once per frame
    void Update()
    {
        //Increase speed with every frame until it reaches max
        if(speed < maxSpeed)
        {
            speed += speedGain * Time.deltaTime;
        } else {
            speed = maxSpeed;
        }
        

        //Steer the car
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        //Consume Gas
        float gasConsume = Random.Range(0.5f, 1.5f);
        gas = gas - gasConsume * Time.deltaTime; 

        //Move Car Forward if player has gas
        if(gas > 0) 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        

        //Update UI
        UpdateUI();

        //Check if player died or has too much gas
        if (health <= 0) 
        {
            SceneManager.LoadScene("MainMenuScene");
        } else if(health > maxHealth) 
        {
            health = maxHealth;
        } else if(gas > maxGas) {
            gas = maxGas;
        }
    }

    //Check for collision with obstacles, health , gas pick up
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            health -= collisionDamage;
            Destroy(other.gameObject);
        } else if(other.CompareTag("Health") && health < maxHealth)
        {
            health += healthRestore;
            Destroy(other.gameObject);
        //Insta kill player if he touches end wall
        } else if(other.CompareTag("EndWall")) {
            SceneManager.LoadScene("MainMenuScene");
        }
        //Regen gas
        else if(other.CompareTag("Gas") && gas < maxGas) {
            gas += gasRestore;
            Destroy(other.gameObject);
        }
    }

    public void Steer(int value)
    {
        steerValue = value; 
    }

    void UpdateUI()
    {
        healthFrontUI.fillAmount = health / maxHealth;
        gasFrontUI.fillAmount = gas / maxGas;
    }
}
