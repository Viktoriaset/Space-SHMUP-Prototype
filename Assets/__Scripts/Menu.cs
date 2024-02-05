using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject recordPrefab;
    public GameObject recordsPanel;

    [Header("Set dynamically")]
    public List<int> records;

    public void Awake()
    {
        records = Records.GetRecords();
        records.Sort();
        records.Reverse();
        ShowRecords();
    }

    public void ShowRecords()
    {
        for (int i = 0; i < records.Count; i++)
        {
            GameObject recordGO = Instantiate(recordPrefab);
            recordGO.transform.SetParent(recordsPanel.transform, false);

            TextMeshProUGUI text = recordGO.GetComponent<TextMeshProUGUI>();
            text.text = records[i].ToString();
            float red = (255f - (i) * 255f / records.Count);
            Color color = Color.red;
            color.r = red / 255f;
            print("color.r: " + color.r);
            text.color = color;
            print("text.color" + text.color.ToString());
        }
    }


    public void Play()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
