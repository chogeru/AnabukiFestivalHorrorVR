using System;

public static class EventManager
{
    public static Action OnKillerPranEvent;

    //キラーのドッキリイベント
    public static void KillerPrankEvent()
    {
        if (OnKillerPranEvent != null)
        {
            OnKillerPranEvent.Invoke();
        }
    }
}