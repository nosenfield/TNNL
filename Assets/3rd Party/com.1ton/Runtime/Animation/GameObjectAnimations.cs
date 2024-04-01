using System.Collections;
using UnityEngine;

namespace nosenfield.Animation
{
    public static class GameObjectAnimations
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="startingPos">the starting world position of the object</param>
        /// <param name="targetPos">the target world position of the object</param>
        /// <param name="duration">the duration in seconds</param>
        /// <param name="easingFunc"></param>
        public static void AnimateGameObjectToPosition(GameObject gameobject, Vector3 startingPos, Vector3 targetPos, float duration, EasingFunction.Function easingFunc)
        {
            gameobject.GetComponent<MonoBehaviour>().StartCoroutine(Routine());

            IEnumerator Routine()
            {
                float t = 0f;
                while (t < duration)
                {
                    SetGameObjectPosition(gameobject, startingPos, targetPos, t / duration, easingFunc);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                SetGameObjectPosition(gameobject, startingPos, targetPos, 1f, easingFunc);
            }
        }

        public static void SetGameObjectPosition(GameObject gameobject, Vector3 startingPos, Vector3 targetPos, float percentComplete, EasingFunction.Function easingFunc)
        {
            gameobject.transform.position = new Vector3(easingFunc(startingPos.x, targetPos.x, percentComplete), easingFunc(startingPos.y, targetPos.y, percentComplete), gameobject.transform.position.z);
        }
    }
}