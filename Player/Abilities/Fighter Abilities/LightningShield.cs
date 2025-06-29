using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Utility.UI;
using UnityEngine;

namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
    public class LightningShield : Subject
    {
        [SerializeField] LightningShieldAbility _skill;
        //[SerializeField] int maxCharges = 3;
        //[SerializeField] int currentCharges;
        //public int MaxCharges => maxCharges;
        //public int CurrentCharges => currentCharges;

        [SerializeField] float checkRadius = 2f;
        [SerializeField] LayerMask projectileLayer;

       
        

        
        private Quaternion initialRotation;
        private void OnEnable()
        {
            _skill.CurrentCharges = _skill.MaxCharges; // Reset charges when the shield is enabled
            initialRotation = transform.rotation;
            //currentCharges = maxCharges;
            _skill.Timer= _skill.GetMaxDuration();
            this.gameObject.layer = LayerMask.NameToLayer("Shield");
        }

        private void OnDisable()
        {
            this.gameObject.layer = LayerMask.NameToLayer("IgnoreAllCollisions");
        }

        private void Update()
        {
            _skill.Timer -= Time.deltaTime;
            if (_skill.CurrentCharges <= 0 || _skill.Timer <= 0f)
            {
                this.gameObject.SetActive(false);
                return;
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius, projectileLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<EnemyProjectileSpell>(out var enemyProjectile))
                {
                    if (_skill.CurrentCharges <= 0)
                    {
                        this.gameObject.SetActive(false);
                        break;
                    }
                   
                    enemyProjectile.ReflectBack();
                    _skill.CurrentCharges--;
                    NotifyObservers(); // Notify observers about charge change
                }
                //if (hit.TryGetComponent<IProjectile>(out var projectile))
                //{
                //    //projectile.AimLocation(hit.transform);
                //    projectile.Disable();
                //}
            }
        }
        private void LateUpdate()
        {
            // Dont rotate shield with player
            transform.rotation = initialRotation;
        }
       
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }

    }
}
