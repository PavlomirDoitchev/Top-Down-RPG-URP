using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;


public class SawbladeTrap : MonoBehaviour
{
    private Animator animator;
    private BoxCollider capsuleCollider;
    //[SerializeField] private float timer = 1f;
    //bool hasTakenDamage = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponentInChildren<BoxCollider>();
    }
    /// <summary>
    /// Used in animation events
    /// </summary>
    public void EnableCollider()
    {
        capsuleCollider.enabled = !capsuleCollider.enabled;
    }
}
