# mutantes

## Análisis de cobertura de código
```powershell
#Ejecución de test unitarios
dotnet test .\Mutants.Testing.Unit\ --results-directory:.\Test --collect:"Code Coverage"

#Conversión de *.coverage a xml
& "$env:userprofile\.nuget\packages\microsoft.codecoverage\16.4.0\build\netstandard1.0\CodeCoverage\CodeCoverage.exe" analyze /output:.\Test\UnitTesting.coveragexml  .\Test\78bf6fe2-9434-46f1-962d-60fe69ac80ca\AZ22207_SP000LP796_2020-01-07.19_15_59.coverage

#Instalo la herramienta global reportgenerator
dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.4.0

#Generación de reporte html de cobertura
reportgenerator "-reports:UnitTesting.coveragexml" "-targetdir:.\Html" -reporttypes:Html
```

