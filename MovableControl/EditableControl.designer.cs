namespace Core.Windows.Forms
{
    partial class EditableControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EditableControl
            // 
            this.Name = "EditableControl";
            this.Size = new System.Drawing.Size(308, 225);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEventHandler);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveEventHandler);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpEventHandler);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
