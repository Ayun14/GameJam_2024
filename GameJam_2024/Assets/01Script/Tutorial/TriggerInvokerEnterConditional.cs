using UnityEngine;

public class TriggerInvokerEnterConditional : InvokerBase
{
    private int cnt = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cnt > 1) InvokeTrigger();
        cnt++;
    }
}
