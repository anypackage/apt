@{
    RootModule = 'AnyPackage.Provider.Apt.dll'
    ModuleVersion = '0.1.0'
    CompatiblePSEditions = @('Core')
    GUID = '0b8cda6d-9f04-4632-bcd3-9e2c1fed0ef9'
    Author = 'Thomas Nieto'
    Copyright = '(c) 2023 Thomas Nieto. All rights reserved.'
    Description = 'APT provider for AnyPackage.'
    PowerShellVersion = '7.0'
    RequiredModules = @(
        @{ ModuleName = 'AnyPackage'; ModuleVersion = '0.5.0' })
    FunctionsToExport = @()
    CmdletsToExport = @()
    AliasesToExport = @()
    PrivateData = @{
        AnyPackage = @{
            Providers = 'Apt'
        }
        PSData = @{
            Tags = @('AnyPackage', 'Provider', 'APT', 'Linux')
            LicenseUri = 'https://github.com/anypackage/apt/blob/main/LICENSE'
            ProjectUri = 'https://github.com/anypackage/apt'
        }
    }
    HelpInfoURI = 'https://go.anypackage.dev/help'
}
