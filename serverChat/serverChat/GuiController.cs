using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Windows.Forms;

namespace serverChat
{
    public class GuiController
    {
        public event ServerOutputHandler Output;

        // Constructor
        public GuiController()
        {
        }

        #region Handle View Output
        // Handle Generic Input
        //
        // Handle input that provides the generic event arugments
        // @param sender The component that raised the event
        // @param e The supplied arguments
        public void HandleGenericInput(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "usersButton":
                    Output("UpdateUserList", "");
                    break;
                case "convButton":
                    Output("UpdateConversationList", "");
                    break;
                default:
                    Output("InvalidInputOption", "");
                    break;
            }
        }

        // Handle Mouse Input
        //
        // Handle input that provides the mouse event arugments
        // @param sender The component that raised the event
        // @param e The supplied arguments
        public void HandleMouseInput(object sender, MouseEventArgs e)
        {
            switch(((ListBox)sender).Name)
            {
                case "eleListBox":
                    Output("ProvideEleName", ((ListBox)sender).SelectedIndex);
                    break;
                default:
                    Output("InvalidInputOption");
                    break;
            }
        }
        #endregion
    }
}
