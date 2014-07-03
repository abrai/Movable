using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MovableControl
{
    public partial class MovableUserControl : Core.Windows.Forms.EditableControl
    {
        public MovableUserControl()
        {
            InitializeComponent();
            //Add the Movable Control            
            base.EditableControls.Add(this.pnlContaner);
            base.EditableControls.Add(this.textBox2);
            base.EditableControls.Add(this.label2);
            // prepare EditableControl
            base.initialize();
        }

        private void MovableUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
