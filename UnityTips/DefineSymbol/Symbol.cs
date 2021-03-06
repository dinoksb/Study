using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor
{
    static class Symbol
    {
        private static IEnumerable<string> currentSymbols;

        private static readonly BuildTargetGroup[] buildTargetGroup = new[]
        {
            BuildTargetGroup.Android, 
            BuildTargetGroup.Standalone, 
        };

        public static string CurrentSymbols
        {
            get { return String.Join(";", currentSymbols.ToArray()); }
        }

        static Symbol()
        {
            currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');
        }

        private static void SaveSymbol()
        {
            var symbols = CurrentSymbols;
            foreach (var target in buildTargetGroup)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols);
            }
        }



        public static void Add(params string[] symbols)
        {
            currentSymbols = currentSymbols.Concat(symbols).Distinct().ToArray();
            SaveSymbol();
        }

        public static void Remove(params string[] symbols)
        {
            currentSymbols = currentSymbols.Except(symbols).ToArray();

            SaveSymbol();
        }

        public static void Set(params string[] symbols)
        {
            currentSymbols = symbols;
            SaveSymbol();
        }

    }
}