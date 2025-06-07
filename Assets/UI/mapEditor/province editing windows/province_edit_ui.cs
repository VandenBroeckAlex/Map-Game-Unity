
using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace provinceEditUi
{
    public class Province_edit_ui : VisualElement
    {
        [UnityEngine.Scripting.Preserve]

        public new class UxmlFactory : UxmlFactory<Province_edit_ui> { }
        public Province_edit_ui()
        {
            VisualElement window = new VisualElement();
            hierarchy.Add(window);
        }
    }
}
