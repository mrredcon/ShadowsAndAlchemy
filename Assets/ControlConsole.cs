using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ControlConsole : MonoBehaviour
{
    [SerializeField] private SpriteRenderer trainBackground;
    [SerializeField] private Sprite newBackgroundSprite;
    [SerializeField] private Volume volume;
    [SerializeField] private TrainManager trainManager;
    [SerializeField] private Image curtain;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float fadeTimer = 3.0f;
    [SerializeField] private float curtainDelay = 5.0f;
    [SerializeField] private Animator[] trainWheels;
    [SerializeField] private ScrollingBackground scrollingBackground;
    [SerializeField] private ScrollingBackground scrollingForeground;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            int enemies = trainManager.GetEnemyCount();
            if (enemies > 0)
            {
                return;
            }

            EndGame();            
        }
    }

    private void EndGame()
    {
        StopTrain();

        trainBackground.sprite = newBackgroundSprite;
        VolumeProfile profile = volume.profile;
        profile.TryGet(out ColorAdjustments color);
        color.colorFilter.value = new Color(0.5f, 0.25f, 0.25f);

        StartCoroutine(ShowCurtain());
    }

    private void StopTrain()
    {
        // Stop the wheels
        foreach (Animator animator in trainWheels)
        {
            animator.speed = 0.0f;
        }

        // Stop scrolling the foreground and background
        scrollingBackground.Stop();
        scrollingForeground.Stop();
    }

    private IEnumerator ShowCurtain()
    {
        timeText.text = "Time: " + Math.Round(gameManager.GetStopwatch(), 2);

        yield return new WaitForSeconds(curtainDelay);

        float timer = 0.0f;
        while (timer < fadeTimer) {
            timer += Time.deltaTime;
            curtain.color = new Color(r: 0.0f, g: 0.0f, b: 0.0f, a: timer / fadeTimer * 1.0f);
            victoryText.color = new Color(r: 1.0f, g: 1.0f, b: 1.0f, a: timer / fadeTimer * 1.0f);
            timeText.color = new Color(r: 1.0f, g: 1.0f, b: 1.0f, a: timer / fadeTimer * 1.0f);
            yield return null;
        }
    }
}
