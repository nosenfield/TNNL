using System;
using System.Collections.Generic;
using TNNL.Level;
using TNNL.RuntimeData;
using UnityEngine;

namespace TNNL.Player
{
    public class PlayerMVC
    {
        public static Action<PlayerMVC> SetCurrentPlayer;
        static GameObject playerContainer;
        static List<PlayerMVC> players;
        public readonly PlayerModel Model;
        public readonly PlayerView View;
        public readonly PlayerController Controller;
        public readonly ShipData ShipData;
        [SerializeField] private GameObject playerPrefab;

        void Awake()
        {
            playerPrefab.SetActive(false);
        }

        private PlayerMVC(ShipData shipData)
        {
            ShipData = shipData;
            GameObject playerGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Player/PlayerPrefab"), playerContainer.transform);
            View = playerGameObject.GetComponentInChildren<PlayerView>();

            Controller = new PlayerController();
            Model = new PlayerModel();

            View.SetMVC(this);
            Controller.SetMVC(this);
            Model.SetMVC(this);
        }

        public static void CreatePlayers(PlayerRuntimeData playerRuntimeData, GameObject playerContainer)
        {
            // We have a game state that consists of:
            // - the current player status
            //      - info on each ship (furthest checkpoint reached, still alive, ship type, ship upgrades, pilot)
            //      - what are the scores

            players?.ForEach(obj => obj.Controller.Deactivate());

            PlayerMVC.playerContainer = playerContainer;
            players = new List<PlayerMVC>();

            for (int i = 0; i < playerRuntimeData.ShipData.Count; i++)
            {
                players.Add(new PlayerMVC(playerRuntimeData.ShipData[i]));
            }
        }

        public static PlayerMVC GetCurrentShip()
        {
            if (players.Count == 0) return null;

            PlayerMVC currentShip = players[0];
            for (int i = 1; i < players.Count; i++)
            {
                if (players[i].ShipData.IsAlive && LevelParser.Instance.CompareLevelOrder(players[i].ShipData.CheckpointReached, currentShip.ShipData.CheckpointReached) < 0)
                {
                    currentShip = players[i];
                }
            }

            SetCurrentPlayer.Invoke(currentShip);

            return currentShip.ShipData.IsAlive ? currentShip : null;
        }
    }
}
