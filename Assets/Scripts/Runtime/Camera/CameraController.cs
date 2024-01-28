using TNNL.Player;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerGameObject;
    public GameObject levelContainer;

    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = PlayerShip.Instance.gameObject;
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