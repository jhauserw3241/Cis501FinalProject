using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace xmlUsage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ux_openFileMenuItem_Click(object sender, EventArgs e)
        {
            if (ux_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder output = new StringBuilder();

                using (XmlTextReader reader = new XmlTextReader(ux_openFileDialog.FileName))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                output.Append(reader.Name + ":" + Environment.NewLine);

                                while (reader.MoveToNextAttribute()) // Read the attributes.
                                    output.Append("\t" + reader.Name + " = " + reader.Value + Environment.NewLine);
                                break;
                            case XmlNodeType.Text: //Display the text in each element.
                                output.Append("\t" + reader.Value + Environment.NewLine);
                                break;
                        }
                    }

                    reader.Close();
                }

                switch (ux_tabPanel.SelectedIndex)
                {
                    case 0:
                        ux_label0.Text = output.ToString();
                        break;
                    case 1:
                        ux_label1.Text = output.ToString();
                        break;
                }
            }
        }
    }
}
