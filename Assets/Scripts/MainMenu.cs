using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationsHandler androidNotificationsHandler;
    [SerializeField] private IOSNotificationHandler iosNotificationsHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRecharge;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) {return;}

        CancelInvoke();

        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}";
        //Let player play only if they have energy
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);//Check if we have enough energy
        //Check if player is out of energy
        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);//Check if energy should be regenerated

            if(energyReadyString == string.Empty) { return; }

            System.DateTime energyReady = System.DateTime.Parse(energyReadyString);//Convert from string into DateTime object

            if(System.DateTime.Now > energyReady)//Check if enough time passed to regenerate
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            } else {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged) ,(energyReady - System.DateTime.Now).Seconds);
            }
        }
        energyText.text = $"Energy Left : {energy} ";
    }
    private void EnergyRecharged()
    {
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Energy Left : {energy} ";
    }

    public void Play()
    {
        //Consume energy when play the game
        if(energy < 1) { return; }
        energy -= 1;
        PlayerPrefs.SetInt(EnergyKey, energy);

        if(energy < maxEnergy)//Check if energy is less than max so regeneration can start
        {
            System.DateTime energyReady = System.DateTime.Now.AddMinutes(energyRecharge);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
            if (energy == maxEnergy)
            {
                //Show a notifications when energy is fully restored
#if UNITY_ANDROID
                androidNotificationsHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
                iosNotificationsHandler.ScheduleNotification(energyRecharge);
#endif
            }
        }

        

        SceneManager.LoadScene(1);
    }
}
