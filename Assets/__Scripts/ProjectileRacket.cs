using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRacket : Projectile
{
    [Header("Set in inspector: ProjectileRacket")]
    public float speed = 10f;

    [Header("Set dynamicaly: ProjectileRacket")]
    private Transform target;
    private GameObject[] enemies;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ChooseTarget();
    }

    private void ChooseTarget()
    {
        if (enemies.Length == 0)
        {
            Destroy(gameObject);
            return;
        }
            

        target = enemies[0].transform;
        float distance = Vector3.Distance(target.position, transform.position);
        for (int i = 1; i < enemies.Length; i++)
        {
            float tempDistance = Vector3.Distance(enemies[i].transform.position, transform.position); ;
            if (distance > tempDistance)
            {
                target = enemies[i].transform;
            }
        }
    }

    protected override void Move()
    {
        base.Move();

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            Vector3 direction = (target.position - transform.position);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            return;
        }

        if (enemies.Length == 0)
        {
            rigid.velocity = Vector3.up * speed;
            return;
        }

        ChooseTarget();
    }
}
