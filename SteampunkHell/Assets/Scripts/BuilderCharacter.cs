using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCharacter 
{
    string _name = "NN";
    int _life = 100;
    Prof _currentProf;
    Color _col;

    public BuilderCharacter SetLife(int life)
    {
        _life = life;

        return this;
    }

    public BuilderCharacter SetName(string name)
    {
        _name = name;

        return this;
    }

    public BuilderCharacter SetProf(Prof currentProf)
    {
        _currentProf = currentProf;

        return this;
    }

    public BuilderCharacter SetColor(Color col)
    {
        _col = col;

        return this;
    }

    /// <summary>
    /// Joel pasame un valor ROJO, de 0 a 1, y uno VERDE, de 0 a 1, y un valor AZUL, de 0 a 1
    /// </summary>
    /// <param name="R"></param>
    /// <param name="G"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public BuilderCharacter SetColor(float R, float G, float B)
    {
        _col = Color.HSVToRGB(R, G, B);

        return this;
    }

    public int GetLife() => _life;
    public string GetName() => _name;
    public Prof GetProf() => _currentProf;
    public Color GetColor() => _col;
}
