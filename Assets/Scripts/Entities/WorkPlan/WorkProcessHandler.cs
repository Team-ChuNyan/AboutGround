using UnityEngine;

public class WorkProcessHandler
{
    public static void Carry(IPickupable pickupable, int amount)
    {
        pickupable.IsGenerateCarry = true;
        var item = pickupable.Item;
        var position = pickupable.Position;
        var workPos = Util.Vector3XZToVector2Int(position);

        Work workPickup = new(amount, workPos);
        workPickup.RegisterStarted(PickUpItem);

        Work workPutDown = new(amount, Vector2Int.one * 80);
        workPutDown.RegisterStarted(Putdown);

        WorkProcess work = default;
        work = WorkProcessGenerator.Instance.SetNewWork(WorkType.Carry)
            .AddWork(workPickup)
            .AddWork(workPutDown)
            .RegisterFinished(FinishCarry)
            .Generate();

        void PickUpItem(IWorkable worker)
        {
            pickupable.PickUp(worker, amount);
            worker.AddWorkload(amount);
        }

        void Putdown(IWorkable worker)
        {
            worker.PutDownItem(pickupable.Item, amount);
            worker.AddWorkload(amount);
        }

        void FinishCarry(IWorkable worker)
        {
            pickupable.FinishWork(WorkType.Carry);
        }
    }
}
