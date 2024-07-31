using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTransform;
    private AudioSource audioSource;
    [SerializeField] private float fireRate;
    private float timer = 0.0f;
    private bool onCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
        {
            timer += Time.deltaTime;
        }

        if (timer >= fireRate)
        {
            onCooldown = false;
            timer = 0.0f;
        }
    }

    public void Shoot()
    {
        if (onCooldown)
        {
            return;
        }

        Instantiate(bulletPrefab, muzzleTransform.position, transform.rotation);
        audioSource.Play();
        timer = 0.0f;
        onCooldown = true;
    }

    public Transform GetMuzzle()
    {
        return muzzleTransform;
    }
}
