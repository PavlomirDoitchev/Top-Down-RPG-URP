using UnityEngine;
namespace Assets.Scripts.Combat_Logic
{
    public interface IProjectile
    {
        Vector3 AimLocation(Transform target);
    }
}
