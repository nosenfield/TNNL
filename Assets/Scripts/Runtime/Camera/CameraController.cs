using nosenfield.Logging;
using TNNL.Level;
using TNNL.Player;
using UnityEngine;

namespace TNNL.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerView playerShip;

        void Awake()
        {
            LevelParser.LevelCreated += UpdateSize;
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition(playerShip.gameObject.transform.position.y);
        }

        void UpdatePosition(float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        void UpdateSize(float levelWidth)
        {
            // fit the viewport to the level width, unless it makes the height smaller than the width
            this.GetComponent<UnityEngine.Camera>().orthographicSize = Mathf.Max(Screen.height * levelWidth * .5f / Screen.width, levelWidth * .5f);
        }
    }
}