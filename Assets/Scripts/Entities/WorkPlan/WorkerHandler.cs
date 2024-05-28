using System.Collections;
using UnityEngine;

public class WorkerHandler : MonoBehaviour
{
    private WorkProcess _workProcess;
    private int _step = 0;
    private int _CompleteStep = 0;

    private Coroutine _processCoroutine;

    private void Awake()
    {
        _workProcess = new();
    }

    public void Refresh()
    {
        _step = 0;
        _CompleteStep = 0;
        _workProcess.Clear();
    }

    public void ContractWork(WorkProcess workProcess)
    {
        _workProcess = workProcess;
        _CompleteStep = workProcess.Count;
    }

    public void StartProcess(IWorkable worker)
    {
        _processCoroutine = StartCoroutine(Process(worker));
    }

    private IEnumerator Process(IWorkable worker)
    {
        _step = 0;
        var works = _workProcess.Works;

        while (_step < _CompleteStep)
        {
            // WorkPos 이동
            worker.Move(works[_step].WorkPos);

            while (worker.IsArrive == false)
            {
                if (_workProcess.IsCanceling == true)
                    break;

                yield return null;
            }

            // 일 시작
            works[_step].OnStarted(worker);

            while (works[_step].Workload < works[_step].MaxWorkload)
            {
                if (_workProcess.IsCanceling == true)
                    break;

                works[_step].OnProcessed(worker);
                yield return null;
            }

            works[_step].OnCompleted(worker);
            _step++;
        }

        if (_step == _CompleteStep)
        {
            CompleteWork(worker);
            WorkProcessGenerator.Instance.Remove(_workProcess);
        }
    }

    private void CompleteWork(IWorkable worker)
    {
        worker.CompleteWork();
        _workProcess.OnFinished(worker);
    }

    public void StopProcess()
    {
        StopCoroutine(_processCoroutine);
    }

    public void AddWorkload(float value)
    {
        _workProcess.AddWorkLoad(_step, value);
    }

}
