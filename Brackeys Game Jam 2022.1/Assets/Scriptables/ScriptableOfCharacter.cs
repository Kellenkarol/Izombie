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
    [Tooltip("[Zombie] se já foi desbloqueado")]
        public bool unlocked;


    // importante para salvamento de dados
    [Header("Identification")]
        [Tooltip("[Human/Zombie] Espécie")]                                                        public SpecieTypes specieType;
        [Tooltip("[Human/Zombie] Tipo de grupo")]                                                  public GroupTypes groupType;
        [Tooltip("[Human/Zombie] Tipo de ataque (Zumbi não tem o tipo 'Shoot')")]                  public AttackTypes attackType; 


    [Tooltip("[Human] quantos humanos já devem ter sido mortos para começar a aparecer no mapa")]  public float humansNeededDie;        // [Human] quantos humanos já devem ter sido mortos para poder aparecer no mapa 


    [Header("Damage")]
        [Tooltip("[Human/Zombie] quanto causa de dano")]                                           public float damage;        // quanto causa de dano
        [Tooltip("[Zombie] preço do upgrade do dano")]                                             public float damagePrice;        // preço do upgrade do dano
        [Tooltip("[Zombie] o quanto o dano vai aumentar a cada upgrade")]                          public float damageMultiply;          // o quanto o dano vai aumentar a cada upgrade
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public float damageMultiplyPrice;          // o quanto o preço do upgrade vai aumentar a cada compra
        [Tooltip("[Zombie] o máximo que o dano pode chegar (-1 = infinito)")]                      public float damageMax;          // o quanto o preço do upgrade vai aumentar a cada compra
    

    [Header("Speed")]
        [Tooltip("[Human/Zombie] velocidade de movimento")]                                        public float speed;        // velocidade de movimento
        [Tooltip("[Zombie] preço do upgrade")]                                                     public float speedPrice;        // preço do upgrade do dano
        [Tooltip("[Zombie] o quanto a velocidade vai aumentar a cada upgrade")]                    public float speedMultiply;          // o quanto o dano vai aumentar a cada upgrade
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public float speedMultiplyPrice;          // o quanto o preço do upgrade vai aumentar a cada compra
        [Tooltip("[Zombie] o máximo que a velocidade pode chegar (-1 = infinito)")]                public float speedMax;          // o quanto o preço do upgrade vai aumentar a cada compra


    [Header("AttackDelay")]
        [Tooltip("[Human/Zombie] tempo entre os ataques")]                                         public float attackDelay;        // tempo entre os ataques
        [Tooltip("[Zombie] preço do upgrade")]                                                     public float attackDelayPrice;        // preço do upgrade
        [Tooltip("[Zombie] o quanto o tempo vai diminuir a cada upgrade")]                         public float attackDelayMultiply;          // o quanto o tempo vai diminuir a cada upgrade
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public float attackDelayMultiplyPrice;          // o quanto o preço do upgrade vai aumentar a cada compra
        [Tooltip("[Zombie] o mínimo que o tempo pode chegar (precisa ser maior ou igual a 0)")]    public float attackDelayMax;          // o quanto o preço do upgrade vai aumentar a cada compra
    
    



    // retorna todos os dados do tipo float de uma forma genérica (ótimo para upgrades na lojinha)
    public Dictionary<string, List<float>> GetFloatProperties()
    {
        // OBS: Não remover uma propriedade depois de adicionar
        return new Dictionary<string, List<float>>()
        {
            {"damage",          new List<float>{damage,      damagePrice,      damageMultiply,      damageMultiplyPrice,      damageMax}},
            {"speed",           new List<float>{speed,       speedPrice,       speedMultiply,       speedMultiplyPrice,       speedMax}},
            {"attackDelay",     new List<float>{attackDelay, attackDelayPrice, attackDelayMultiply, attackDelayMultiplyPrice, attackDelayMax}},
            
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
