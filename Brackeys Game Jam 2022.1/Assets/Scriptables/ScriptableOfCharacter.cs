using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecieTypes  {Human, Zombie};
public enum GroupTypes   {Civil, Police, Military, Survivor, Medic};
public enum AttackTypes  {None, Melee, Shoot, Special}; // IMPORTANTE: o zumbi não tem o tipo "Shoot", 
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


    [Tooltip("[Human] quantos humanos já devem ter sido mortos para começar a aparecer no mapa")]  public int   humansNeededDie;

    [Header("Health")]
        [Tooltip("[Human/Zombie] quantidade de vida")]                                             public int   health;
        [Tooltip("[Zombie] preço do upgrade da vida")]                                             public int   healthPrice;
        [Tooltip("[Zombie] o quanto a vida vai aumentar a cada upgrade")]                          public int   healthMultiply;
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public int   healthMultiplyPrice;
        [Tooltip("[Zombie] o máximo que a vida pode chegar (-1 = infinito)")]                      public float healthMax;


    [Header("Damage")]
        [Tooltip("[Human/Zombie] quanto causa de dano")]                                           public float damage;
        [Tooltip("[Zombie] preço do upgrade do dano")]                                             public int   damagePrice;
        [Tooltip("[Zombie] o quanto o dano vai aumentar a cada upgrade")]                          public int   damageMultiply;
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public int   damageMultiplyPrice;
        [Tooltip("[Zombie] o máximo que o dano pode chegar (-1 = infinito)")]                      public float damageMax;
    

    [Header("Speed")]
        [Tooltip("[Human/Zombie] velocidade de movimento")]                                        public float speed;
        [Tooltip("[Zombie] preço do upgrade")]                                                     public int   speedPrice;
        [Tooltip("[Zombie] o quanto a velocidade vai aumentar a cada upgrade")]                    public int   speedMultiply;
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public int   speedMultiplyPrice;
        [Tooltip("[Zombie] o máximo que a velocidade pode chegar (-1 = infinito)")]                public float speedMax;


    [Header("AttackDelay")]
        [Tooltip("[Human/Zombie] tempo entre os ataques")]                                         public float attackDelay;
        [Tooltip("[Zombie] preço do upgrade")]                                                     public int   attackDelayPrice;
        [Tooltip("[Zombie] o quanto o tempo vai diminuir a cada upgrade")]                         public int   attackDelayMultiply;
        [Tooltip("[Zombie] o quanto o preço do upgrade vai aumentar a cada compra")]               public int   attackDelayMultiplyPrice;
        [Tooltip("[Zombie] o mínimo que o tempo pode chegar (precisa ser maior ou igual a 0)")]    public float attackDelayMax;
    
    



    // retorna todos os dados do tipo float de uma forma genérica (ótimo para upgrades na lojinha)
    public Dictionary<string, List<float>> GetFloatProperties()
    {
        // OBS: Não remover uma propriedade depois de adicionar

        // IMPORTANTE PARA MODIFICAÇÕES FUTURAS:
            // quando for necessário acessar os dados padrões, deve ser acessado o scriptable, e não SaveGame.
            // por exemplo, imagina que no futuro "damageMax" é aumentado, mas se acessar o dado do SaveGame o valor não estará atualizado corretamente

        return new Dictionary<string, List<float>>()
        {
            {"health",          new List<float>{health,      healthPrice,      healthMultiply,      healthMultiplyPrice,      healthMax}},
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
