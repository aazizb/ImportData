# ImportData
	What I would Add:
Surely, carry out the implementation of the different tests ; and next I would recommend to add the error handling to make it global error handling.

	How to run the code:

Clone or download the project
Open it using VS Code
Open VS Code integrated terminal
Compile the project by executing: 
	dotnet build
Run the project by passing the file full path as follow:
	 dotnet run -- "C:\...\...\feed-products\capterra.yaml"
The other way round to run the project is to open window command line and navigate to bin->debug folder and run:
	import "C:\...\...\feed-products\capterra.yaml"

	How to deploy the project

An easy way to publish the application is to use VS Code:
Open VS Code integrated terminal (make sure you are in Import folder)
Run the following command: 
	dotnet publish --configuration Release

The other way round is to use built in feature in Visual Studio:
Open the project in Visual studio and make sure the build option is in Release mode instead of Debug mode as it use to be. Within Solution Explorer right click Import project and select Publish from contextual menu and follow the wizard.

	Where to find the code

The code and the database assessment are available at: https://github.com/aazizb/ImportData/tree/master
