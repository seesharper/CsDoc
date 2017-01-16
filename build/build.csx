
#load "common.csx"

using System.IO;

private const string projectName = "CsDoc";


private const string csharpProjectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";


string pathToBuildDirectory = @"tmp/";
private string version = "1.0.2";

private string fileVersion = Regex.Match(version, @"(^[\d\.]+)-?").Groups[1].Captures[0].Value;

WriteLine("CsDoc version {0}" , version);

Execute(() => InitializBuildDirectories(), "Preparing build directories");
Execute(() => RenameSolutionFiles(), "Renaming solution files");
Execute(() => PatchAssemblyInfo(), "Patching assembly information");
Execute(() => PatchProjectFiles(), "Patching project files");
Execute(() => RestoreNuGetPackages(), "NuGet");
Execute(() => BuildAllFrameworks(), "Building all frameworks");
//Execute(() => RunAllUnitTests(), "Running unit tests");
//Execute(() => AnalyzeTestCoverage(), "Analyzing test coverage");
Execute(() => CreateNugetPackages(), "Creating NuGet packages");

private void CreateNugetPackages()
{
	string pathToNuGetBuildDirectory = Path.Combine(pathToBuildDirectory, "NuGetPackages");
	DirectoryUtils.Delete(pathToNuGetBuildDirectory);
	
				
	Execute(() => CopyBinaryFilesToNuGetLibDirectory(), "Copy binary files to NuGet lib directory");
				
	Execute(() => CreateBinaryPackage(), "Creating binary package");
    string myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    RoboCopy(pathToBuildDirectory, myDocumentsFolder, "*.nupkg");		
}

private void CopyBinaryFilesToNuGetLibDirectory()
{	
	CopyBinaryFile("NET46", "net46");			
}


private void CreateBinaryPackage()
{	    
	string pathToMetadataFile = Path.Combine(pathToBuildDirectory, "NugetPackages/Binary/package/CsDoc.nuspec");
	PatchNugetVersionInfo(pathToMetadataFile, version);
	NuGet.CreatePackage(pathToMetadataFile, pathToBuildDirectory);
}

private void CopyBinaryFile(string frameworkMoniker, string packageDirectoryName)
{
	string pathToMetadata = "./";
	string pathToPackageDirectory = Path.Combine(pathToBuildDirectory, "NugetPackages/Binary/package");
	RoboCopy(pathToMetadata, pathToPackageDirectory, "csdoc.nuspec");
	string pathToBinaryFile =  ResolvePathToBinaryFile(frameworkMoniker);
	string pathToDestination = Path.Combine(pathToPackageDirectory, "tools/" + packageDirectoryName);
	RoboCopy(pathToBinaryFile, pathToDestination, "*.*");
}

private string ResolvePathToBinaryFile(string frameworkMoniker)
{
	var pathToBinaryFile = Directory.GetFiles("tmp/" + frameworkMoniker + "/Binary/CsDoc/bin/Release","CsDoc.Exe", SearchOption.AllDirectories).First();
	return Path.GetDirectoryName(pathToBinaryFile);		
}

private void BuildAllFrameworks()
{		
	Build("Net46");	    		
}

private void Build(string frameworkMoniker)
{
	var pathToSolutionFile = Directory.GetFiles(Path.Combine(pathToBuildDirectory, frameworkMoniker + @"\Binary\"),"*.sln").First();	
	WriteLine(pathToSolutionFile);
	MsBuild.Build(pathToSolutionFile);	
}

private void BuildDotNet()
{		
	string pathToProjectFile = Path.Combine(pathToBuildDirectory, @"netstandard16/Binary/CsDoc/project.json");
	DotNet.Build(pathToProjectFile, "netstandard16");
}

private void RestoreNuGetPackages()
{		
	RestoreNuGetPackages("net46");    			
}

private void RestoreNuGetPackages(string frameworkMoniker)
{
	string pathToProjectDirectory = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"/Binary/CsDoc");
	NuGet.Restore(pathToProjectDirectory);

	pathToProjectDirectory = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"/Binary/CsDoc.Tests");
	NuGet.Restore(pathToProjectDirectory);
	
    //NuGet.Update(GetFile(Path.Combine(pathToBuildDirectory, frameworkMoniker, "Binary"), "*.sln"));        
}

private void RunAllUnitTests()
{	
	DirectoryUtils.Delete("TestResults");
	Execute(() => RunUnitTests("Net46"), "Running unit tests for Net46");		
}

private void RunUnitTests(string frameworkMoniker)
{
	string pathToTestAssembly = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"/Binary/CsDoc.Tests/bin/Release/CsDoc.Tests.dll");
	string testAdapterSearchDirectory = Path.Combine(pathToBuildDirectory, frameworkMoniker, @"Binary/packages/");
    string pathToTestAdapterDirectory = ResolveDirectory(testAdapterSearchDirectory, "Fixie.VisualStudio.TestAdapter.dll");
	MsTest.Run(pathToTestAssembly, pathToTestAdapterDirectory);	
}

private void AnalyzeTestCoverage()
{		
	Execute(() => AnalyzeTestCoverage("NET46"), "Analyzing test coverage for NET46");
}

