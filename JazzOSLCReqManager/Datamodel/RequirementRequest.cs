using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JazzOSLCReqManager.Datamodel
{
    internal class RequirementRequest
    {
        internal string Uri { get; set; }
        internal string DcTitle { get; set; }
        internal string DcIdentifier { get; set; }
        internal string DcType { get; set; }
        internal string DcDescription { get; set; }
        internal string DcSubject { get; set; }
        internal string DcCreator { get; set; }
        internal DateTime DcModified { get; set; }
        internal Dictionary<string, string> RmLiteralProperties { get; set; }
        internal Dictionary<string, string> RmResourceProperties { get; set; }
        internal string RmPropertyURI { get; set; }
        internal string ShapeURI { get; set; }
        internal string ParentFolder { get; set; }

        static string RM_TITLE = "dc:title";
	    static string RM_IDENTIFIER = "dc:identifier";
	    static string RM_DESCRIPTION = "dc:description";
	    static string RM_CREATOR = "dc:creator";
	    static string RM_MODIFIED = "dc:modified";
	    static string RM_ABOUT = "rdf:about";
	    static string RDF_RESOURCE = "rdf:resource";
	    static string RM_PROPERTY = "rm_property";
	    static string INSTANCE_SHAPE = "oslc:instanceShape";
	    static string RM_PARENT = "nav:parent";
	    static string RM_JAZZ = "rm_jazz";  // New for RM 4.0.1 - Vocabulary Support

        
        internal RequirementRequest(string server, string uri, string shape)
        {
            
            this.Uri = uri;
            this.RmPropertyURI = server + "/types/";
            this.ShapeURI = shape;

            RmLiteralProperties = new Dictionary<string, string>();
            RmResourceProperties = new Dictionary<string, string>();
        }

        internal XDocument WriteXML()
        {
            return null;
        }
    }
}
