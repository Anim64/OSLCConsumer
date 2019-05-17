using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzOSLCReqManager
{
    public class RequirementHandler
    {
        private OSLCManager Manager { get;}
        public string ServiceProviderCatalog { get; private set; }
        public string ServiceProvider { get; private set; }
        

        public RequirementHandler(string server, string jtsServer, string login, string password, string projectName, float version = 3.0f)
        {
            this.Manager = new OSLCManager(server, jtsServer, login, password, projectName, version);

            this.ServiceProviderCatalog = Manager.GetServiceProviderCatalog();
            this.ServiceProvider = Manager.GetServiceProvider(ServiceProviderCatalog);

        }

        public void ChangeProjectArea(string newProjectArea)
        {
            this.Manager.ProjectName = newProjectArea;
            this.ServiceProvider = Manager.GetServiceProvider(ServiceProviderCatalog);
        }

        public bool CreateRequirement()
        {
            return CreateRequirement(null);
        }

        public bool CreateRequirement(string parentFolder)
        {
            return this.Manager.CreateRequirement(this.ServiceProvider, parentFolder);
        }

        

        
    }
}
