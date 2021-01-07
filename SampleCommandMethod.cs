using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.IO;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

[assembly: ExtensionApplication(null)]
[assembly: CommandClass(typeof(CommandMethodTemplate.SampleCommandClass))]
namespace CommandMethodTemplate
{
    public class SampleCommandClass
    {
        [CommandMethod("tt")]
        public static void SampleCommandMethod()
        {
            // Current doc
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            //Get Layer
            try
            {
                List<string> position = LayersToList(db);
                MessageBox.Show(string.Join(Environment.NewLine, position));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //GetData
            
        }
        public static List<string> LayersToList(Database db)
        {
            List<string> lstlay = new List<string>();

            LayerTableRecord layer;
            using (Transaction tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    lstlay.Add(layer.Name);
                }

            }
            return lstlay;
        }

        public static List<string> Position(Database db)
        {
            List<string> lstlay = new List<string>();

            using (Transaction tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockReference lt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockReference;
                lstlay.Add(lt.Position.X.ToString());

            }
            return lstlay;
        }

        [CommandMethod("PickObject")]
        public static List<Entity> SelectedObject()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<Entity> objs = new List<Entity>();
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                PromptSelectionResult prompt = ed.GetSelection();
                if (prompt.Status == PromptStatus.OK)
                {
                    SelectionSet selectionSet = prompt.Value;
                    foreach (SelectedObject obj in selectionSet)
                    {
                        if (obj != null)
                        {
                            Entity o = tran.GetObject(obj.ObjectId, OpenMode.ForWrite) as Entity;
                            objs.Add(o);
                        }
                    }
                }
                tran.Commit();
            }
            return objs;
        }
    }
}
