using UnityEngine;

namespace TNNL.Prototype_1
{

    public class CameraController : MonoBehaviour
    {
        public GameObject playerGameObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition(playerGameObject.transform.position.y);
        }

        public void UpdatePosition(float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}