private void AnalyzeTestCoverage(string frameworkMoniker)
{	
    string pathToTestAssembly = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"/Binary/CsDoc.Tests/bin/Release/CsDoc.Tests.dll");
	string pathToPackagesFolder = Path.Combine(pathToBuildDirectory, frameworkMoniker, @"Binary/packages/");
    string pathToTestAdapterDirectory = ResolveDirectory(pathToPackagesFolder, "xunit.runner.visualstudio.testadapter.dll");		
    MsTest.RunWithCodeCoverage(pathToTestAssembly, pathToPackagesFolder,pathToTestAdapterDirectory, "CsDoc.dll");
}

private void InitializBuildDirectories()
{
	DirectoryUtils.Delete(pathToBuildDirectory);		
	Execute(() => InitializeNugetBuildDirectory("NET46"), "Preparing Net46");		
}

private void InitializeNugetBuildDirectory(string frameworkMoniker)
{
	var pathToBinary = Path.Combine(pathToBuildDirectory, frameworkMoniker +  "/Binary");		
    CreateDirectory(pathToBinary);
	RoboCopy("../src", pathToBinary, "/e /XD bin obj .vs NuGet TestResults packages");	
						
	if (frameworkMoniker.StartsWith("NETSTANDARD"))
	{
		var pathToJsonTemplateFile = Path.Combine(pathToBinary, "CsDoc/project.json_");
		var pathToJsonFile = Path.Combine(pathToBinary, "CsDoc/project.json");
		File.Move(pathToJsonTemplateFile, pathToJsonFile);
		string pathToLightInject = Path.Combine(pathToBinary, "CsDoc/LightInject/LightInject.cs");
		ReplaceInFile(@".*\[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage\]\r\n",string.Empty,pathToLightInject);	
		// Do another replace to make this work on AppVeyor. Go figure 
		ReplaceInFile(@".*\[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage\]\n",string.Empty,pathToLightInject);	
	}				  
}

private void RenameSolutionFile(string frameworkMoniker)
{
	string pathToSolutionFolder = Path.Combine(pathToBuildDirectory, frameworkMoniker +  "/Binary");
	string pathToSolutionFile = Directory.GetFiles(pathToSolutionFolder, "*.sln").First();
	string newPathToSolutionFile = Regex.Replace(pathToSolutionFile, @"(\w+)(.sln)", "$1_" + frameworkMoniker + "_Binary$2");
	File.Move(pathToSolutionFile, newPathToSolutionFile);
	WriteLine("{0} (Binary) solution file renamed to : {1}", frameworkMoniker, newPathToSolutionFile);		
}

private void RenameSolutionFiles()
{		
	RenameSolutionFile("NET46");		
}

private void PatchAssemblyInfo()
{	
	Execute(() => PatchAssemblyInfo("Net46"), "Patching AssemblyInfo (Net46)");		
}

private void PatchAssemblyInfo(string framework)
{	
	var pathToAssemblyInfo = Path.Combine(pathToBuildDirectory, framework + @"\Binary\CsDoc\Properties\AssemblyInfo.cs");	
	PatchAssemblyVersionInfo(version, fileVersion, framework, pathToAssemblyInfo);			
}

private void PatchProjectFiles()
{	
	Execute(() => PatchProjectFile("NET46", "4.6"), "Patching project file (NET46)");    		
}

private void PatchProjectFile(string frameworkMoniker, string frameworkVersion)
{
	var pathToProjectFile = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"/Binary/CsDoc/CsDoc.csproj");
	PatchProjectFile(frameworkMoniker, frameworkVersion, pathToProjectFile);	
	PatchTestProjectFile(frameworkMoniker);
}
 
private void PatchProjectFile(string frameworkMoniker, string frameworkVersion, string pathToProjectFile)
{
	WriteLine("Patching {0} ", pathToProjectFile);	
	SetProjectFrameworkMoniker(frameworkMoniker, pathToProjectFile);
	SetProjectFrameworkVersion(frameworkVersion, pathToProjectFile);		
	SetHintPath(frameworkMoniker, pathToProjectFile);	
}

private void SetProjectFrameworkVersion(string frameworkVersion, string pathToProjectFile)
{
	XDocument xmlDocument = XDocument.Load(pathToProjectFile);
	var frameworkVersionElement = xmlDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == "TargetFrameworkVersion");
	frameworkVersionElement.Value = "v" + frameworkVersion;
	xmlDocument.Save(pathToProjectFile);
}

private void SetProjectFrameworkMoniker(string frameworkMoniker, string pathToProjectFile)
{
	XDocument xmlDocument = XDocument.Load(pathToProjectFile);
	var defineConstantsElements = xmlDocument.Descendants().Where(p => p.Name.LocalName == "DefineConstants");
	foreach (var defineConstantsElement in defineConstantsElements)
	{
		defineConstantsElement.Value = defineConstantsElement.Value.Replace("NET46", frameworkMoniker); 
	}	
	xmlDocument.Save(pathToProjectFile);
}

private void SetHintPath(string frameworkMoniker, string pathToProjectFile)
{	
	ReplaceInFile(@"(.*\\packages\\CsDoc.*\\lib\\).*(\\.*)","$1"+ frameworkMoniker + "$2", pathToProjectFile);	
}

private void PatchTestProjectFile(string frameworkMoniker)
{
	var pathToProjectFile = Path.Combine(pathToBuildDirectory, frameworkMoniker + @"\Binary\CsDoc.Tests\CsDoc.Tests.csproj");
	WriteLine("Patching {0} ", pathToProjectFile);	
	SetProjectFrameworkMoniker(frameworkMoniker, pathToProjectFile);	
}