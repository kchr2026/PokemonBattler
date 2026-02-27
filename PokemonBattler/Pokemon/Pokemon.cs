namespace PokemonBattler;
using System.Collections.Generic;
public enum PokemonType {
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy,
    Stellar // Gen 9
};
public class Stats
{
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Speed { get; set; }
}

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public PokemonType Type1 { get; set; }
    public PokemonType? Type2 { get; set; }
    public List<Move> Moves { get; set; }
    public Stats BaseStats { get; set; }
    public Stats Ev { get; set; }
    public Stats Iv { get; set; }

    private int CalculateHp()
    {
        // Official pokemon HP formula
        return ((2 * BaseStats.HP + Iv.HP + (Ev.HP / 4)) * Level / 100) + Level + 10;
    }
    public Pokemon(string name, int level, PokemonType type1, PokemonType? type2, Stats baseStats, List<Move> moves)
    {

        Moves = moves;
        Name = name;    
        Level = level;
        Type1 = type1;
        Type2 = type2;
        BaseStats = baseStats;
        Ev = new Stats(); // defaults to 0
        Iv = new Stats(); // defaults to 0
        Moves = new List<string>();
        MaxHP = CalculateHp();
        HP = MaxHP;
    }
}

