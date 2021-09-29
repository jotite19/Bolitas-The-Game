using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControler : MonoBehaviour
{
    public LayerMask m_playerMask;                              
    public float m_MaxDamage = 100f;
    public float m_HittRadius = 10f;
    public float m_MaxLifeTime = 2f;                              

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_HittRadius, m_playerMask);

        
        for (int i = 0; i < colliders.Length; i++)
        {
            /*
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
                continue;
            float targetHealth = targetRigidbody.GetComponent<Mass>();
            if (!targetHealth)
                continue;
            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);
            */
        }
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        float damage = m_MaxDamage;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}