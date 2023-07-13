// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System;
using System.Management.Automation;
using static AnyPackage.Provider.PackageProviderManager;

namespace AnyPackage.Provider.Apt
{
    public sealed class Init : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        private readonly Guid _id = new Guid("1bb6b4e7-63a3-48f9-85f0-b5a411028f8a");
        
        public void OnImport()
        {
            RegisterProvider(_id, typeof(AptProvider), "AnyPackage.Apt");
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            UnregisterProvider(_id);
        }
    }
}