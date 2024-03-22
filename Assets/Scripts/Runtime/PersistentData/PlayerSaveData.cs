using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using nosenfield.PersistentData;
using Sirenix.Utilities;
using UnityEngine;

namespace TNNL.Data
{
    [Serializable]
    public class PlayerSaveData
    {
        [NonSerialized] private static readonly string DirectoryPath = "UserData";
        [NonSerialized] private static readonly string FileName = "PlayerSaveData";
        [JsonProperty] private readonly Dictionary<string, int> HighScores = new();


        private static PlayerSaveData instance;
        public static PlayerSaveData Instance
        {
            get
            {
                instance ??= PersistentDataService.LoadDataOfType<PlayerSaveData>(DirectoryPath, FileName);
                if (instance != null)
                {
                    Debug.Log("PlayerSaveData loaded from disk");
                    Debug.Log(instance.HighScores.Keys.Count);

                    foreach (KeyValuePair<string, int> kvp in instance.HighScores)
                    {
                        Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
                    }
                }

                instance ??= new PlayerSaveData();
                return instance;
            }
        }

        private PlayerSaveData() { }

        public static void Save()
        {
            PersistentDataService.SaveSerializableData<PlayerSaveData>(Instance, DirectoryPath, FileName);
        }

        public static int GetHighScore(string levelId)
        {
            int highScore = 0;
            if (Instance.HighScores.ContainsKey(levelId))
            {
                highScore = Instance.HighScores[levelId];
            }

            Debug.Log($"High score for {levelId}:{highScore}");

            return highScore;
        }

        public static void SetHighScore(string levelId, int score)
        {
            if (!Instance.HighScores.ContainsKey(levelId))
            {
                Instance.HighScores.Add(levelId, 0);
            }

            Instance.HighScores[levelId] = score;
            Save();
        }
    }
}