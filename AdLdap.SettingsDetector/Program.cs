namespace AdLdap.SettingsDetector
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.ActiveDirectory;
    using System.Linq;
    using System.Security.Principal;

    class Program
    {
        private static string resultTemplate = "{{ " +
            "\"domainDNS\": \"{0}\", " +
            "\"baseDN\": \"{1}\", " +
            "\"domainController\": \"{2}\", " +
            "\"userLoggedIn\": \"{3}\" " +
            "\"windowsIdentity\": \"{4}\" " +
        "}}";

        private static string errorTemplate = "{{ " +
            "\"error\": \"{0}\" " +
        "}}";

        static void Main(string[] args)
        {
            try
            {
                var userLoggedIn = Environment.UserName;
                var windowsIdentity = WindowsIdentity.GetCurrent() != null ? WindowsIdentity.GetCurrent().Name : string.Empty;

                var domain = Domain.GetCurrentDomain();
                
                var domainDNS = domain.Name;
                var baseDN = GetBaseDN(domainDNS);
                var domainController = domain.DomainControllers.Count > 0 ? domain.DomainControllers[0].Name : string.Empty;
                
                // Print result
                Console.WriteLine(
                    resultTemplate,
                    domainDNS.CleanForJSON(),
                    baseDN.CleanForJSON(),
                    domainController.CleanForJSON(),
                    userLoggedIn.CleanForJSON(),
                    windowsIdentity.CleanForJSON());
            }
            catch (Exception ex)
            {
                Console.WriteLine(errorTemplate, ex.Message.CleanForJSON());
                Environment.Exit(1);
            }
        }

        private static string GetBaseDN(string domainDNS)
        {
            if (string.IsNullOrEmpty(domainDNS))
            {
                return string.Empty;
            }

            var rootDSE = new DirectoryEntry("LDAP://" + domainDNS);
            return rootDSE.Properties["distinguishedName"].Value as string;
        }
    }
}
