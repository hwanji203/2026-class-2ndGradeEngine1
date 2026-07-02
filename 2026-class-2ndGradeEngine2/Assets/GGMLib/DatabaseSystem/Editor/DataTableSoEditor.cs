using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codice.Client.BaseCommands.BranchExplorer;
using GGMLib.DatabaseSystem.Runtime;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGMLib.DatabaseSystem.Editor
{
    [UnityEditor.CustomEditor(typeof(DataTableSO))]
    public class DataTableSoEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView;
        
        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (editorView == null)
            {
                root.Add(new Label("[DataTableSO Editor] VisualTree asset 이 할당되지 않았습니다."));
                return root;
            }

            editorView.CloneTree(root);

            Button validateButton = root.Q<Button>("validate-button");
            ScrollView resultScroll = root.Q<ScrollView>("result-scroll");
            Label resultLabel = root.Q<Label>("result-label");

            validateButton.clicked += () =>
            {
                DataTableSO dataTable = (DataTableSO)target;
                (string message, bool isPassed) = BuildValidationResult(dataTable);

                resultLabel.text = message;
                resultLabel.RemoveFromClassList("result-label--pass");
                resultLabel.RemoveFromClassList("result-label--fail");
                resultLabel.AddToClassList(isPassed ? "result-label--pass" : "result-label-fail");
                resultScroll.style.display = DisplayStyle.Flex;
            };
            return root;
        }

        private (string message, bool isPassed) BuildValidationResult(DataTableSO dataTable)
        {
            IndexedAsset[] assets = dataTable.assets;
            if (assets == null || assets.Length == 0)
                return ("검증완료", true);

            StringBuilder sb = new StringBuilder();
            HashSet<IndexedAsset> failedAssets = new HashSet<IndexedAsset>();

            var duplicatedIndexGroups = assets.GroupBy(asset => asset.AssetIndex)
                .Where(group => group.Count() > 1);

            foreach (var group in duplicatedIndexGroups)
            {
                sb.AppendLine($"[중복 인덱스 {group.Key}]");
                foreach (IndexedAsset asset in group)
                {
                    sb.AppendLine($"{asset.AssetIndex} : {asset.AssetName}");
                    failedAssets.Add(asset);
                }
            }

//Asset이름 비운 것도 검사.
            var blankNameAssets = assets.Where(asset => string.IsNullOrWhiteSpace(asset.AssetName));

            bool blankedHeaderPrinted = false;
            foreach (IndexedAsset asset in blankNameAssets)
            {
                if (!blankedHeaderPrinted)
                {
                    sb.AppendLine($"[비어있는 이름] {asset.AssetName}");
                    blankedHeaderPrinted = true;
                }
                sb.AppendLine($"{asset.AssetIndex} : (이름공백)");
                failedAssets.Add(asset);
            }
            
            var duplicatedNameGroups = assets
                .Where(asset => !string.IsNullOrWhiteSpace(asset.AssetName))
                .GroupBy(asset => asset.AssetName)
                .Where(group => group.Count() > 1);

            foreach (var group in duplicatedNameGroups)
            {
                sb.AppendLine($"[중복 이름 {group.Key}]");
                foreach (IndexedAsset asset in group)
                {
                    sb.AppendLine($"{asset.AssetName} : {asset.AssetName}");
                    failedAssets.Add(asset);
                }
            }

            if (failedAssets.Count == 0)
                return ("검증완료", true);

            return (sb.ToString().TrimEnd(), false);
        }
    }
}