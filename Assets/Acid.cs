using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    private Dictionary<Collider2D, float> occupants;

    [SerializeField] private int damage = 1;
    [SerializeField] private float damageSeconds = 1.0f;

    void Awake()
    {
        occupants = new Dictionary<Collider2D, float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Brigand")) {
            Creature creature = collision.gameObject.GetComponent<Creature>();
            if (creature != null) {
                creature.Hurt(damage);
                occupants.Add(collision, 0.0f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (occupants.ContainsKey(collision)) {
            occupants[collision] += Time.deltaTime;
            if (occupants[collision] >= damageSeconds) {
                Creature creature = collision.gameObject.GetComponent<Creature>();
                if (creature.IsAlive()) {
                    creature.Hurt(damage);
                }
                
                occupants[collision] = 0.0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        occupants.Remove(collision);
    }
}
