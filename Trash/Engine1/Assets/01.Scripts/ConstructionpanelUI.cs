using TMPro;
using UnityEngine;

namespace _01.Scripts
{
    public class ConstructionpanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI constructionText;

        public void SetConstruction(bool isConstruction)
        {
            constructionText.text = isConstruction ? "Construction mode" : "Normal";
        }
    }
}