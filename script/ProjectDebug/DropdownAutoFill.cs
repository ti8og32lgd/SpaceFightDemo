using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace script.ProjectDebug
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownAutoFill : MonoBehaviour
    {
        public List<string> unitValues=new();
        private TMP_Dropdown _dropdown;
        private void Start()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            
            
            _dropdown.options = new List<TMP_Dropdown.OptionData>(unitValues.Count);
           
            for(var i=0;i<unitValues.Count;i++)
            {
                _dropdown.options.Add( new TMP_Dropdown.OptionData(unitValues[i]));
            }
        }
    }
}