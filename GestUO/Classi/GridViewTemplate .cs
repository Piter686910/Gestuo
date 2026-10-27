using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestUO.Classi
{
    public class GridViewTemplate : ITemplate
    {
        ListItemType templateType;
        string columnName;
        string controlType;

        public GridViewTemplate(ListItemType type, string colname, string ctrlType)
        {
            templateType = type;
            columnName = colname;
            controlType = ctrlType;
        }

        public void InstantiateIn(Control container)
        {
            if (templateType == ListItemType.Item || templateType == ListItemType.AlternatingItem)
            {
                // Modalità Visualizzazione
                Label lbl = new Label();
                lbl.ID = "lbl" + columnName;
                lbl.DataBinding += new EventHandler(this.BindData);
                container.Controls.Add(lbl);
            }
            else if (templateType == ListItemType.EditItem)
            {
                // Modalità Modifica
                TextBox txt = new TextBox();
                txt.ID = "txt" + columnName;
                txt.Width = 30; // Piccolo
                txt.MaxLength = 1; // Un solo carattere (es. P, A, F)
                txt.DataBinding += new EventHandler(this.BindData);
                container.Controls.Add(txt);
            }
        }

        private void BindData(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            GridViewRow row = (GridViewRow)ctrl.NamingContainer;

            // Collega il valore dal DataItem (la DataRow)
            object dataValue = DataBinder.Eval(row.DataItem, columnName);

            if (ctrl is Label lbl)
            {
                lbl.Text = dataValue.ToString();
            }
            else if (ctrl is TextBox txt)
            {
                txt.Text = dataValue.ToString();
            }
        }
    }
}
