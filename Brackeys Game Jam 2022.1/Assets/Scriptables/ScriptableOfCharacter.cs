using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecieTypes  {Human, Zombie};
public enum GroupTypes   {Civil, Police, Military, Survivor, Medic};

public enum AttackTypes  {Melee, Shoot, Special}; // IMPORTANTE: o zumbi não tem o tipo "Shoot", 
                                                 // então desconsiderar essa opção.


[CreateAssetMenu(fileName = "Character", menuName = "Character/New Character")]
public class ScriptableOfCharacter : ScriptableObject
{
    // importante para salvamento de dados
        public SpecieTypes specieType;
        public GroupTypes groupType;
        public AttackTypes attackType; 


    public float damage;        // quanto causa de dano
    public float speed;         // velocidade de movimento
    public float attackDelay;   // tempo entre os ataques


    // retorna todos os dados de uma forma genérica (ótimo para upgrades na lojinha)
    public Dictionary<string, string> GetDataAsString()
    {
        return new Dictionary<string, string>()
        {
            {"specieType",      specieType.ToString()},
            {"groupType",       groupType.ToString()},
            {"attackType",      attackType.ToString()},
            {"damage",          damage.ToString()},
            {"speed",           speed.ToString()},
            {"attackDelay",   attackDelay.ToString()},
            
            //{"", },
            //...
        };
    }


    // apresentação
    public virtual string Print()
    {
        return $"Eu sou um {specieType} do tipo {groupType} com ataque do tipo {attackType}";
    }


    // nome (é diferente do gameObject.name)
    public string Name()
    {
        return $"{specieType}{groupType}{attackType}"; // EX: HumanMilitarySpecial
    }
}
