using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Form
{
    public class MvcFormButtons : MvcBaseControl
    {
        internal override string TagName { get { return "div"; } }
        internal override bool SelfClosing { get { return false; } }
        internal FormButtonsOptions Options { get; private set; }
        public MvcFormButtons(ControlFactory controlFactory, string callBack, bool showCancel , bool showSave) : base(controlFactory)
        {
            Options = new FormButtonsOptions();
            Options.CallBack = callBack;
            Options.ShowCancel = showCancel;
            Options.showSave = showSave;
        }

        internal override MvcControlAttributes GetAttributes()
        {
            return Options;
        }

        internal override string GetContent()
        {
            var res = $@"<hr />
                     <div style='text-align:left'>"                       ;
            if (Options.showSave)
                res += $@"<button 
                          data-submit='ajax'{(!string.IsNullOrEmpty(Options.CallBack)? $"data-callback='{ Options.CallBack }'" : "") } 
                          class='k-grid-save-command k-button k-button-md k-rounded-md k-button-solid k-button-solid-primary' type='submit'>
                          <span class='k-icon k-i-save k-button-icon'></span>ثبت
                            </button>";

            if (Options.ShowCancel)
                res += @"
                         <button type='button' class='k-grid-cancel-command k-button k-button-md k-rounded-md k-button-solid k-button-solid-base'>
                            <span class='k-icon k-i-cancel-outline k-button-icon'></span>لغو
                        </button>";

            return res + "</div>";
        }
    }
}
