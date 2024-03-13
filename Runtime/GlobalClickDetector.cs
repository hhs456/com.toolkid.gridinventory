using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
namespace Toolkid.GridInventory {
    public class GlobalClickDetector<T> where T : MonoBehaviour {
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
                target.StartCoroutine("Detect");
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
                target.StartCoroutine("PermanentDetect");
            }
        }

#if UNITASK
        public async UniTask DetectAsync() {
            while (enabled) {
                await UniTask.Yield();                
                if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                    if (match.Invoke(target)) {
                        action?.Invoke();
                        break;
                    }
                }
            }
        }
        public async UniTask PermanentDetectAsync() {
            while (enabled) {
                await UniTask.Yield();
                if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                    if (match.Invoke(target)) {
                        action?.Invoke();                        
                    }
                }
            }
        }
#endif
        public IEnumerator Detect() {
            while (enabled) {
                yield return new WaitForEndOfFrame();
                if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                    if (match.Invoke(target)) {
                        action?.Invoke();
                        break;
                    }
                }
            }
        }

        public IEnumerator PermanentDetect() {
            while (enabled) {
                yield return new WaitForEndOfFrame();
                if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                    if (match.Invoke(target)) {
                        action?.Invoke();
                        break;
                    }
                }
            }
        }

        public void Dispose() {
            enabled = false;
        }
    }
}