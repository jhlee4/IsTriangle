using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace IsTriangle
{
    [System.Runtime.InteropServices.Guid("5e281339-92e6-44c4-8b09-286e65c1060e")]
    public class IsTriangleCommand : Command
    {
        public IsTriangleCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static IsTriangleCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "IsTriangleCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---

            Rhino.DocObjects.ObjRef c;
            RhinoGet.GetOneObject("Select polygon(s)",false,Rhino.DocObjects.ObjectType.Curve,out c);
            Curve shape = c.Curve();

            Curve simplifiedCurve = Simplify.Simplification(shape,13);
            doc.Objects.AddCurve(simplifiedCurve);
            bool isTri = TriIdentifier.Identify(simplifiedCurve);
            if (isTri == true)
            {
                RhinoApp.WriteLine("This polygon is a triangular polygon");
            }
            else
            {
                RhinoApp.WriteLine("This polygon is not a triangular polygon");
            }

            // ---

            return Result.Success;
        }
    }
}
