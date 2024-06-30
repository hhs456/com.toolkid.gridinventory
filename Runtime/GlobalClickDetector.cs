#if UNITASK
using Cysharp.Threading.Tasks;
#endif
using System;
using System.Collections;
using UnityEngine;
namespace Toolkid.UIGrid {
    public class GlobalClickDetector<T> : MonoBehaviour where T : MonoBehaviour {
        T target;
        Predicate<T> match;
        Action action;

        bool enabled = false;

        public GlobalClickDetector(T target, Predicate<T> match, Action action) {
            this.target = target;
            this.match = match;
            this.action = action;
        }

        public void Forget() {
            enabled = true;
            bool enableUniTask = false;
#if UNITASK
            enableUniTask = true;
            DetectAsync().Forget();
#endif
            if (!enableUniTask) {
                StartCoroutine("Detect");
            }
        }

        public void Start() {
            enabled = true;
            bool enableUniTask = false;
#if UNITASK
            enableUniTask = true;
            PermanentDetectAsync().Forget();
#endif
            if (!enableUniTask) {
                StartCoroutine("PermanentDetect");
            }
        }

#if UNITASK
        public async UniTask DetectAsync() {
            while (enabled) {
                await UniTask.Yield();
                Invoke(false);
            }
        }
        public async UniTask PermanentDetectAsync() {
            while (enabled) {
                await UniTask.Yield();
                Invoke(true);
            }
        }
#endif
        public IEnumerator Detect() {
            while (enabled) {
                yield return new WaitForEndOfFrame();
                Invoke(false);
            }
        }

        public IEnumerator PermanentDetect() {
            while (enabled) {
                yield return new WaitForEndOfFrame();
                Invoke(true);
            }
        }        

        protected void Invoke(bool isPermanent) {
            if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                if (match.Invoke(target)) {
                    action?.Invoke();
                    enabled = isPermanent;
                }
            }
        }

        public void Dispose() {
            enabled = false;
        }
    }
}