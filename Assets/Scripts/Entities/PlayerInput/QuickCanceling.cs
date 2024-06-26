using System.Collections.Generic;

public class QuickCanceling
{
    private PlayerInputManager _inputManager;
    private List<ICancelable> _stack;

    public QuickCanceling()
    {
        _stack = new();
    }

    public void Init( )
    {
        _inputManager = PlayerInputManager.Instance;
        _inputManager.RegisterEsc(CancelLastAction);
    }

    public void Push(ICancelable cancelable)
    {
        if (_stack.Contains(cancelable))
        {
            _stack.Remove(cancelable);
        }
        _stack.Add(cancelable);
    }

    public void Remove(ICancelable cancelable)
    {
        _stack.Remove(cancelable);
    }

    private void CancelLastAction()
    {
        if (_stack.Count == 0)
            return;

        for (int i = _stack.Count -1; i >= 0; i--)
        {
            var item = _stack[i];
            _stack.RemoveAt(i);
            if (item.IsCanceled() == false)
            {
                item.QuickCancel();
                break;
            }
        }
    }
}
