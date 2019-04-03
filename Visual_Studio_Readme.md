#  Utilizing Microsoft Visual Studio 2017 Community Edition with DartAssistant.

## Description

DartAssistant was developed using Microsoft Visual Studio.  The application can be utilized for testing purposes inside Miscrosoft Visual Studio 2017 Community Edition utilizing these instructions.

## Instructions
* Download Microsoft Visual Studio for your platform of choice (either Windows or OSX).  Visit https://visualstudio.microsoft.com/vs/community/ to download.

* Install the Visual Studio using the install Wizard.  Make sure during the install process to have at least `Mobile Development with .NET` is a selected `Workloads`.

* Once installation is complete, you need to connect Visual Studio to the DartAssistant Github repository.  This is accomplished by going to the `Version Control` menu and select `Checkout`.  In the `Checkout` Dialog box and enter the following:
  * Type: `Git`
  * Name: `DartAssistant Repo`
  * Url: `https://github.com/jkc0019/DartAssist_561.git`
  * Select `OK`
  * Select `Checkout`

* The source code will now be loaded into Visual Studio.  The application now needs to be compiled before utilizing Visual Studio to test the application.  Click on the `DartAssistant.Android` on the left menu of Visual Studio and then Select `Build` from the menu, and then `Build All`.  

* The application can now be ran inside of Visual Studio's Android Emulator.  This is accomplished by selecting `Run` from menu and then `Start without Debugging`.  Note: This process might take several minutes depending on your machines available resources.

* After the Android Emulator is up and running, you will see the DartAssistant Application running.
