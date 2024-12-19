using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInvokerEnter : InvokerBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeTrigger();
    }
}
