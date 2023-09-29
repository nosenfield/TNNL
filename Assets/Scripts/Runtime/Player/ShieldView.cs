using UnityEngine;
using UnityEngine.Events;

public class ShieldView : MonoBehaviour
{
    // events
    public UnityEvent<IShieldCollidable> ShieldCollision;

    // mvc 
    [SerializeField] private bool GenerateController;
    public bool DoUpdate;
    ShieldController controller;
    ShieldModel model;

    // member vars
    public float MinScale; // the scale of the shield game object when at 0% health. note, this should match the scale of the ship
    public float MaxScale; // the scale of the shield game object when at 100% health
    public SphereCollider sphereCollider; //
    private float colliderStartingRadius = .5f; //
    private float colliderMinRadius;

    void Awake()
    {
        colliderMinRadius = MinScale / MaxScale * colliderStartingRadius;

        if (GenerateController)
        {
            controller = new ShieldController(this);
        }
    }

    public void SetModel(ShieldModel model)
    {
        this.model = model;
    }

    public void Update()
    {
        if (model != null && DoUpdate)
        {
            UpdateAppearance(model.PercentHealth);
        }
    }

    public void UpdateAppearance(float percentHealth)
    {
        // set the visual scale of the orb and the scale of the collider
        float newScale = MinScale + (MaxScale - MinScale) * percentHealth;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        IShieldCollidable collidable = other.GetComponentInParent<IShieldCollidable>();

        switch (collidable)
        {
            case null:
                break;
            default:
                ShieldCollision.Invoke(collidable);
                break;
        }

        // parse/report the collision by some kind of type identifier (what did we hit)
    }
}