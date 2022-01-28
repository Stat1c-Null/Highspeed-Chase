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
    public float collisionDamage = 25f;
    [Header("UI Element")]
    [SerializeField] private Image healthFrontUI = null;

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

        //Move Car Forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //Update health UI
        UpdateHealthUI();

        //Check if player died
        if (health <= 0) 
        {
            SceneManager.LoadScene("MainMenuScene");
        } else if(health > maxHealth) 
        {
            health = maxHealth;
        }
    }

    //Check for collision with obstacles and health pick up
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            health -= collisionDamage;
            Destroy(other.gameObject);
        } else if(other.CompareTag("Health") && health < maxHealth)
        {
            health += 50;
            Destroy(other.gameObject);
        //Insta kill player if he touches end wall
        } else if(other.CompareTag("EndWall")) {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void Steer(int value)
    {
        steerValue = value; 
    }

    void UpdateHealthUI()
    {
        healthFrontUI.fillAmount = health / maxHealth;
    }
}
