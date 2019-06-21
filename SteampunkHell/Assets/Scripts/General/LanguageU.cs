using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class LanguageU
{
    public static Dictionary<Language, Dictionary<string,string>> loadCodexFromString(string source, string sheet)
    {
        //Creamos una variable del tipo que vamos a tener que devolver
        var codex = new Dictionary<Language, Dictionary<string,string>>();

        //un contador de lineas para saber en donde fallo,si es que falla
        int lineNum = 0;
        
        //Cortamos los renglones cada vez que encontremos un salto de linea
        var rows = sheet.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        //Booleano para saber si es la primera linea
        bool first = true;
        //Diccionario para saber en que posicion esta determinada columna, Ej: <Idioma,0>, <Texto,1>
        var columnToIndex = new Dictionary<string, int>();

        //Examinamos cada renglon por separado
        foreach (var row in rows)
        {
            //Sumamos para saber que estamos en la primera linea
            lineNum++;
            //Separamos por columna al encontrar un ";"...tambien se toma la ","
            var cells = row.Split(',');

            //Si es la primera linea 
            if (first)
            {
                first = false;//Ya sabemos que la siguiente no va a ser la primera
                for (int i = 0; i < cells.Length; i++)
                    columnToIndex[cells[i]] = i; //Guardamos el indice de donde se encuentra esa columna que tiene un nombre,en nuestro caso ID,Lenguaje,Texto,etc
                continue;
            }

            //Si detectamos que hay una diferencia en las columnas avisamos en consola para que sepamos que algo falla en el documento
            if (cells.Length != columnToIndex.Count)
            {
                Debug.Log(string.Format("Parsing CSV file {2} at line {0} columns {1} should be 4", lineNum, cells.Length, source));
                continue;
            }

            var langName = cells[columnToIndex["Idioma"]];//Le decimos que tome el valor de la columna que se encuentra en Idioma

            Language lang;

            try
            {
                lang = (Language)Enum.Parse(typeof(Language), langName);
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Parsing CSV file {2} at line {0} invalid language {1}", lineNum, langName, source));
                Debug.Log(e.ToString());
                continue;
            }

            var idName = cells[columnToIndex["ID"]];//Le decimos que tome la ID correspondiente
            var text = cells[columnToIndex["Texto"]];//Le decimos que tome el Texto correspondiente

            if (!codex.ContainsKey(lang))// Si el diccionario no contiene ese idioma
            {
                codex[lang] = new Dictionary<string,string>();//La Creamos
            }
            codex[lang][idName] = text;//Le decimos al diccionario que en X lenguaje, utilizando Y id vamos a guardar Z texto
        }
        return codex;
    }
}
