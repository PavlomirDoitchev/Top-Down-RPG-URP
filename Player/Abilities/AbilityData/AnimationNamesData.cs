using UnityEngine;

[CreateAssetMenu(fileName = "AnimationNamesData", menuName = "Abilities/AnimationNamesData")]
public class AnimationNamesData : ScriptableObject
{
	#region Common Armed
	[Header("Common Armed")]
	public string Locomotion = "Locomotion";
	public string SheathWeapon = "SheathWeapon";
	public string Jump = "Jump";
	public string Dodge = "Dodge";
	public string Fall = "Fall";
	public string Interact = "Interact"; 
	public string Stunned = "Stunned";	
	public string Hit = "Hit";
	public string Death = "Death";
	#endregion

	#region Common Unarmed
	[Space(20)]
	[Header("Common Unarmed")]
	public string Unarmed_Locomotion = "Unarmed_Locomotion";
	public string Unarmed_SheathWeapon = "Unarmed_SheathWeapon";
	public string Unarmed_Jump = "Unarmed_Jump";
	public string Unarmed_Dodge = "Unarmed_Dodge";
	public string Unarmed_Fall = "Unarmed_Fall";
	public string Unarmed_Stunned = "Unarmed_Stunned";
	public string Unarmed_Hit = "Unarmed_Hit";
	public string Unarmed_Death = "Unarmed_Death";
	#endregion

	#region Attacks 2H Weapon
	[Space(20)]
	[Header("Attacks")]
	public string Basic_Attack_Chain_One = "BasicAttackChainOne";
	public string Basic_Attack_Chain_Two = "BasicAttackChainTwo";
	public string Basic_Attack_Chain_Three = "BasicAttackChainThree";
	public string Ability_One = "Ability_1";
	public string Ability_Two = "Ability_2";
	public string Ability_Three = "Ability_3";
	public string Ability_Four = "Ability_4";
	#endregion


	#region Attacks 1H Weapon
	[Space(20)]
	[Header("1H Weapon Attacks")]
	public string OneHanded_Basic_Attack_Chain_One = "OneHanded_BasicAttackChainOne";
	public string OneHanded_Basic_Attack_Chain_Two = "OneHanded_BasicAttackChainTwo";
	public string OneHanded_Basic_Attack_Chain_Three = "OneHanded_BasicAttackChainThree";
	public string OneHanded_Ability_One = "OneHanded_Ability_1";
	public string OneHanded_Ability_Two = "OneHanded_Ability_2";
	public string OneHanded_Ability_Three = "OneHanded_Ability_3";
	public string OneHanded_Ability_Four = "OneHanded_Ability_4";
	#endregion

	#region Attacks Dual Wield	
	[Space(20)]
	[Header("Dual Wield Attacks")]
	public string DualWield_Basic_Attack_Chain_One = "DualWield_BasicAttackChainOne";
	public string DualWield_Basic_Attack_Chain_Two = "DualWield_BasicAttackChainTwo";
	public string DualWield_Basic_Attack_Chain_Three = "DualWield_BasicAttackChainThree";
	public string DualWield_Ability_One = "DualWield_Ability_1";
	public string DualWield_Ability_Two = "DualWield_Ability_2";
	public string DualWield_Ability_Three = "DualWield_Ability_3";
	public string DualWield_Ability_Four = "DualWield_Ability_4";
	#endregion

	#region Attacks Unarmed
	[Space(20)]
	[Header("Unarmed Attacks")]
	public string Unarmed_Basic_Attack_Chain_One = "Unarmed_BasicAttackChainOne";
	public string Unarmed_Basic_Attack_Chain_Two = "Unarmed_BasicAttackChainTwo";
	public string Unarmed_Basic_Attack_Chain_Three = "Unarmed_BasicAttackChainThree";
	public string Unarmed_Ability_One = "Unarmed_Ability_1";
	public string Unarmed_Ability_Two = "Unarmed_Ability_2";
	public string Unarmed_Ability_Three = "Unarmed_Ability_3";
	public string Unarmed_Ability_Four = "Unarmed_Ability_4";
	#endregion
}
