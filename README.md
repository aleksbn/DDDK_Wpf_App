# DDDK WP frontend app
Hello and welcome to the instruction manual for the DDDK application! This app is about managing data for the bloood donors club. It's managind blood donations, events, location of the events etc, together with data about the donors. App is not hosted anywhere, but there are others that are, and you can check them out at my portfolio site at [https://aleksandar-matic.web.app/skillsandprojects](https://aleksandar-matic.web.app/skillsandprojects). Once you open up the page, scroll a little bit down and check those with web page links.

# Backend
Backend is actually an asp.net web api made in C#. Database is already included inside of a project. 
1. Download code from [backend repo](https://github.com/aleksbn/dddk-backend-asp.net).
2. Next step is getting visual studio. In case you don't have one, head down to the [VS download page](https://visualstudio.microsoft.com/downloads/), download Community version and install it on your computer. If you already have it, you can skip this step.
3. You need .net 7 for this app to be able to run. If you don't have it, go to the [.net 7 download page](https://dotnet.microsoft.com/en-us/download/dotnet/7.0), download version for your OS and install it.

# Frontend
1. Now, download code from this repo and open the solution file, just like you did in the previous step. If you already installed .net 7 and Visual Studio, you're good to go.

# Instructions
1. Next, open backend solution file and press F5, or go to Debug menu bar option and select Start Debugging. And that’s it! Web browser window with every endpoint configuration displayed for testing purposes will show up, command prompt with serilog displaying what’s going on in the background will also show up, and you’re good to go!

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/f1dfdaf7-2947-435c-8631-5f702de2a46b)
 
2. While having backend running, press F5 inside of a frontend and you'll start the app. You'll get the login window:

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/a16c8b2c-61dc-473b-9d4d-acb5284b507f)

3. Type in email and password. For this purpose, considering database is already generated and loaded, type in the following data:

Username: admin@dddk.org

Password: Admin@417302

If you’re of the administrator role, and if you used previous credentials to log in, which you most certainly did, you’ll get the following menu:
![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/920cbc99-c875-4a60-80e1-92b9e78e7688)

The difference between moderators and administrators is option number one: Users. If you log in as a moderator, you won’t see it at all and won’t be able to manipulate with users allowed to use the app. That’s it. Everything else is the same for both roles. If you click on the Users option, you’ll get the following appearance:

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/78de19b7-69cb-4c32-b318-21bc85dfb9b5)

Once you click on any of the names rounded with red square, interface on the right side will fill in with data and you’ll be able to delete the user or change his username or password, or change the role. If you press New button, the interface will be enabled and you’ll be able to input any data of a new user you’re registering. Everything is pretty self explanatory once you start using it.

If you select Donators, Donations or Locations, the interface will pretty much be the same, and you’ll be able to do the very same CRUD operations like in the previous example. Here’s an example of loaded interface for Donators option:

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/fdd1209c-1161-46df-aa0f-52d717d8f93a)

Bear in mind Date type is used in serbian/bosnian/croatian etc. format: dd.MM.yyyy. Of cousre, inside of a database, it is saved in standard SQL format: MM/dd/yyyy. If you click on Donation Events option on the left, things are a tittle bit different and the menu looks like this:

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/0e906d70-5777-46b8-b353-b55f58b92358)

As you can see, it’s similar to the previous ones, but you can see every person donating blood on that particular event. In case you want to edit donators, just click on Edit button and select everyone who donated blood on that specific donation event. Same goes when you’re adding a new donating event. In case you delete certain donation event, every donation connected to that event will also be deleted.

If you click on Searches on the left, you’ll be able to search all the data based on certain criterias.

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/10cda291-9572-47a6-bbbe-d246b128f79c)

By donating availability will allow you to search for any donator who is of certain blood type, or is able to donate (meaning it passed more than 4 months since previous donating), or both.

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/8a6fd8e2-c957-47ed-89b9-2a48ed560e6e)

If you select By donator option, input some search data (in following example it’s “Aleks” as a part of the first name), and then select any name from the list displayed below input fields, you’ll get their personal data, as well as every donation made by that donator.

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/34ed63aa-2fe4-4f62-a391-f50c3f384a6b)

If you select By donation event, search for any and click on it, you’ll get every donator donating blood at that specific event, as well as data about the event itself.

![image](https://github.com/aleksbn/dddk-frontend-wpf/assets/44110941/e6b2bff7-0738-4c81-bc7e-4878f65acc7f)

And that’s it! Small, simple, yet very useful application. It’s not perfectly pretty, but it’s more than functional. In case you want to see my other apps as well, head down to my portfolio web site mentioned at the start of this manual, and go to the Skills and projects page.

# Architecture considerations
The app has 2 layers:
1. Backend is consisting of two parts. First one is SQL database, and this project comes with DB integrated inside of a project. Of course, this is not a good practice as DB should be hosted on the internet for security reasons, but for this size of a project, it's more than enough. Second part of the app are endpoints. Key points here are usage of the following techniques:
- Using Microsoft ASP.NET Core identity system for storing user data
- Using Auto Mapper libary for converting entity objects to DTOs and vice-versa
- Using Code First approach with migrations
- Using Repository pattern
- Using altered Unit Of Work pattern
- Using JWT token for fast authentication
2. Frontend is built with WPF framework as a windows application. Key points here are:
- Dynamically updating interface by using observable collections
- Using custom designed warehouse with Data Access Layer classes builded upon generic DAL class for easier code readability and maintainability
- Using custon controlls for better display od user data
