#!/bin/bash

# Function to run a .NET project
run_dotnet_project() {
    local project_path=$1
    if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" || "$OSTYPE" == "cygwin" ]]; then
        # Windows-specific command
        powershell.exe -Command "Start-Process -NoNewWindow -FilePath 'dotnet' -ArgumentList 'run' -WorkingDirectory '$project_path'"
    else
        # Unix-like systems command
        dotnet run --project "$project_path" &
    fi
    
    # Wait for a few seconds before starting the next project
        sleep 3
}

# List of project paths
projects=(
        "../MonitoringApp"
        "../ScheduleManagementApi"
        "../RealTimeDataConsumer"
        "../RealTimeDataGenerator"
)

# Loop through each project and run it
for project in "${projects[@]}"; do
    run_dotnet_project "$project"
done

# Wait for all background processes to finish (Unix-like systems only)
if [[ "$OSTYPE" != "msys" && "$OSTYPE" != "win32" && "$OSTYPE" != "cygwin" ]]; then
    wait
fi
