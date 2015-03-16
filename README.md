# DNNExtensions

This is a solution that houses most of the DNN extensions that I manage for the community. It also is 
used to display how a DNN solution should be architected as a best practice, which includes several 
very useful MSBuild scripts.  

---

## Getting Started

1. Update your global gitignore to allow DLL, BAK, and PDB files.
2. You should create a website folder where your project will be. This can be anywhere on your system.  My environment is in 
C:\Websites\Development\
3. Clone the repository into that folder
4. Create a new folder in the root:  _Website_
5. Install DNN 7.3.4 into the Website folder just like you would for any other DNN site
6. Update IIS and your local HOSTS file to point to http://dnndev.me
7. Write code.

---

## Dependencies

### DNN / DotNetNuke

This solution is currently being built against DNN 07.03.04.  It is the latest production version that's stable 
enough to use. When 07.04.01 comes, we might upgrade to that.

http://dotnetnuke.codeplex.com/releases/view/137325

Be sure that you get the file permissions properly assigned to the folders when you install DNN.

### Visual Studio Extensions

The following Visual Studio Extensions are currently installed and being used in my environment, but are not 
necessary to work on the project.  There are more, but these are the only ones that are relevant to this project.

* GhostDoc
* Guidinserter2
* Microsoft ASP.NET and Web Tools
* NuGet Package Manager
* ReSharper 9 (not free, except to active open source developers)
* UIMap Toolbox
* Web Essentials

---

## Checking in Code

Please fork the project and submit a pull request.

## How to Build

You should probably not build the at the solution level, since this will also build the website, but it won't 
hurt anything if you do.  It will just take longer.

In general, you should be building from the specific module project you're currently working on.

### What happens when I build?

Building in __DEBUG__ mode will compile the project/solution as you'd expect, but an MS Build script will also 
move the module files into the appropriate Website\DesktopModules\ folder as well.  

Building in __RELEASE__ mode will _not_ move the project files, but it will package up the respective module 
in an Install and Source package that can be used to install on another DNN site for testing or deployment. The 
resulting packages will be found in the following directory.

\Website\Install\Module\
\Website\Install\Skin\
\Website\Install\Widget\

This is VERY important to know.  Each project has a .Build file that properly maps it's files that need to 
be moved into the website folder.  

You can reverse engineer this to see how it works by referencing each individual Module.Build file.  In order 
to add this to your own module project, copy the build file, make the appropriate changes, and then add the 
following lines of code to your project file.

```xml
  <ItemGroup> 
    <Content Include="Module.Build"> 
      <SubType>Designer</SubType> 
    </Content> 
  </ItemGroup> 
  <Import Project="Module.Build" /> 
```

## Debugging

Debugging should be done using the "Attach to Process" method.

## Solution Architecture

You'll find a database backup in the Assets folder.  This should be used as your starting point, as defined in 
the Getting Started section above.

### Configuration Files Solution Folder

This solution folder contains the common configuration files for the entire solution, such as the web.config.

### Libraries Solution Folder

This folder should contain all of the class library projects that will be used across the various modules.  
These projects should contain any code that will be common across two or more modules.

### Modules Solution Folder

This contains the various module projects that will be placed onto pages.

### Website Project

The website project is only used for reference.  We should not be making any core code changes to DNN itself.
