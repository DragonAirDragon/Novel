using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Naninovel.UI
{
    public class SeasonReloadCancelButton : ScriptableButton
    {
        private SeasonReloadMenu seasonReloadMenu;

        protected override void Awake()
        {
            base.Awake();
            seasonReloadMenu = GetComponentInParent<SeasonReloadMenu>();
        }
        protected override void OnButtonClick() => HideMenu();
        private void HideMenu()
        {
            seasonReloadMenu.Hide();
            
        }
    }
}