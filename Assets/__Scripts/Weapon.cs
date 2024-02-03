using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѕеречисление всех возможных типов оружи€ 
/// “ак же включает тип "shield", что бы дать возможность совершенствовать защиту
/// </summary>
public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser, // волновой фазеп
    missile, // самоновод€щиес€ ракеты
    laser,
    shield
}

/// <summary>
///  ласс WeaponDefinition позвол€ет настраивать свойства 
/// конкретного оружи€ в инспекторе. ƒл€ этого класс Main 
/// будет хранить массив элементов типа WeaponDefinition.
/// </summary>
[Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    [Tooltip("Ѕуква на кубике, изоброжающем бонус")]
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor;
    public float damageOnHit = 0;
    public float continuosDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;

}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set dynamically")]
    [SerializeField] private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return _type; }
        set { SetType(_type); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            gameObject.SetActive(false);
            return;
        } 
        else
        {
            gameObject.SetActive(true);  
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Time.time - lastShotTime < def.delayBetweenShots) return;

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch(type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.spread:
                p = MakeProjectile(); // fly straight
                p.rigid.velocity = vel;

                p = MakeProjectile(); // fly right
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;

                p = MakeProjectile(); // fly left 
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;

            case WeaponType.phaser:
                p = MakeProjectile();
                p.rigid.velocity = vel;

                p = MakeProjectile();
                p.rigid.velocity = vel;
                ProjectilePhaser pp = p as ProjectilePhaser;
                pp.frequencyOffset = 1;
                break;
        }
    }

    private Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
