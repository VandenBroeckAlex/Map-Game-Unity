//using UnityEngine;
//using System.IO;
//using UnityEditor;

//public static class UnitStatsManager
//{
//    private static UnitBaseStats _unitStats;

//    public static UnitBaseStats Stats => _unitStats;

//    public static void LoadStats()
//    {
//        string path = Path.Combine(Application.persistentDataPath, "data/unit_stats.json");

//        if (File.Exists(path))
//        {
//            string json = File.ReadAllText(path);
//            _unitStats = JsonUtility.FromJson<UnitBaseStats>(json);
//        } 
//        else
//        {
//            Debug.LogError("Stats file not found at: " + path);
//            _unitStats = new UnitBaseStats(); // fallback
//        }
//    }
//}

///*
// 3. Call LoadStats() once early (e.g., in a GameManager or entry scene):

//    void Start()
//    {
//        UnitStatsManager.LoadStats();
//    }

//4. Access anywhere like:

//float damage = UnitStatsManager.Stats.baseDamage;


//    ?? Optional Enhancements
//    Cache multiple unit types (e.g., a dictionary of unit types ? stats).

//    Use a singleton MonoBehaviour if you need to use Unity-specific features like Coroutines or ScriptableObjects.

//    Add editor/debug fallback values.
//*/