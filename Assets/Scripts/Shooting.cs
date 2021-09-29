using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Rigidbody bullet;                    // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    [SerializeField] Transform orientation;
    Vector3 shootDirection;
    Vector3 moveDirection;

    public int m_PlayerNumber = 1;
    public float shootForce = 150f;
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

    private string m_FireButton;                // The input axis that is used for launching shells.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
    }


    private void Update()
    {
        if (!drawingMenu.isDrawing)
        { 
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            else if (Input.GetButtonDown(m_FireButton))
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;
            }
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            }
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        m_Fired = true;
        Rigidbody bulletInstance = Instantiate(bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shootDirection = orientation.forward * shootForce + moveDirection;
        bulletInstance.AddForce(shootDirection, ForceMode.Impulse);
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}