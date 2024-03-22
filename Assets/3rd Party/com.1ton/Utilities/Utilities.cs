using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using nosenfield.Animation;
using nosenfield.Logging;
using UnityEngine;

namespace nosenfield.Utilities
{
    public static class Utilities
    {
        private static Regex emailRegex = new Regex(@"[\w-\.]+@([\w-]+\.)+[\w-]");
        public static int CompareChildIndex<T>(T a, T b) where T : MonoBehaviour
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        }
        public static void SetPivot(RectTransform target, Vector2 pivot)
        {
            if (!target) return;
            Vector2 offset = pivot - target.pivot;
            offset.Scale(target.rect.size);
            Vector2 wordlPos = target.position + target.TransformVector(offset);
            target.pivot = pivot;
            target.position = wordlPos;
        }
        public static Rect GetBoundingBox(RectTransform[] transforms)
        {
            if (transforms.Length == 0) return new Rect(Vector2.zero, Vector2.zero);

            float xMin = Mathf.Infinity;
            float xMax = Mathf.NegativeInfinity;
            float yMin = Mathf.Infinity;
            float yMax = Mathf.NegativeInfinity;

            for (int i = 0; i < transforms.Length; i++)
            {
                RectTransform rt = transforms[i];
                xMin = Mathf.Min(rt.position.x + rt.rect.xMin * rt.lossyScale.x, xMin);
                xMax = Mathf.Max(rt.position.x + rt.rect.xMax * rt.lossyScale.x, xMax);
                yMin = Mathf.Min(rt.position.y + rt.rect.yMin * rt.lossyScale.y, yMin);
                yMax = Mathf.Max(rt.position.y + rt.rect.yMax * rt.lossyScale.y, yMax);
            }

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string GetNestedObjectName(Transform t)
        {
            string name = t.name;
            while (t.parent != null)
            {
                name += $".{t.parent.name}";
                t = t.parent;
            }

            return name;
        }

        public static void SetGameObjectToPosition(GameObject gameobject, Vector3 startingPos, Vector3 targetPos, float percentComplete, EasingFunction.Function easingFunc)
        {
            gameobject.transform.position = new Vector3(easingFunc(startingPos.x, targetPos.x, percentComplete), easingFunc(startingPos.y, targetPos.y, percentComplete), gameobject.transform.position.z);
        }

        public static void SetGameObjectToScale(GameObject gameobject, Vector3 startingScale, Vector3 targetScale, float percentComplete, EasingFunction.Function easingFunc)
        {
            gameobject.transform.localScale = new Vector3(easingFunc(startingScale.x, targetScale.x, percentComplete), easingFunc(startingScale.y, targetScale.y, percentComplete), gameobject.transform.localScale.z);
        }

        public static bool IsValidEmail(string email)
        {
            DefaultLogger.Instance.LogTrace();

            bool valid = true;
            try
            {
                new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            DefaultLogger.Instance.Log(LogLevel.DEBUG, $"Email validation test 1: {email}, {valid.ToString()}");

            DefaultLogger.Instance.Log(LogLevel.DEBUG, $"Email validation test 2: {email}, {emailRegex.IsMatch(email)}");

            return (valid ? emailRegex.IsMatch(email) : valid);
        }
    }
}