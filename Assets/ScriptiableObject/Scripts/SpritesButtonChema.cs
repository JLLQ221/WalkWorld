using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ActionType { Move, Interact, Dash, Attack, Plant, Jump }

[CreateAssetMenu(fileName = "SpritesButtonChema", menuName = "Scriptable Objects/SpritesButtonChema")]
public class SpritesButtonChema : ScriptableObject
{
    public TMP_SpriteAsset pcAsset;
    public TMP_SpriteAsset controllerAsset;
    public TMP_SpriteAsset selectAsset;

    [System.Serializable]
    public struct ActionMapping
    {
        public ActionType action;
        public int variant;
        public int indexPC;
        public int indexController;
    }

    [SerializeField] private List<ActionMapping> mappings;

    public int GetIndex(ActionType action, bool isGamepad, int variant)
    {
        if (isGamepad)
        {
            selectAsset = controllerAsset;
        }
        else
        {
            selectAsset = pcAsset;
        }

        foreach (var m in mappings)
        {
            if (m.action == action && variant == m.variant)
                return isGamepad ? m.indexController : m.indexPC;
        }
        return -1;
    }

    public string ReplacePlaceholders(string template, bool isGamepad)
    {
        template = template.Replace("{MOVE}", $"<sprite={GetIndex(ActionType.Move, isGamepad, 0)}>");
        template = template.Replace("{MOVE2}", $"<sprite={GetIndex(ActionType.Move, isGamepad, 1)}>");
        template = template.Replace("{MOVE3}", $"<sprite={GetIndex(ActionType.Move, isGamepad, 2)}>");
        template = template.Replace("{MOVE4}", $"<sprite={GetIndex(ActionType.Move, isGamepad, 4)}>");
        template = template.Replace("{INTERACT}", $"<sprite={GetIndex(ActionType.Interact, isGamepad, 0)}>");
        template = template.Replace("{JUMP}", $"<sprite={GetIndex(ActionType.Jump, isGamepad, 0)}>");
        template = template.Replace("{ATTACK}", $"<sprite={GetIndex(ActionType.Attack, isGamepad, 0)}>");
        template = template.Replace("{ATTACK1}", $"<sprite={GetIndex(ActionType.Attack, isGamepad, 1)}>");
        template = template.Replace("{PLANT}", $"<sprite={GetIndex(ActionType.Plant, isGamepad, 0)}>");
        template = template.Replace("{DASH}", $"<sprite={GetIndex(ActionType.Dash, isGamepad, 0)}>");
        return template;
    }
}
