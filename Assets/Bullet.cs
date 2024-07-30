using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float initialForce = 300.0f;
    [SerializeField] private float lifespan = 10.0f;
    [SerializeField] private int damage = 5;
    [SerializeField] private bool playersBullet = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(initialForce, 0));
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool hitThePlayer = collision.gameObject.CompareTag("Player");

        if (playersBullet && hitThePlayer) {
            return;
        }

        if (hitThePlayer || collision.gameObject.CompareTag("Brigand")) {
            Creature creature = collision.gameObject.GetComponent<Creature>();
            if (creature != null) {
                creature.Hurt(damage);
            }
        }

        Destroy(gameObject);
    }
}
