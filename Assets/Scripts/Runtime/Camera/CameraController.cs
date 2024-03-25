using System;
using nosenfield.Logging;
using TNNL.Level;
using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.Camera
{
    public class CameraController : MonoBehaviour
    {
        private float cameraSize = 0f;
        [SerializeField] private float percentVerticalBuffer = 0f; // the percentage of the screen to shift the viewport by
        [SerializeField] private PlayerView player;

        void Awake()
        {
            LevelParser.LevelCreated += UpdateSize;
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition(player.gameObject.transform.position.y + cameraSize * 2 * percentVerticalBuffer * PlayerModel.Direction);
        }

        void UpdatePosition(float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        void UpdateSize(float levelWidth)
        {
            // size the viewport height to at least 2x the level width
            float maxAspectRatio = 8f / 16f;
            cameraSize = Mathf.Max(Screen.height * levelWidth * .5f / Screen.width, levelWidth / maxAspectRatio * .5f);
            this.GetComponent<UnityEngine.Camera>().orthographicSize = cameraSize;


            // the orthographicSize is equal to half the viewport height
            // if the size == half the level width, the "shortest" shape we show is a square
            //


            // screen.height * aspectRatio = the width of the viewport to achieve the desired aspect ratio
            // if the width of the viewport to achieve the desired ratio is greater than the width of the screen,
            // increase the orthographic size to shrink the width of the viewport to the screen

            // if the width of the viewport is less than the width of the screen, that's ok, set the orthographic size to the screen.height * aspectRatio * .5


            // this.GetComponent<UnityEngine.Camera>().orthographicSize = Mathf.Max(Screen.height * maxAspectRatio * .5f, levelWidth / maxAspectRatio * .5f);
        }

        public void SetActiveShip(PlayerView view)
        {
            player = view;
        }
    }
}