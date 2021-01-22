using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    public float currentHealth;

    private GameManager gm;
    private void Start()
    {
        currentHealth = maxHealth;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        gm.Respawn();
        Destroy(gameObject);
    }
}
