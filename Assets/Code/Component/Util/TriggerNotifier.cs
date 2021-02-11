using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    [SerializeField] private Collider2D target = null;
    [SerializeField] private bool oneTime = false;
    [SerializeField] private UnityEngine.Events.UnityEvent onContact = null;
    [SerializeField] private UnityEngine.Events.UnityEvent onLeave = null;

    private new bool enabled = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled) {
            return;
        }

        if (!target) {
            onContact.Invoke();
            if (oneTime) {
                enabled = false;
            }
        }

        if (target == collision) {
            onContact.Invoke();
            if (oneTime) {
                enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!enabled) {
            return;
        }

        if (!target) {
            onLeave.Invoke();
            if (oneTime) {
                enabled = false;
            }
        }

        if (target == collision) {
            onLeave.Invoke();
            if (oneTime) {
                enabled = false;
            }
        }
    }
}
