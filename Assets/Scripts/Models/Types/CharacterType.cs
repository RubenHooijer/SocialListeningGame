using Dialogue;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System.Collections.Generic;
using XNode;

[CreateAssetMenu(menuName = "SLG/Type/Character")]
public class CharacterType : ScriptableObject {

    public string Name;
    public DialogueGraph DialogueGraph;

    public void LoadAllTablesFromDialogueGraph() {
        List<TableReference> tableReferences = new List<TableReference>();
        List<Node> nodes = DialogueGraph.nodes;

        for (int i = 0; i < nodes.Count; i++) {
            Node node = nodes[i];
            if (node is ILoadableTableReference loadableTableReference && !tableReferences.Contains(loadableTableReference.TableReference)) {
                tableReferences.Add(loadableTableReference.TableReference);
            }
        }

        LocalizationSettings.StringDatabase.PreLoadTables(tableReferences).Completed += x => Debug.Log("StringDatabase is done loading");
        LocalizationSettings.AssetDatabase.PreLoadTables(tableReferences).Completed += x => Debug.Log("AssetDatabase is done loading");
    }

}