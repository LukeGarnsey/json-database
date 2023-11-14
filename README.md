# JSON file database #

### Overview ###

This is a project that helps you quickly (Read/Write)/(Save/Load) Model class objects and setting them up in a database format.
It is for quickly testing viability of data structures and more easily visualizing the data within your project.
Master branch contains the scripts and plugin required for use. 
The behavior in the main scripts are independent of Unity engine, though the example project
uses Unity and it is setup for unitypackage import.
This setup allows for models to be built from GUID references to other JSON files, models can reference a JSON file
and if data is changed in the referenced file, all models referencing that file would be able to update their info.
The system is like a non-relational database.

### Project Notes ###

'Model.cs': This is required to be the base class of all Data Types to be saved.
Simply extend 'Model' from any class and that class will have access to all behavior.
To save a Model call 'Save()' and everything will be handled from there.  
To load all Models of Type use 'Model.AllItems<type>()' This returns an array of all specified 'type'.
The system will create a root folder called 'JUDData' in the unity project where it stores all of the json files and folder structures.
Folder structure is based on 'namespace and class name of all saved 'Model.cs' objects.  

### Example Branch ###
Open the easy scene and run the game, you will see the changes in your Unity project (after reload).
Example scripts show required setup for any class extending 'Model'

### Developer ###
Name                | E-mail
:-------------------|:------------------------
Luke Garnsey        | <lukegarnsey@gmail.com>