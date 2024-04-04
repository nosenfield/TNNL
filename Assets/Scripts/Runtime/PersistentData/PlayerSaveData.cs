using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using nosenfield.PersistentData;
using Sirenix.Utilities;
using TNNL.RuntimeData;
using UnityEngine;

namespace TNNL.Data
{
    [Serializable]
    public class PlayerSaveData
    {
        [NonSerialized] private static readonly string DirectoryPath = "UserData";
        [NonSerialized] private static readonly string FileName = "PlayerSaveData";
        [JsonProperty] private readonly Dictionary<string, int> highScores = new();
        [JsonProperty] private List<AttemptData> attempts = new();


        private static PlayerSaveData instance;
        public static PlayerSaveData Instance
        {
            get
            {
#if UNITY_EDITOR || !UNITY_WEBGL
                instance ??= PersistentDataService.LoadDataOfType<PlayerSaveData>(DirectoryPath, FileName);
#endif
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
            if (Instance.highScores.ContainsKey(levelId))
            {
                highScore = Instance.highScores[levelId];
            }

            Debug.Log($"High score for {levelId}:{highScore}");

            return highScore;
        }

        public static void SetHighScore(string levelId, int score)
        {
            if (!Instance.highScores.ContainsKey(levelId))
            {
                Instance.highScores.Add(levelId, 0);
            }

            Instance.highScores[levelId] = score;
            Save();
        }

        public static List<AttemptData> GetAttempts()
        {
            return Instance.attempts;
        }

        public static void SetAttempts(List<AttemptData> attempts)
        {
            Instance.attempts = attempts;
        }

        public static void ResetData()
        {
            instance = new PlayerSaveData();
        }
    }
}