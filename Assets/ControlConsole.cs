using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ControlConsole : MonoBehaviour
{
    [SerializeField] private SpriteRenderer trainBackground;
    [SerializeField] private Sprite newBackgroundSprite;
    [SerializeField] private Volume volume;

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
            trainBackground.sprite = newBackgroundSprite;
            VolumeProfile profile = volume.profile;
            profile.TryGet(out ColorAdjustments color);
            color.colorFilter.value = new Color(0.5f, 0.25f, 0.25f);
            //color.colorFilter.overrideState
            
            //volume.profile = profile;
            
        }
    }
}
