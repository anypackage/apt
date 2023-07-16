#Requires -Modules AnyPackage.Apt

Describe Get-Package {
    Context 'with no parameters' {
        It 'should return results' {
            Get-Package |
            Should -Not -BeNullOrEmpty
        }
    }

    Context 'with -Name parameter' {
        It 'should return powershell' {
            Get-Package -Name powershell |
            Should -Not -BeNullOrEmpty
        }
    }
}
