using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.Player;
public class PlayerMelee : MonoBehaviour
{
    [Header("-----References-----")]
    [SerializeField] private DamageNumber damageText;
    [field: SerializeField] public WeaponDataSO EquippedWeaponDataSO { get; set; }
    public GameObject[] damageColliders;
    PlayerManager playerManager;

    [Header("-----Stats-----")]
    [SerializeField] private int baseDamage;
    [field: SerializeField] public float KnockbackForce { get;  set; }
    [field: SerializeField] public bool ShouldKnockback { get; private set; }
    [field: SerializeField] public float KnockbackDuration { get; private set; } = 0.5f;

    [SerializeField] private WeaponDataSO newWeapon;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EquipNewWeapon(newWeapon);
        }
    }
    public void SetWeaponActive(bool isActive, int index)
    {
        if (isActive)
            damageColliders[index].gameObject.layer = LayerMask.NameToLayer("Enemy");
        else if (!isActive)
            damageColliders[index].gameObject.layer = 3;
    }
    /// <summary>
    /// Used in Animation Events to toggle the knockback effect on and off.
    /// </summary>
    public void ShouldKnockBackSwitcher()
    {
        ShouldKnockback = !ShouldKnockback;
    }
    public void SpawnDamageText(Collider other)
    {
        damageText.Spawn(other.transform.position, EquippedWeaponDataSO.maxDamage);
    }
    public void EquipNewWeapon(WeaponDataSO weaponData)
    {
        if (EquippedWeaponDataSO != null)
        {
            EquippedWeaponDataSO = null;
            Transform handTransform = playerManager.PlayerStateMachine.Animator
            .GetBoneTransform(HumanBodyBones.RightIndexProximal);
            foreach (GameObject child in handTransform)
            {
                Destroy(child.gameObject);
            }
        }
        Transform equipSlot = playerManager.PlayerStateMachine.Animator
       .GetBoneTransform(HumanBodyBones.RightIndexProximal);

        GameObject newWeaponInstance = Instantiate(weaponData.weaponPrefab, equipSlot);
        newWeaponInstance.transform.localPosition = Vector3.zero;
        newWeaponInstance.transform.localRotation = Quaternion.identity;

        // Assign new weapon data
        EquippedWeaponDataSO = weaponData;
    }
}
