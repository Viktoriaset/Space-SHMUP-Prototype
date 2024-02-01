using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in inspector")]
    [SerializeField] private float speed = 30;
    [SerializeField] private float rollMult = -45;
    [SerializeField] private float pitchMult = 30;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;

    public float gameRestartDelay = 2f;
    

    [Header("Set dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        } else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }
    }

    private void TempFire()
    {
        GameObject projGO = Instantiate(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidBD = projGO.GetComponent<Rigidbody>();
        rigidBD.velocity = Vector3.up * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        }

        set
        {
            _shieldLevel = Mathf.Min(value, 4);

            if (value < 0)
            {
                Destroy(gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}