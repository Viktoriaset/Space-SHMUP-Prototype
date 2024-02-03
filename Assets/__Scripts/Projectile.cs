using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return (_type);
        }

        set
        {
            SetType(value);
        }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }

        Move();
    }

    protected virtual void Move() { }

    /// <summary>
    /// �������� ������� ���� _type � ������������� ���� ����� �������,
    /// ��� ����������� � WeaponDifinition.
    /// </summary>
    /// <param name="eType">��� WeaponType ������������� ������ </param>
    public void  SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        ChangeColor(def.projectileColor);
        
    }

    protected virtual void ChangeColor(Color c)
    {
        rend.material.color = c;
    }
}
