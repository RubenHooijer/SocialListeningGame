using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using XNode;

namespace Dialogue {

    [NodeTint("#5C7C6A")]
    [CreateNodeMenu("Chat", order = 1)]
    public class Chat : DialogueBaseNode, IChat {

        public string Text => text.GetLocalizedString().Result;
        public AudioClip SpeechAudioClip => audio;
        public int AnswerCount => answers.Count;
        public List<Answer> Answers => answers;

        public CharacterInfo character;
        public LocalizedString text;
        public AudioClip audio;
        [Output(dynamicPortList = true)] public List<Answer> answers = new List<Answer>();

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Count == 0) {
                port = GetOutputPort("output");
            } else {
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            string portName = from.fieldName;
            if (!portName.Contains("answers")) { return; }

            if (to.node is Answer answer && int.TryParse(portName[portName.Length - 1].ToString(), out int answerIndex)) {
                answers[answerIndex] = answer;
            }
        }

        //#if UNITY_EDITOR
        //        [ContextMenu("Refresh dialogue")]
        //        private void CreateDialogue() {
        //            LocalizationSettings.StringDatabase.GetTableAsync(text.TableReference).Completed += OnTableLoaded;
        //        }

        //        private void OnTableLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> obj) {
        //            SharedTableData sharedData = obj.Result.SharedData;
        //            List<SharedTableData.SharedTableEntry> entries = sharedData.Entries;
        //            Dictionary<(string, long), LocalizedString> keyStringDictionary = new Dictionary<(string, long), LocalizedString>();

        //            foreach (SharedTableData.SharedTableEntry entry in entries) {
        //                LocalizedString localizedString = new LocalizedString();
        //                localizedString.SetReference(text.TableReference, entry.Id);

        //                keyStringDictionary.Add((entry.Key, entry.Id), localizedString);
        //            }

        //            float maxColumnSize = 5;
        //            float columnSpacing = 500;
        //            float rowSpacing = 300;
        //            int columnIndex = 0;
        //            int rowIndex = 0;

        //            List<(Node node, string[] answerKeys)> spawnedNodes = new List<(Node node, string[] answerKeys)>();
        //            IEnumerable<KeyValuePair<(string, long), LocalizedString>> nodeKeys = keyStringDictionary.Where(x => !x.Key.Item1.Contains('-'));
        //            foreach (KeyValuePair<(string, long), LocalizedString> nodeKey in nodeKeys) {
        //                Vector2 position = new Vector2(columnSpacing * columnIndex, rowSpacing * rowIndex);
        //                spawnedNodes.Add(SpawnNode(nodeKey.Key.Item1, position, sharedData, keyStringDictionary));
        //                columnIndex += 1;
        //                if (columnIndex >= maxColumnSize) {
        //                    columnIndex = 0;
        //                    rowIndex += 1;
        //                }
        //            }
        //            UnityEditor.AssetDatabase.SaveAssets();

        //            EditorCoroutineUtility.StartCoroutineOwnerless(ConnectNodes(spawnedNodes));
        //        }

        //        private IEnumerator ConnectNodes(List<(Node node, string[] answerKeys)> spawnedNodes) {
        //            Debug.Log("Gimme 5 seconds to bind the connections");
        //            yield return new EditorWaitForSeconds(5);

        //            for (int i = 0; i < spawnedNodes.Count; i++) {
        //                Node node = spawnedNodes[i].node;
        //                string[] answerKeys = spawnedNodes[i].answerKeys;

        //                for (int a = 0; a < answerKeys.Length; a++) {
        //                    string connectingKey = answerKeys[a].Split('-')[1];
        //                    if (connectingKey == "x") { continue; }

        //                    Node connectingNode = spawnedNodes.First(x => x.node.name == connectingKey).node;
        //                    NodePort outputPort = node.GetOutputPort($"answers {a}");
        //                    if (outputPort == null) { continue; }

        //                    outputPort.ClearConnections();
        //                    outputPort.Connect(connectingNode.GetInputPort("input"));
        //                }
        //            }
        //        }

        //        private (Node node, string[] answerKeys) SpawnNode(string nodeKey, Vector2 position, SharedTableData sharedData, Dictionary<(string, long), LocalizedString> keyStringDictionary) {
        //            IEnumerable<(string, long)> answerKeyIds = keyStringDictionary.Keys.Where(x => x.Item1.Contains('-') && x.Item1.Split('-')[0] == nodeKey);
        //            List<string> answerKeys = new List<string>();
        //            LocalizedString localizedStringText = new LocalizedString();
        //            localizedStringText.SetReference(sharedData.TableCollectionNameGuid, nodeKey);

        //            Node existingNode = graph.nodes.FirstOrDefault(x => x.name == nodeKey);
        //            if (existingNode != null) {
        //                if (existingNode is Chat chatNode) {
        //                    chatNode.text = localizedStringText;

        //                    chatNode.answers = new List<LocalizedString>();
        //                    foreach ((string key, long id) answerKeyId in answerKeyIds) {
        //                        chatNode.answers.Add(keyStringDictionary[answerKeyId]);
        //                        answerKeys.Add(answerKeyId.key);
        //                    }

        //                    return (chatNode, answerKeys.ToArray());
        //                } else {
        //                    PictureChat pictureChatNode = (PictureChat)existingNode;
        //                    pictureChatNode.text = localizedStringText;
        //                    return (pictureChatNode, answerKeys.ToArray());
        //                }
        //            } else if (answerKeyIds.Any()) {
        //                Chat chatNode = graph.AddNode<Chat>();

        //                chatNode.character = character;
        //                chatNode.text = localizedStringText;
        //                chatNode.name = nodeKey;
        //                chatNode.position = position;

        //                foreach ((string key, long id) answerKeyId in answerKeyIds) {
        //                    chatNode.answers.Add(keyStringDictionary[answerKeyId]);
        //                    answerKeys.Add(answerKeyId.key);
        //                }

        //                UnityEditor.AssetDatabase.AddObjectToAsset(chatNode, graph);
        //                return (chatNode, answerKeys.ToArray());
        //            } else {
        //                PictureChat pictureChatNode = graph.AddNode<PictureChat>();

        //                pictureChatNode.character = character;
        //                pictureChatNode.text = localizedStringText;
        //                pictureChatNode.name = nodeKey;
        //                pictureChatNode.position = position;

        //                UnityEditor.AssetDatabase.AddObjectToAsset(pictureChatNode, graph);
        //                return (pictureChatNode, answerKeys.ToArray());
        //            }
        //        }

        //    }
        //#endif

    }

}