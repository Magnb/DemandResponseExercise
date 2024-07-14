# DemandResponseExercise

This is a microservice-solution for the Demand Reponse Exercise.

## Prerequisites

### Docker: Ensure you have Docker installed on your machine.

You can download and install Docker from Docker's official website.
Docker Compose is included with Docker Desktop, so installing Docker Desktop will also install Docker Compose.

### .NET SDK: Ensure you have the .NET SDK installed on your machine.

You can download it from Microsoft's .NET download page.
Verify the installation by running the following command in your terminal or command prompt:

>dotnet --version

PowerShell (for Windows): The script uses PowerShell commands to start the .NET projects on Windows.

Bash (for Unix-like systems): The script uses Bash commands to start the .NET projects on Unix-like systems (macOS/Linux). 


## Running the solution
To start the InfluxDB, MongoDB and Kafka, start up the docker-containers via the docker-compose file
Navigate to the directory containing the docker-compose.yml file and start up the containers:
>docker-compose up --build

To execute the microservices navigate to the run-all-microservices.sh script and execute it:

>./run-all-microservices.sh

Congratulations, the solution shall now be started.

## Usage
If you started the services via console you should be able to see console logs now.
Now you can navigate to http://localhost:5256/realtime for the application and to http://localhost:5262/swagger/index.html for the swagger-UI for the API.
On the swagger UI you can e.g. create a POST create to /api/consumers/wallbox to create a virtual wallbox. As there is no real wallbox available, there is no data-connection to one - the generated data is simulated.
After adding the device with the POST request you can copy the device-id from the response body and use it to add a schedule via /api/consumers/{your-id}/schedule via the Swagger UI - add a time interval and a value there - e.g. for a charging-period. This would also be used to respond to prices (e.g. to charge under a certain limit for a ToU tarif).
After adding an interval you can see the changed real-time-data on http://localhost:5256/realtime if the current time is in the configured time-interval and you'll see the historical data when you click on the device name. This chart is created from the historical data in the InfluxDB.
