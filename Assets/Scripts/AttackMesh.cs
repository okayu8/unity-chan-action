using System.Linq;
using UnityEditor;
using UnityEngine;

public class AttachMeshColliders : MonoBehaviour
{
    [MenuItem("Utility/Attach mesh colliders")]
    public static void Attach()
    {
        var selected = Selection.gameObjects;
        foreach (var gameObject in selected)
        {
            // 選択されたオブジェクトおよび子階層からMeshFilterを持っているものを抜き出し、そのうちColliderを持っていないものを抜き出す
            var objectsWithoutCollider = gameObject.GetComponentsInChildren<MeshFilter>()
                .Select(m => m.gameObject)
                .Distinct()
                .Where(o => o.GetComponent<Collider>() == null);
            foreach (var target in objectsWithoutCollider)
            {
                // MeshColliderを追加する
                // 念のため取り消し履歴に記録を残すようにしました
                // 完了まで数秒～数十秒フリーズするかもしれません
                // 終わるとコンソールにずらずらと追加されたオブジェクトが並ぶと思います
                Undo.AddComponent<MeshCollider>(target);
                Debug.LogFormat(target, "Collider added: {0}", target.name);
            }
        }
    }
}