using System.Collections;
using UnityEngine;

namespace TNNL.Animation
{
    public static class Utilities
    {
        private static void SetObjectToPosition(GameObject gameObject, Vector3 startingPos, Vector3 targetPos, float percentComplete, EasingFunction.Function easingFunc)
        {
            gameObject.transform.position = new Vector3(easingFunc(startingPos.x, targetPos.x, percentComplete), easingFunc(startingPos.y, targetPos.y, percentComplete), easingFunc(startingPos.z, targetPos.z, percentComplete));
        }

        public static Coroutine AnimateGameObjectToPosition(MonoBehaviour caller, GameObject gameobject, Vector3 startingPos, Vector3 targetPos, float duration, EasingFunction.Function easingFunc)
        {
            IEnumerator Routine()
            {
                float t = 0f;
                while (t < duration)
                {
                    SetObjectToPosition(gameobject, startingPos, targetPos, t / duration, easingFunc);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                gameobject.transform.position = targetPos;
            };

            return caller.StartCoroutine(Routine());
        }
    }
}