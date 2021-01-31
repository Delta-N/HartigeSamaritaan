# Improving this documentation

Documentation for the application is written in Markdown and build using DocFX.  
To improve the documentation follow the steps below:

### Prerequisites 
- Basic knowlege of Git, Github and [markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)
- [DocFX](https://dotnet.github.io/docfx/) 


### 1. Pull the latest version of the master branch to your machine.
You can use git bash or any other tool to do this.  

You cannot directly edit the master or develop branches.  
In order to update the documentation you need to create a branch of the master.   
Once created you can move on to the next step.

### 2. Create/update documentation.
All documentation is located in the *Roosterplanner.Documentation* folder.  
The API-documentation is generated automatically by docFX and should not be changed manually.  
All other forms of documentation are editable.


You can add categories to the header in the upper level toc.yml file.  
(*Roosterplanner.Documentation/toc.yml*)  
Simply specify the name of the header you want to add and specify the href.  
The href should direct to a folder with a toc.yml file in it.

For example:  
*- name: Articles   
href: articles/*  
Leads to: *Roosterplanner.Documentation/articles/toc.yml*

You can specify all subheaders in this second layer toc.yml file.  
Don't forget to fill in the href here as well. It each href should lead to a markdown file. (*example.md*).


Finally: create your documentation in the file using the markdown syntax.

### 3. Build the project.
Once you're finished with editing the documentation, it is time to build the project.  
The correct output paths are already specified, you just need to run the following commands in any terminal:  

- cd c:\\'pathToSourceFiles'\HartigeSamaritaan\Roosterplanner.Documentation    
- c:\\'pathToDocFX'\docfx.ex --serve


The documentation will now be build and the website, where you can view the documentation, will be [served](http://localhost:8080).

### 4. Create a pullrequest.
Finally it is time to push your branch to Github and create a pull request to master.  
Again: Use the tools that you are most comfortable with to do this.

