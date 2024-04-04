using System;
using System.Collections.Generic;
using TNNL.Events;
using TNNL.Level;
using TNNL.RuntimeData;
using Unity.Mathematics;
using UnityEngine;

namespace TNNL.Player
{
    public class PlayerMVC
    {
        static List<PlayerMVC> players;
        public readonly PlayerModel Model;
        public readonly PlayerView View;
        public readonly PlayerController Controller;
        public readonly AttemptData AttemptData;
        readonly ShieldMVC shieldMVC;

        private PlayerMVC(AttemptData attempt, GameObject playerContainer)
        {
            AttemptData = attempt;

            // FUTURE
            // create the appropriate ship prefab and models based on the AttemptData's ship/pilot/upgrades
            ///

            GameObject playerPrefab = Resources.Load<GameObject>("Player/PlayerPrefab");
            GameObject playerGameObject = GameObject.Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity, playerContainer.transform);
            View = playerGameObject.GetComponentInChildren<PlayerView>();

            Controller = new PlayerController();
            Model = new PlayerModel();

            View.SetMVC(this);
            Controller.SetMVC(this);
            Model.SetMVC(this);

            //
            shieldMVC = new ShieldMVC();
            shieldMVC.Controller = new ShieldController();
            shieldMVC.Model = new ShieldModel(ShieldModel.DefaultShieldStartingHealth, ShieldModel.DefaultShieldMaxHealth);
            shieldMVC.View = View.GetComponentInChildren<ShieldView>();
            shieldMVC.View.SetMVC(shieldMVC);
            shieldMVC.Controller.SetMVC(shieldMVC);
            shieldMVC.Model.SetMVC(shieldMVC);
        }

        public static void CreatePlayers(PlayerRuntimeData playerRuntimeData, GameObject playerContainer)
        {
            // We have a game state that consists of:
            // - the current player status
            //      - info on each ship (furthest checkpoint reached, still alive, ship type, ship upgrades, pilot)
            //      - what are the scores

            players?.ForEach(mvc =>
            {
                mvc.Destroy();
            });

            players = new List<PlayerMVC>();
            PlayerMVC mvc;

            for (int i = 0; i < playerRuntimeData.Attempts.Count; i++)
            {
                mvc = new PlayerMVC(playerRuntimeData.Attempts[i], playerContainer);
                mvc.Model.AdjustY(-PlayerModel.Direction * mvc.View.GetComponentInChildren<ShieldView>().MaxScale * 1.5f * i);
                mvc.View.Update();
                players.Add(mvc);
            }
        }

        public static void SetCurrentPlayer(PlayerMVC playerMVC)
        {
            ShieldHealthUpdateEvent.Dispatch(playerMVC.shieldMVC.Model.PercentHealth);
            ShieldInvincibilityUpdateEvent.Dispatch(playerMVC.shieldMVC.Model.SecondsInvincibleRemaining);
        }

        public static void ClearDockingSpaceForShip(PlayerMVC incomingShip)
        {
            for (int i = players.IndexOf(incomingShip) - 1; i >= 0; i--)
            {
                if (players[i].AttemptData.IsAlive)
                {
                    players[i].Model.AdjustY(PlayerModel.Direction * players[i].shieldMVC.View.MaxScale * 1.5f);
                }
            }
        }

        public static PlayerMVC GetNextAttempt()
        {
            PlayerMVC nextShip = null;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].AttemptData.IsAlive)
                {
                    if (nextShip == null)
                    {
                        nextShip = players[i];
                    }
                    else if (LevelParser.Instance.CompareLevelOrder(players[i].AttemptData.CheckpointReached, nextShip.AttemptData.CheckpointReached) < 0)
                    {
                        nextShip = players[i];
                    }
                }
            }

            return nextShip;
        }

        public static void PrepForNextLevel()
        {
            int i = 0;
            players?.ForEach(mvc =>
            {
                if (mvc.AttemptData.IsAlive)
                {
                    mvc.Model.SetDefaults();
                    mvc.Model.AdjustY(-PlayerModel.Direction * mvc.shieldMVC.View.MaxScale * 1.5f * i);

                    mvc.shieldMVC.Controller.ResetShield();
                    i++;
                }
            });
        }

        private void Destroy()
        {
            Controller.Destroy();
            // View.gameObject.SetActive(true);
            View.Destroy();
        }
    }
}
