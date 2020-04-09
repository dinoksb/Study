using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor
{
    static class SymbolCustom
    {
        private static IEnumerable<string> currentSymbols;

        private static BuildTargetGroup[] buildTargetGroup = null;

        public static string CurrentSymbols
        {
            get { return String.Join(";", currentSymbols.ToArray()); }
        }

        static SymbolCustom()
        {
            currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
        }

        private static void SaveSymbol()
        {
            var symbols = CurrentSymbols;
            foreach (var target in buildTargetGroup)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols);
            }
        }



        public static void Add(BuildTargetGroup[] buildTargetGroups, params string[] symbols)
        {
            buildTargetGroup = buildTargetGroups;
            currentSymbols = currentSymbols.Concat(symbols).Distinct().ToArray();

            SaveSymbol();
        }

        public static void Remove(BuildTargetGroup[] buildTargetGroups, params string[] symbols)
        {
            buildTargetGroup = buildTargetGroups;
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