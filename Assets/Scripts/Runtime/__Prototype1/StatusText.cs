using UnityEngine;
using TMPro;

namespace TNNL.Prototype_1
{
    public class StatusText : MonoBehaviour
    {

        public int TotalMines = 0;
        int minesHit = 0;
        public int TotalShields = 0;
        int shieldsCollected = 0;
        int attempts = 0;
        int wins = 0;
        [SerializeField] TextMeshProUGUI statusText;

        void Start()
        {
            UpdateText();
        }

        void UpdateText()
        {
            string str = "";
            str += $"TotalMines:{TotalMines}\n";
            str += $"TotalShields:{TotalShields}\n";
            str += $"MinesHit:{minesHit}\n";
            str += $"ShieldsCollected:{shieldsCollected}\n";
            str += $"Record:{wins}/{attempts}\n";
            statusText.text = str;
        }

        public void MineHit()
        {
            minesHit++;
            UpdateText();
        }

        public void ShieldCollected()
        {
            shieldsCollected++;
            UpdateText();
        }

        public void IncreaseAttempts()
        {
            attempts++;
            UpdateText();
        }

        public void IncreaseWinCount()
        {
            wins++;
            UpdateText();
        }

        public void Reset()
        {
            attempts = 0;
            minesHit = 0;
            shieldsCollected = 0;
            UpdateText();
        }
    }
}