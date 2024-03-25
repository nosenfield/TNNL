using System.Collections;
using nosenfield.Logging;
using TNNL.Player;
using UnityEngine;


namespace TNNL.Collidables
{
    /// <summary>
    /// The Mine Behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
    /// A SuperMine would be a secondary prefab that had a Mine behaviour and higher damage.
    /// In order to record this we either need to store the entire model (type + damage)
    /// Or we need to create a new type for the Supermine
    /// </summary>
    public class ElectricGate : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.ElectricGate;
            }
        }

        public static int MinRowsBetweenGates = 10;
        public float DamagePerSecond = 10f;

        public override int CollisionPoints
        {
            get
            {
                return 0;
            }
        }

        private bool doDamage;
        private ShieldModel shieldModel;
        [SerializeField] private GameObject leftNode;
        [SerializeField] private GameObject rightNode;
        [SerializeField] private GameObject bar;
        [SerializeField] private float barBuffer = 1f;

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            DefaultLogger.Instance.LogTrace();
            ShieldView shield = other.GetComponentInParent<ShieldView>();
            if (shield != null)
            {
                DefaultLogger.Instance.Log(LogLevel.DEBUG, "Shield collided with electric gate");
                doDamage = true;
                // We need access to the ShieldModel instance to damage it.
                // The model is provided via collision listeners of the shield system.
                // The model could be set here if we had access to it via ShieldView.model
                // or some other interface (ie. ShieldMVC.model)
            }
        }

        public void OnTriggerExit(Collider other)
        {
            DefaultLogger.Instance.LogTrace();
            ShieldView shield = other.GetComponentInParent<ShieldView>();
            if (shield != null)
            {
                DefaultLogger.Instance.Log(LogLevel.DEBUG, "Shield exited electric gate");
                shieldModel = null;
                doDamage = false;
            }
        }

        void Update()
        {
            if (shieldModel != null && doDamage)
            {
                shieldModel.DamageShield(DamagePerSecond * Time.deltaTime);
            }
        }

        public void SetModelToDamage(ShieldModel model)
        {
            shieldModel = model;
        }

        public void SetGatePositionAndWidth(float xPos, float width)
        {
            container.transform.localPosition = new Vector3(xPos, container.transform.localPosition.y, container.transform.localPosition.z);
            container.transform.localPosition = new Vector3(xPos, container.transform.localPosition.y, container.transform.localPosition.z);
            bar.transform.localScale = new Vector3(width - leftNode.transform.localScale.x - rightNode.transform.localScale.x - barBuffer, bar.transform.lossyScale.y, bar.transform.lossyScale.z);
            leftNode.transform.localPosition = new Vector3(-width * .5f + leftNode.transform.localScale.x * .5f, leftNode.transform.localPosition.y, leftNode.transform.localPosition.z);
            rightNode.transform.localPosition = new Vector3(width * .5f - rightNode.transform.localScale.x * .5f, rightNode.transform.localPosition.y, rightNode.transform.localPosition.z);
        }
    }
}