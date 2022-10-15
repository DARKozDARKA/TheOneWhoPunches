using System.Linq;
using CodeBase.StaticData.Strings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.StaticData.ScriptableObjects
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelData = (LevelData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.Key = SceneManager.GetActiveScene().name;

                levelData.SpawnPoints = GameObject
                    .FindGameObjectsWithTag(GameTags.SpawnPoint)
                    .Select(_ => _.transform.position)
                    .ToArray();
            }

            EditorUtility.SetDirty(target);
        }
    }
}