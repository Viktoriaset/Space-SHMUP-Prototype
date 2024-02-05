using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Records
{
    public static int maxSize = 5;
    static public List<int> GetRecords()
    {
        List<int> records = new List<int>();
        if (!PlayerPrefs.HasKey("recordsCount"))
        {
            PlayerPrefs.SetInt("recordsCount", 0);
        }

        if (PlayerPrefs.GetInt("recordsCount") == 0)
        {
            return records;
        }

        for (int i = 0; i < PlayerPrefs.GetInt("recordsCount"); i++)
        {
            records.Add(PlayerPrefs.GetInt("record_" + i));
        }

        return records;
    }

    public static void AddRecod(int record)
    {
        if (!PlayerPrefs.HasKey("recordsCount"))
        {
            PlayerPrefs.SetInt("recordsCount", 0);
        }

        int size = PlayerPrefs.GetInt("recordsCount");
        if (size == maxSize)
        {
            List<int> records = GetRecords();
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i] < record)
                {
                    (records[i], record) = (record, records[i]);
                    PlayerPrefs.SetInt("record_" + i, records[i]);
                }
            }
            return;
        }

        PlayerPrefs.SetInt("record_" + size, record);
        PlayerPrefs.SetInt("recordsCount", size + 1);
    }
}
