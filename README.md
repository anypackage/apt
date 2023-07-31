# AnyPackage.Apt

[![gallery-image]][gallery-site]
[![build-image]][build-site]
[![cf-image]][cf-site]

[gallery-image]: https://img.shields.io/powershellgallery/dt/AnyPackage.Apt
[build-image]: https://img.shields.io/github/actions/workflow/status/anypackage/apt/ci.yml
[cf-image]: https://img.shields.io/codefactor/grade/github/anypackage/apt
[gallery-site]: https://www.powershellgallery.com/packages/AnyPackage.Apt
[build-site]: https://github.com/anypackage/apt/actions/workflows/ci.yml
[cf-site]: https://www.codefactor.io/repository/github/anypackage/apt

`AnyPackage.Apt` is an Advanced Package Tool (APT) provider for AnyPackage.

## Install AnyPackage.Apt

```PowerShell
Install-PSResource AnyPackage.Apt
```

## Import AnyPackage.Apt

```PowerShell
Import-Module AnyPackage.Apt
```

## Sample usages

### Get installed packages

```PowerShell
Get-Package -Name 7zip
```

### Find available packages

```PowerShell
Find-Package -Name 7zip
```
