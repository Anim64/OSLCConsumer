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
            this.RmPropertyURI = server + "types/";
            this.ShapeURI = shape;

            RmLiteralProperties = new Dictionary<string, string>();
            RmResourceProperties = new Dictionary<string, string>();
        }

        internal XDocument WriteXML()
        {
            XDocument xDoc = new XDocument();

            //         xDoc.Add("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            //         xDoc.Add("<rdf:RDF\n");
            //         xDoc.Add("\t\txmlns:oslc_rm=\"http://open-services.net/ns/rm#\"\n");
            //         xDoc.Add("\t\txmlns:dc=\"http://purl.org/dc/terms/\"\n");
            //         xDoc.Add("\t\txmlns:oslc=\"http://open-services.net/ns/core#\"\n");
            //         xDoc.Add("\t\txmlns:nav=\"http://jazz.net/ns/rm/navigation#\"\n");
            //         xDoc.Add("\t\txmlns:rm_property=\"" + this.RmPropertyURI + "\"\n");
            //         xDoc.Add("\t\txmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">\n");
            //         xDoc.Add("\t<oslc_rm:Requirement\n");
            //         xDoc.Add("\t\txmlns:rm_jazz=\"http://jazz.net/ns/rm#\"\n");

            //         if (this.Uri != null)
            //         {
            //xDoc.Add("\t\trdf:about=\"" + this.Uri + "\">\n");
            //         }
            //         else
            //         {
            //xDoc.Add(">\n");
            //         }

            Console.WriteLine(xDoc.Document.ToString());

            return null;
        }
        internal XDocument WriteXML(Dictionary<string, XNamespace> Namespaces)
        {
            XDocument xDoc = new XDocument();

            //         xDoc.Add("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            //         xDoc.Add("<rdf:RDF\n");
            //         xDoc.Add("\t\txmlns:oslc_rm=\"http://open-services.net/ns/rm#\"\n");
            //         xDoc.Add("\t\txmlns:dc=\"http://purl.org/dc/terms/\"\n");
            //         xDoc.Add("\t\txmlns:oslc=\"http://open-services.net/ns/core#\"\n");
            //         xDoc.Add("\t\txmlns:nav=\"http://jazz.net/ns/rm/navigation#\"\n");
            //         xDoc.Add("\t\txmlns:rm_property=\"" + this.RmPropertyURI + "\"\n");
            //         xDoc.Add("\t\txmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">\n");
            //         xDoc.Add("\t<oslc_rm:Requirement\n");
            //         xDoc.Add("\t\txmlns:rm_jazz=\"http://jazz.net/ns/rm#\"\n");

            //         if (this.Uri != null)
            //         {
            //xDoc.Add("\t\trdf:about=\"" + this.Uri + "\">\n");
            //         }
            //         else
            //         {
            //xDoc.Add(">\n");
            //         }
            xDoc.Add(new XElement("xml", new XAttribute("version", "1.0"), new XAttribute("encoding", "UTF-8")
                ));
            xDoc.Root.Add(
                new XElement(Namespaces["rdf"]+"RDF", 
                    new XAttribute(XNamespace.Xmlns + "rdf", Namespaces["rdf"]),
                    new XAttribute(XNamespace.Xmlns + "oslc_rm", Namespaces["oslc_rm"]),
                    new XAttribute(XNamespace.Xmlns + "dc", Namespaces["dc"]),
                    new XAttribute(XNamespace.Xmlns + "oslc", Namespaces["oslc"]),
                    new XAttribute(XNamespace.Xmlns + "nav", Namespaces["nav"]),
                    new XAttribute(XNamespace.Xmlns + "rm_property", this.RmPropertyURI),
                    new XElement(Namespaces["oslc_rm"]+"Requirement",
                        new XAttribute(XNamespace.Xmlns+"rm_jazz", "http://jazz.net/ns/rm#"))
                ));
            if (this.Uri.Length > 1)
            xDoc.Descendants(Namespaces["oslc_rm"] + "Requirement").FirstOrDefault().Add(new XAttribute(Namespaces["rdf"] + "about", this.Uri));

            Console.WriteLine(xDoc.Document.ToString());

            return null;
        }
    }
}
