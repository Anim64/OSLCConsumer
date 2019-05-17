using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzOSLCReqManager.Datamodel
{
    public class RequirementRequest
    {
        public string Uri { get; set; }
        public string DcTitle { get; set; }
        public string DcIdentifier { get; set; }
        public string DcType { get; set; }
        public string DcDescription { get; set; }
        public string DcSubject { get; set; }
        public string DcCreator { get; set; }
        public DateTime DcModified { get; set; }
        public Dictionary<string, string> RmLiteralProperties { get; set; }
        public Dictionary<string, string> RmResourceProperties { get; set; }
        public string RmPropertyURI { get; set; }
        public string ShapeURI { get; set; }
        public string ParentFolder { get; set; }

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

        public RequirementRequest()
        {
            RmLiteralProperties = new Dictionary<string, string>();
            RmResourceProperties = new Dictionary<string, string>();
        }
    }
}
