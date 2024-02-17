using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Prototype_1
{
    public class Player : MonoBehaviour
    {
        public float CenterpointX = 0f; // the centerpoint x-position of the ship in units
        public float Displacement = 50f; // the maximum displacement to either side of the centerpoint
        public float PeriodInMilliseconds = 1000f; // the time the ship takes to complete 1 full cycle from minX to maxX and back to minX
        public float PercentageOffset = .0f; // the percentage by which we want to adjust the starting position of our ship between minX and maxX
        public float YUnitsPerPeriod = 25f; // the change in y position of the ship per period
        public float YUnitsPerBoost = 25f; // the change in y position of the ship per period
        private float timeInPeriod; // this is the amount of time that has passed in current period

        bool isDead = true;
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                if (value)
                {
                    Main.GameOverAction.Invoke();
                }

                isDead = value;
                DoUpdateShip = false;
            }
        }

        public bool DoUpdateShip;

        public InputActionAsset actions;
        private InputAction boostAction;
        public float BoostTimeoutMilliseconds;
        [SerializeField] bool isBoosting;

        void Awake()
        {
            boostAction = actions.FindActionMap("Gameplay").FindAction("Boost");
            boostAction.performed += OnBoostAction;
        }

        // Start is called before the first frame update
        void Start()
        {
            timeInPeriod = 0f;
        }

        public void StartRun()
        {
            isDead = false;
            DoUpdateShip = true;
            OnBoost();
        }

        // Update is called once per frame
        void Update()
        {
            if (DoUpdateShip)
            {
                // if we're boosting, pause our left/right movement 
                timeInPeriod += isBoosting ? 0f : Time.deltaTime * 1000;

                if (timeInPeriod > PeriodInMilliseconds)
                {
                    timeInPeriod = (timeInPeriod - PeriodInMilliseconds);
                }

                UpdatePosition();

                if (timeInPeriod >= PeriodInMilliseconds)
                {
                    timeInPeriod = 0f;
                }
            }
        }

        public void UpdatePosition()
        {
            float percentComplete = timeInPeriod / PeriodInMilliseconds;
            float xPos = CalculateXPosition(percentComplete);

            float yPos = transform.position.y + CalculateYPosition(Time.deltaTime * 1000 / PeriodInMilliseconds);
            transform.SetPositionAndRotation(new Vector3(xPos, yPos, transform.position.z), Quaternion.identity);
        }

        public float CalculateXPosition(float t)
        {
            // t == the percetange of time into the current period
            float percentDisplaced = Mathf.Sin(Mathf.PI * 2 * t);
            float deltaX = percentDisplaced * Displacement;
            return CenterpointX + deltaX;
        }

        public float CalculateYPosition(float t)
        {
            return isBoosting ? (YUnitsPerBoost + YUnitsPerPeriod) * t : YUnitsPerPeriod * t;
        }

        public void OnBoostAction(InputAction.CallbackContext context)
        {
            OnBoost();
        }

        public void OnBoost()
        {
            if (isDead || isBoosting) return;
            isBoosting = true;

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                float elapsedTime = 0f;
                while (elapsedTime < BoostTimeoutMilliseconds)
                {
                    elapsedTime += Time.deltaTime * 1000;
                    yield return null;
                }

                isBoosting = false;
            }
        }

        void OnEnable()
        {
            actions.FindActionMap("gameplay").Enable();
        }
        void OnDisable()
        {
            actions.FindActionMap("gameplay").Disable();
        }

        public void ResetShip()
        {
            transform.position = new Vector3(CenterpointX, 0f, transform.position.z);
            GetComponentInChildren<Shield>().ResetShield();
            timeInPeriod = 0f;
        }
    }
}