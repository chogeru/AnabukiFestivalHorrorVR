using System;

public static class EventManager
{
    public static Action OnKillerPranEvent;

    //�L���[�̃h�b�L���C�x���g
    public static void KillerPrankEvent()
    {
        if (OnKillerPranEvent != null)
        {
            OnKillerPranEvent.Invoke();
        }
    }
}