#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

#endregion

namespace Core.Windows.Forms
{
   
    /// EditableControl allows user to place Contained Controls as desired.
    /// Inherit from EditableControl, add some cotrols to be moved and
    /// in inherited (subclass/subform) and in default ctor add 
    public partial class EditableControl:UserControl
    {
        private Point _point_begin;                  /// Point where from Control will be moved
        private Point _point_end;                    /// Point where to Control will be moved
        private Control _control_moving = null;     /// Moved Control
        private int x_offset_on_client_control = 0; /// x offeset on client control
        private int y_offset_on_client_control = 0; /// y offeset on client control
        public ArrayList EditableControls;           /// Container for defining set of movable/editable Controls
        Hashtable htCoordinte=new Hashtable();
        public const string strFileName = "Coordinate.xml";
        XmlSerializer ser = new XmlSerializer(typeof(DataSet));
        DataSet ds = new DataSet();
        DataTable tblCoordinate = new DataTable();
        public EditableControl()
        {
            InitializeComponent();
            EditableControls = new ArrayList();
        }

        /// initialize memeber function hooks mouse events of all child 
        /// controls to mouse event handlers of the EditableControl
        protected virtual void initialize()
        {
            foreach (Control ctrl in Controls)
            {
                ctrl.MouseDown += new MouseEventHandler(MouseDownEventHandler);
                ctrl.MouseUp += new MouseEventHandler(MouseUpEventHandler);
                ctrl.MouseMove += new MouseEventHandler(MouseMoveEventHandler);
            }
            DeSerialize();
            return;
        }

       /// MouseDownEventHandler detects whether child control was hit and
        /// sets the starting point for the move of the child control
        protected virtual void MouseDownEventHandler(object sender, MouseEventArgs evnt_args_mouse)
        {
            int x;
            int y;
            if (
                // while left button pressed (could define other keys)
                evnt_args_mouse.Button == MouseButtons.Left
                &&
                // ignore containing (parent) control
                !"Core.Windows.Forms.EditableControl"
                .Equals(sender.GetType().ToString())
                &&
                // control is defined as movable
                EditableControls.Contains(sender)
               )
            {
                Point pt;

                pt = Cursor.Position;
                x_offset_on_client_control = evnt_args_mouse.X;
                y_offset_on_client_control = evnt_args_mouse.Y;

                x = x_offset_on_client_control + ((Control)sender).Location.X;
                y = y_offset_on_client_control + ((Control)sender).Location.Y;
                pt = new Point(x, y);

                _point_begin = pt;

                foreach (Control ctrl in Controls)
                {
                    if (ctrl.Bounds.Contains(_point_begin))
                    {
                        _control_moving = ctrl;
                    }
                }
            }

            return;
        }

        /// MouseMoveEventHandler moves child control on the screen
        protected virtual void MouseMoveEventHandler(object sender, MouseEventArgs evnt_args_mouse)
        {
            if (
                !"Core.Windows.Forms.EditableControl"
                .Equals(sender.GetType().ToString())
                &&
                _control_moving != null
                &&
                evnt_args_mouse.Button == MouseButtons.Left
               )
            {
                Point pt = Cursor.Position;
                _control_moving.Left =
                                (this.PointToClient(pt)).X
                                -
                                x_offset_on_client_control
                                ;
                _control_moving.Top =
                                (this.PointToClient(pt)).Y
                                -
                                y_offset_on_client_control
                                ;
            }
        }

        /// MouseUpEventHandler detects when and where child control released
        /// and sets the ending point for the move of the child control
        protected virtual void MouseUpEventHandler(object sender, MouseEventArgs evnt_args_mouse)
        {
            if (_control_moving != null &&  evnt_args_mouse.Button == MouseButtons.Left)
            {
               
                int x;
                int y;
                x = ((Control)sender).Location.X;
                y = ((Control)sender).Location.Y;
                Point pt = new Point(x, y);
                _point_end = pt;
                _control_moving.Location = _point_end;
                _control_moving = null;
                SerializeDataSet();
             }
            return;
        }

        private void SerializeDataSet()
        {
            DataColumn clmName = new DataColumn("ControlName");
            DataColumn clmxpos = new DataColumn("XPosition");
            DataColumn clmypos = new DataColumn("YPosition");
            if (tblCoordinate.Columns.Count == 0)
            {
                tblCoordinate.Columns.Add(clmName);
                tblCoordinate.Columns.Add(clmxpos);
                tblCoordinate.Columns.Add(clmypos);
                ds.Tables.Add(tblCoordinate);
            }
            DataRow tblrow;
            foreach (Control cnt in this.Controls)
            {
                tblrow = tblCoordinate.NewRow();
                    tblrow["ControlName"] = cnt.Name;
                    tblrow["XPosition"] = cnt.Location.X;
                    tblrow["YPosition"] = cnt.Location.Y;
                tblCoordinate.Rows.Add(tblrow);
            }
            TextWriter writer = new StreamWriter(strFileName);
            ser.Serialize(writer, ds);
            writer.Close();
        }
        
        private void DeSerialize()
        {
            string strcntName = "";
            int intxPos;
            int intyPos;
            FileInfo fleMembers = new FileInfo(strFileName);
            if(fleMembers.Exists==true)
            {
                DataSet dsDeSerialize = new DataSet();
                XmlSerializer mySerializer = new XmlSerializer(typeof(DataSet));
                FileStream myFileStream = new FileStream(strFileName, FileMode.Open);
                dsDeSerialize = (DataSet)mySerializer.Deserialize(myFileStream);
                if (dsDeSerialize.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDeSerialize.Tables[0].Rows.Count; i++)
                    {
                        strcntName = dsDeSerialize.Tables[0].Rows[i][0].ToString();
                        intxPos = Convert.ToInt32(dsDeSerialize.Tables[0].Rows[i][1].ToString());
                        intyPos = Convert.ToInt32(dsDeSerialize.Tables[0].Rows[i][2].ToString());
                        foreach (Control c in this.Controls)
                        {
                            if (c.Name == strcntName)
                            {
                                c.Location = new System.Drawing.Point(intxPos, intyPos);
                            }
                        }
                    }
                }
                myFileStream.Close(); 
            }
        }
    }
}
