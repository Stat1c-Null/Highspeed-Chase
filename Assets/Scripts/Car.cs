using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGain = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    private int steerValue;

    // Update is called once per frame
    void Update()
    {
        //Increase speed with every frame
        speed += speedGain * Time.deltaTime;

        //Steer the car
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        //Move Car Forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //Check for collision with obstacles
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void Steer(int value)
    {
        steerValue = value; 
    }
}
