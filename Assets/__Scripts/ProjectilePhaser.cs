using UnityEngine;

public class ProjectilePhaser : Projectile
{
    [Header("Set in inspector: ProjectilePhaser")]
    [SerializeField] private float waveFrequency = 2;
    [SerializeField] private float waveWidth = 4;
    [SerializeField] private float waveRotY = 45;

    public float frequencyOffset = 0;

    private float x0;
    private float birthTime;

    private void Start()
    {
        x0 = transform.position.x;
        birthTime = Time.time;
    }

    protected override void Move()
    {
        Vector3 tempPos = transform.position;
        float age = Time.time - birthTime + frequencyOffset;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        transform.position = tempPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        transform.rotation = Quaternion.Euler(rot);
    }

    protected override void ChangeColor(Color c)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = c;
        }
    }
}
