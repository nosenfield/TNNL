// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;

public class GameObjectNote : MonoBehaviour
{
    [TextArea(20, 40)][SerializeField] private string note;
}