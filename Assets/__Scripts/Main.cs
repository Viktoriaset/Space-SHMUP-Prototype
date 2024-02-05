using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in inspector")]
    public TextMeshProUGUI score;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;
    public GameObject powerUpPrefab;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread, WeaponType.shield
    };

    private int points = 0;
    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    private void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;

        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);

        pos.y = bndCheck.camHeight + enemyPadding;

        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    private void Restart()
    {
        Records.AddRecod(points);
        SceneManager.LoadScene("Menu");
    }

    public void ShipDestroyed(Enemy e)
    {
        points += e.score;
        score.text = "Score: " + points;

        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            GameObject go = Instantiate(powerUpPrefab);
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetTipe(puType);

            pu.transform.position = e.transform.position;
        }
    }

    /// <summary>
    /// —татическа€ функци€ возвращающа€ WeaponDefinition из статического
    /// защищеного пол€ WEAP_DICT класса Main
    /// </summary>
    /// <param name="wt">Tnn оружи€ WeaponType, дл€ которого требуетс€ получить
    /// WeaponDefinition</param>
    /// <returns>Ёкземпл€р WeaponDefinition или, если нет такого определени€
    /// дл€ указанного WeaponType, возвращает новый экземпл€р WeaponDefinition
    /// с типом none</returns>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        return new WeaponDefinition();
    }
}
